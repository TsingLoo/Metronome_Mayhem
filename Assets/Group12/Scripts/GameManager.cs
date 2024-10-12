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

        [SerializeField] TMP_Text timer_text;
        public static GameManager Instance { get; private set; }
        
        InputAction[] inputChannels; 
        Lane[] lanes;

        float _laneLength = 25f;
        
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
        }

        private void Start()
        {
            var beatmap = BeatmapLoader.load(Constants.BeatmapNames.Excelsus);
            lanes = beatmap.Select((beats, i) => new Lane(inputChannels[i], beats.Select(beat =>
                new Note(
                    GetComponent<NoteSpawner>().note,
                    speed: beat.speed,
                    pressMoment: beat.beat,
                    releaseMoment: beat.beat + beat.hold,
                    pressMomentPadding: 0.03f,
                    excellentTolerance: 0.05f,
                    goodTolerance: 0.05f,
                    fairTolerance: 0.05f,
                    missingTolerance: 0.2f,
                    laneLength: _laneLength
                )).ToArray(), transform.GetChild(i), _laneLength)
            ).ToArray();

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
            
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            combo = 0;
        }

        void Update()
        {
            timer_text.text = $"{lanes[0]._laneTime}";
        }

        private void OnDestroy()
        {
            _mainInput.Disable();
        }

        public void noNote()
        {
            // remove combo, decrease health
            MyHealth.ChangeHealth(-5);
            //combo = 0;
        }

        public void missedNote()
        {
            // add a Miss, remove combo, decrease health
            missedNum++;
            MyHealth.ChangeHealth(-5);
            //combo = 0;
        }

        public void okNote()
        {
            // add an Ok, remove combo, decrease health
            okNum++;
            MyHealth.ChangeHealth(-3);
            combo++;
            Debug.Log("Combo: " + combo);
        }

        public void greatNote()
        {
            // add a Great, remove combo, decrease health
            greatNum++;
            MyHealth.ChangeHealth(-1);
            combo++;
            Debug.Log("Combo: " + combo);
        }

        public void excellentNote()
        {
            // add an Excellent, decrease health, increase combo
            excellentNum++;
            MyHealth.ChangeHealth(1);
            combo++;
            Debug.Log("Combo: " + combo);
        }

        public void perfectNote()
        {
            // add a Perfect, increase combo\
            perfectNum++;
            MyHealth.ChangeHealth(5);
            combo++;
            Debug.Log("Combo: " + combo);
        }
    }
}