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
        int _currentNoteIdx;
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
        float missingCeiling { get { return fairCeil - curNote.missingTolerance; }}
#endregion
        
        private Note curNote { get { return _notes[_currentNoteIdx]; } }
        
        public Lane(InputAction actionChannel, Note[] notes, int noteStartIdex = 0)
        {
            _actionChannel = actionChannel;
            _notes = notes;
            _currentNoteIdx = noteStartIdex;
            
            _actionChannel.started += HandlePress;
            _actionChannel.canceled += HandleRelease;
        }

        void HandlePress(InputAction.CallbackContext context)
        {
            //Debug.Log($"[{nameof(HandlePress)}]  {context} Pressed)]");
            
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
            }
            else if(context.startTime < missingFloor)
            {
                Debug.Log($"[{nameof(HandlePress)}] Do Nothing ...");
            }
        } 

        void HandleRelease(InputAction.CallbackContext context)
        {
        }

        void HandleTimeout()
        {
            
        }

        
        //If pressed too early, nothing happened
        void fn()
        {
            float progress = 0f;
            DOTween.To(() => progress, x => progress = x, 1f,   (_notes[_currentNoteIdx].pressMoment + _notes[_currentNoteIdx].missingTolerance) - _notes[_currentNoteIdx].spawnMoment )
                .SetId($"{_notes[_currentNoteIdx].GetHashCode()}").OnComplete(() =>
                    {
                        HandleTimeout();
                        NextNote();
                    }
                );
            
        }

        void NextNote()
        {
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
