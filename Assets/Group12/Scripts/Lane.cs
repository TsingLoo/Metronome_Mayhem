using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Group12
{
    
    public class Game
    {
        private float time;
    }

    public class Lane
    {
        Transform _spawnTransform;
        int _currentNoteIdx = -1;

        private float _length;
        private float _lengthPadding = 10f;
        private float _visualOffset;
        
        //Game game;

        private Note[] _notes;
        private float _score;
        private float _currentProgress;
        private InputAction _actionChannel;
        

        #region Timing Definitions

        public float velocity
        {
            get { return _length / (pressFloor - spawnMoment); }
        }

        float fullLength {
            get { return _length + _lengthPadding;  }
        }

        float pressMoment { get { return curNote.pressMoment; } }
        float spawnMoment { get { return curNote.spawnMoment; } }
        float pressFloor { get { return curNote.pressMoment - curNote.pressMomentPadding; } }
        float pressCeil{ get { return curNote.pressMoment + curNote.pressMomentPadding; } }
        float excellentFloor { get { return pressFloor - curNote.excellentTolerance; }}
        float excellentCeil { get { return pressCeil + curNote.excellentTolerance; }}
        float goodFloor { get { return excellentFloor - curNote.goodTolerance; }}
        float goodCeil{get { return excellentCeil + curNote.excellentTolerance; }}
        float fairFloor { get { return goodFloor - curNote.fairTolerance;}}
        float fairCeil { get { return goodCeil + curNote.fairTolerance;}}
        float missingFloor { get { return fairFloor - curNote.missingTolerance; }}
        float missingCeiling { get { return fairCeil + curNote.missingTolerance; }}
        #endregion
        
        private Note curNote { get { return _notes[_currentNoteIdx]; } }
        
        public Lane(InputAction actionChannel, Note[] notes, Transform spawnTransform, float length = 25, float visualOffset = 1f, int noteStartIdx = 0)
        {
            _actionChannel = actionChannel;
            _notes = notes;
         
            _spawnTransform = spawnTransform;
            _length = length;
            _visualOffset = visualOffset;
            
            //_currentNoteIdx = noteStartIdx - 1;
            NextNote(noteStartIdx);
            
            _actionChannel.started += HandlePress;
            _actionChannel.canceled += HandleRelease;

            ScheduleNoteSpawning();
        }

        void HandlePress(InputAction.CallbackContext context)
        {
            //GameObject.Instantiate(new GameObject("ASD"));
            
                    //float adjustedStartTime = (float)(context.startTime - _actionChannel.action.time);

            
            Debug.Log($"[{nameof(HandlePress)}]  {context.action.name} Pressed at {Time.time} " +
                      $"missingFloor: {missingFloor}, missingCeiling: {missingCeiling}, " +
                      $"excellentFloor: {excellentFloor}, excellentCeiling: {excellentCeil} " +
                      $"goodFloor: {goodFloor}, goodCeiling: {goodCeil}" +
                      $"fairFloor: {fairFloor}, fairCeiling: {fairCeil}");

            float actionInGameTime = Time.time;
            
            if(actionInGameTime < missingFloor )
            {
                Debug.Log($"[{nameof(HandlePress)}] Do Nothing ...");
                return;
            } 
            
            if (excellentFloor < actionInGameTime && actionInGameTime < excellentCeil)
            {
                Debug.Log($"[{nameof(HandlePress)}] Excellent Press !!!!");
            }
            else if (goodFloor < actionInGameTime && actionInGameTime < goodCeil)
            {
                Debug.Log($"[{nameof(HandlePress)}] Good Press !!!!");
            } 
            else if (fairFloor < actionInGameTime && actionInGameTime < fairCeil) 
            {
                Debug.Log($"[{nameof(HandlePress)}] Fair Press !!!!");
                
            }else if (actionInGameTime < missingCeiling)
            {
                Debug.Log($"[{nameof(HandlePress)}] Miss Press!!!!");
            }

            Debug.Log($"[{nameof(HandlePress)}] Press {curNote.GetHashCode()}, Try to go to Next Note ... !!!!");
            NextNote();
        } 
        
        void ScheduleNoteSpawning()
        {
            foreach (var note in _notes)
            {
                float delay = note.spawnMoment - Time.time;
                if (delay > 0)
                {
                    DOVirtual.DelayedCall(delay, () => SpawnNote(note))
                        .SetId(note.GetHashCode());  
                }
                else
                {
                    SpawnNote(note);
                }
            }
        }

        void SpawnNote(Note note)
        {
            Debug.Log($"[{nameof(SpawnNote)}] Spawning Note {note.GetHashCode()} at time: {Time.time}");
            var go = Object.Instantiate(note.gameObject as GameObject, _spawnTransform);
            go.name = $"{note.GetHashCode()}";
            
            float fullLength = _length + _lengthPadding;
            float velocity = _length / (note.pressMoment - note.spawnMoment + _visualOffset);
            go.transform.DOLocalMoveZ(-fullLength, fullLength / velocity).SetEase(Ease.Linear);
        

            float progress = 0f;
            DOTween.To(() => progress, x => progress = x, 1f,   note.missingCeiling - Time.time)
                .SetId(note.GetHashCode()).OnComplete(() =>
                    {
                        HandleTimeout(note);
                    }
                );
        }

        void HandleRelease(InputAction.CallbackContext context)
        {
        }

        void HandleTimeout(Note note)
        {
            Debug.Log($"[{nameof(HandleTimeout)}] {nameof(missingCeiling)} of {note.GetHashCode()} is {missingCeiling} Timeout ... At {Time.time}");
            NextNote();
        }

        void NextNote(int noteIdx = -1)
        {
            
            if (_currentNoteIdx - 1 >= 0)
            {
                Debug.Log($"[{nameof(NextNote)}] Trying to kill the timeout of {_notes[_currentNoteIdx].GetHashCode()}");
                DOTween.Kill(_notes[_currentNoteIdx].GetHashCode());
            }
            
            if (noteIdx != -1)
            {
                _currentNoteIdx = noteIdx;
            }
            else
            {
                if (_currentNoteIdx + 1 >= _notes.Length)
                {
                    Debug.LogWarning($"[{nameof(NextNote)}] There is no next note at this time");
                    return;
                }

                _currentNoteIdx += 1;
            }
            
            Debug.Log($"[{nameof(NextNote)}] Cur Note is set to {_notes[_currentNoteIdx]}");
            
            // if (_currentNoteIdx - 1 >= 0)
            // {
            //     Debug.LogWarning($"[{nameof(NextNote)}] Trying to kill the timeout of {_notes[_currentNoteIdx - 1].GetHashCode()}");
            //     DOTween.Kill(_notes[_currentNoteIdx - 1].GetHashCode());
            // }
            //
            // // var go =  Object.Instantiate(curNote.gameObject as GameObject, _spawnTransform);
            // // go.name = $"{curNote.GetHashCode()}";
            // // go.transform.DOLocalMoveZ(-fullLength, fullLength/velocity).SetEase(Ease.Linear);
            //
            // //go.transform.position
            //
            // float progress = 0f;
            // DOTween.To(() => progress, x => progress = x, 1f,   missingCeiling - Time.time)
            //     .SetId(curNote.GetHashCode()).OnComplete(() =>
            //         {
            //             HandleTimeout();
            //         }
            //     );
        }
    }

    public class Note
    {
        public GameObject gameObject { get;}

        private float score;
        private float scoreScale;

        public float spawnMoment { get;}
        public float pressMoment { get;}

        public float pressMomentPadding { get; }

        // excellent time period for press: [pressMoment - excellentTolerance, pressMoment + excellentTolerance]
        public float excellentTolerance { get; }

        // good time period for press: [pressMoment - goodTolerance, pressMoment + goodTolerance]
        public float goodTolerance { get; }

        // fair time period for press: [pressMoment - fairTolerance, pressMoment + fairTolerance]
        public float fairTolerance { get; }
        public float missingTolerance { get;}
        

        private UnityAction<float> onPress;
        UnityAction<float> onRelease;
        
        public Note(GameObject obj, float spawnMoment, float pressMoment, float pressMomentPadding, 
            float excellentTolerance, float goodTolerance, float fairTolerance, 
            float missingTolerance)
        {
            this.gameObject = obj;
            this.spawnMoment = spawnMoment;
            this.pressMoment = pressMoment;
            this.pressMomentPadding = pressMomentPadding;
            this.excellentTolerance = excellentTolerance;
            this.goodTolerance = goodTolerance;
            this.fairTolerance = fairTolerance;
            this.missingTolerance = missingTolerance;
        }
        
        public float pressFloor { get { return pressMoment - pressMomentPadding; } }
        public float pressCeil{ get { return  pressMoment +  pressMomentPadding; } }
        public float excellentFloor { get { return pressFloor -  excellentTolerance; }}
        public float excellentCeil { get { return pressCeil +  excellentTolerance; }}
        public float goodFloor { get { return excellentFloor -  goodTolerance; }}
        public float goodCeil{get { return excellentCeil +  excellentTolerance; }}
        public float fairFloor { get { return goodFloor -  fairTolerance;}}
        public float fairCeil { get { return goodCeil +  fairTolerance;}}
        public float missingFloor { get { return fairFloor -  missingTolerance; }}
        public float missingCeiling { get { return fairCeil +  missingTolerance; }}
        
        public override string ToString()
        {
            return $"[{nameof(Note)}] {this.GetHashCode()} pressMoment:{pressMoment }";
        }
    }
    

    // public class TapNote : Note
    // {
    //     
    // }
    //
    // public class HoldNote : Note
    // {
    //     private float releaseMoment;
    //     
    // }
}
