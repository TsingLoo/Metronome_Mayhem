using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
        public float firstDelay = 0f;

        private MainInput _mainInput;

        private void Awake()
        {
            DOTween.SetTweensCapacity(1024, 1024);

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
            
            _mainInput.inLevel.showPanel.performed += ctx =>
            {
                //Debug.Log(Panel.Instance.isActiveAndEnabled);
                if (Panel.Instance.isActiveAndEnabled)
                {
                    Panel.Instance.Hide();
                    Time.timeScale = 1.0f;
                    DOTween.timeScale = 1.0f;
                }
                else
                {
                    Panel.Instance.Show(PanelStatus.PAUSE);
                    Time.timeScale = 0.0f;
                    DOTween.timeScale = 0.0f;
                }
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
            StartSong();
        }

        public void ResetGame()
        {
            // RemoveAllCubes();
            // StartSong();
            RemoveAllCubes();
            DOTween.KillAll();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void StartSong()
        {
            readyDelay = 4.0f;
            //DOTween.RestartAll();

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
                
                combo = 0;
                readyCountDown_text.transform.DOScale(0, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
                {
                    readyCountDown_text.gameObject.SetActive(false);
                });
                    
                var beatmap = BeatmapLoader.load(Constants.BeatmapNames.Excelsus);
                var lastBeatTime = 0.0f;
                //firstDelay = 0;
                
                
                foreach (var beats in beatmap)
                {
                    Array.Sort(beats, (a, b) => a.beat - b.beat > 0 ? 1 : -1);
                    firstDelay = Math.Max(firstDelay, _laneLength / beats[0].speed - beats[0].beat);
                    lastBeatTime = Math.Max(lastBeatTime, beats[beats.Length - 1].beat + beats[beats.Length - 1].hold);
                }

                foreach (var beats in beatmap)
                {
                    for (int i = 0; i < beats.Length; i++)
                    {
                        beats[i].beat += firstDelay;
                    }
                }
                
                DOVirtual.DelayedCall(firstDelay, () =>
                {
                    _audioSource.Play();
                },false);
                DOVirtual.DelayedCall(firstDelay + lastBeatTime, () =>
                {
                    // call the panel to show the win panel
                    Panel.Instance.Show(PanelStatus.WIN);
                    // stop the game
                    Time.timeScale = 0;
                },false);

                lanes = beatmap.Select((beats, i) => new Lane(i, inputChannels[i], beats.Select(beat =>
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
        }

        void Update()
        {
            if (lanes != null && lanes.Length > 0)
            {
                timer_text.text = $"Time from song starts: {lanes[0]._laneTime - firstDelay}";
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

        public void DoMissedNote(int sourceIdx)
        {
            //FxManager.Instance.PlayActionFx(sourceIdx,timingGrade.Missed);
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

        public void RemoveAllCubes()
        {
            if(lanes == null) return;
            foreach (var lane in lanes)
            {
                if(lane == null) return;
                Transform spawnTransform = lane._spawnTransform;
                for (int i = spawnTransform.childCount - 1; i >= 0; i--)
                {
                    Destroy(spawnTransform.GetChild(i).gameObject);
                }
            }
        }
    }
}