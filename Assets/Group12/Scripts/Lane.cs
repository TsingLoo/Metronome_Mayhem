using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor.ShaderGraph.Internal;
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
        Game game;

        private Note[] _notes;
        private float _score;
        int _currentNoteIdx = -1;
        private float _currentProgress;
        private InputAction _actionChannel;

        #region Timing Definitions
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
        
        public Lane(InputAction actionChannel, Note[] notes, int noteStartIdx = 0)
        {
            _actionChannel = actionChannel;
            _notes = notes;
            //_currentNoteIdx = noteStartIdx - 1;
            NextNote(noteStartIdx);
            
            _actionChannel.started += HandlePress;
            _actionChannel.canceled += HandleRelease;
        }

        void HandlePress(InputAction.CallbackContext context)
        {
            //Debug.Log($"[{nameof(HandlePress)}]  {context} Pressed)]");
            if(context.startTime < missingFloor )
            {
                Debug.Log($"[{nameof(HandlePress)}] Do Nothing ...");
                return;
            } 
            
            if (excellentFloor < context.startTime && context.startTime < excellentCeil)
            {
                Debug.Log($"[{nameof(HandlePress)}] Excellent Press !!!!");
            }
            else if (goodFloor < context.startTime && context.startTime < goodCeil)
            {
                Debug.Log($"[{nameof(HandlePress)}] Good Press !!!!");
            } 
            else if (fairFloor < context.startTime && context.startTime < fairCeil) 
            {
                Debug.Log($"[{nameof(HandlePress)}] Fair Press !!!!");
                
            }else if (context.startTime < missingCeiling)
            {
                Debug.Log($"[{nameof(HandlePress)}] Miss Press!!!!");
            }

            Debug.Log($"[{nameof(HandlePress)}] Press {curNote.GetHashCode()}, Go to Next Note ... !!!!");
            NextNote();
        } 

        void HandleRelease(InputAction.CallbackContext context)
        {
        }

        void HandleTimeout()
        {
            Debug.Log($"[{nameof(HandleTimeout)}] {nameof(missingCeiling)} is {missingCeiling} Timeout ... At {Time.time}");
            NextNote();
        }

        void NextNote(int noteIdx = -1)
        {
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
            
            Debug.Log($"[{nameof(NextNote)}] Cur Note is {_notes[_currentNoteIdx]}");
            if (_currentNoteIdx - 1 >= 0)
            {
                DOTween.Kill(_notes[_currentNoteIdx - 1].GetHashCode());
            }
            
            
            float progress = 0f;
            DOTween.To(() => progress, x => progress = x, 1f,   missingCeiling - Time.time)
                .SetId(curNote.GetHashCode()).OnComplete(() =>
                    {
                        HandleTimeout();
                    }
                );
        }
    }

    public class Note
    {
        GameObject gameObject;

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
        
        public Note(float spawnMoment, float pressMoment, float pressMomentPadding, 
            float excellentTolerance, float goodTolerance, float fairTolerance, 
            float missingTolerance)
        {
            this.spawnMoment = spawnMoment;
            this.pressMoment = pressMoment;
            this.pressMomentPadding = pressMomentPadding;
            this.excellentTolerance = excellentTolerance;
            this.goodTolerance = goodTolerance;
            this.fairTolerance = fairTolerance;
            this.missingTolerance = missingTolerance;
        }

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
