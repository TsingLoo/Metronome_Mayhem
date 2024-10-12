using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Group12
{
    public class GameManager : MonoBehaviour
    {
        public Health MyHealth;
        private int totalNotes;
        [SerializeField] private int missedNum, perfectNum, excellentNum, greatNum, okNum;
        private int combo;
        //add variables that keep track of the current score, combo, number of Good/Bad, etc\

        [SerializeField] AudioSource _audioSource;

        [SerializeField] TMP_Text readyCountDown_text;
        [SerializeField] TMP_Text timer_text;
        public static GameManager Instance { get; private set; }
        
        InputAction[] inputChannels; 
        Lane[] lanes;

        float _laneLength = 25f;

        private float readyDelay = 4f;
        
        private MainInput _mainInput;

        private void Awake()
        {
            DOTween.SetTweensCapacity(1024,1024);
            
            _mainInput = new MainInput();
            _mainInput.Enable();

            inputChannels = new[]
            {
                _mainInput.inLevel.inputChannel0,
                _mainInput.inLevel.inputChannel1,
                _mainInput.inLevel.inputChannel2,
                _mainInput.inLevel.inputChannel3,
                _mainInput.inLevel.inputChannel4,
                _mainInput.inLevel.inputChannel5,
                _mainInput.inLevel.inputChannel6,
                _mainInput.inLevel.inputChannel7,
            };   
            
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {

            DOTween.To(() => readyDelay, x => readyDelay = x, 0f, readyDelay).OnUpdate(
                () =>
                {
                    if (readyDelay - 1 < 0)
                    {
                        readyCountDown_text.text = "Go!";
                    }
                    else
                    {
                        readyCountDown_text.text = (readyDelay - 1).ToString("0.0");
                    }
                }
            ).OnComplete(() =>
            {
                readyCountDown_text.transform.DOScale(0, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
                {
                    readyCountDown_text.gameObject.SetActive(false);
                });
                
                _audioSource.Play();
                var beatmap = BeatmapLoader.load(Constants.BeatmapNames.Excelsus);
                lanes = beatmap.Select((beats, i) => new Lane(inputChannels[i], beats.Select(beat =>
                        new Note(
                            GetComponent<NoteSpawner>().note,
                            speed: beat.speed,
                            pressMoment: beat.beat,
                            releaseMoment: beat.beat + beat.hold,
                            pressMomentPadding: 0.05f,
                            perfectTolerance: 0.03f,
                            excellentTolerance: 0.05f,
                            greatTolerance: 0.1f,
                            okTolerance: 0.14f,
                            missedTolerance: 0.3f,
                            laneLength: _laneLength
                        )).ToArray(), transform.GetChild(i), _laneLength)
                ).ToArray();
            });
            
                

            

            //_audioSource.Play();
            
            // var firstLane = new Lane(
            //     inputChannels[0], // Use the first input channel
            //     beatmap[0].Select(beat => new Note(
            //         GetComponent<NoteSpawner>().note,
            //         speed: beat.speed,
            //         pressMoment: beat.beat,
            //         releaseMoment: -1,
            //         //releaseMoment: beat.hold == 0 ? -1: beat.beat + beat.hold,
            //         pressMomentPadding: 0.15f,
            //         excellentTolerance: 0.05f,
            //         goodTolerance: 0.12f,
            //         fairTolerance: 0.25f,
            //         missingTolerance: 0.6f
            //     )).ToArray(),
            //     transform.GetChild(0) // Use the first child
            // );

            //lanes = new[] { firstLane };

            combo = 0;
        }

        void Update()
        {
            if (lanes != null && lanes.Length > 0)
            {
                timer_text.text = $"{lanes[0]._laneTime}";   
            }
        }

        private void OnDestroy()
        {
            _mainInput.Disable();
        }

        public int getCombo()
        {
            return combo; 
        }

        public void noNote()
        {
            // remove combo, decrease health
            MyHealth.ChangeHealth(-5);
            combo = 0;
        }

        public void DoMissedNote()
        {
            // add a Miss, remove combo, decrease health
            missedNum++;
            MyHealth.ChangeHealth(-5);
            combo = 0;

        }

        public void DoOkNote()
        {
            // add an Ok, remove combo, decrease health
            okNum++;
            MyHealth.ChangeHealth(-3);
            combo++;
            //Debug.Log("Combo: " + combo);
        }

        public void DoGreatNote()
        {
            // add a Great, remove combo, decrease health
            greatNum++;
            MyHealth.ChangeHealth(-1);
            combo++;
            //Debug.Log("Combo: " + combo);
        }

        public void DoExcellentNote()
        {
            // add an Excellent, decrease health, increase combo
            excellentNum++;
            MyHealth.ChangeHealth(1);
            combo++;
            //Debug.Log("Combo: " + combo);
        }

        public void DoPerfectNote()
        {
            // add a Perfect, increase combo\
            perfectNum++;
            MyHealth.ChangeHealth(5);
            combo++;
            //Debug.Log("Combo: " + combo);
        }
    }
}