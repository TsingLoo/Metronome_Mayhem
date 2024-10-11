using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

namespace Group12
{
    // public class Game
    // {
    //     private float time;
    // }

    public class Lane
    {
        private float initTime;

        Transform _spawnTransform;
        int _currentNoteIdx = -1;

        float _timingOffset;

        float _laneTime
        {
            get { return Time.time - initTime + _timingOffset; }
        }

        private float _length;
        private float _lengthPadding = 100f;
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

        float fullLength
        {
            get { return _length + _lengthPadding; }
        }

        float pressMoment
        {
            get { return curNote.pressMoment; }
        }

        float spawnMoment
        {
            get { return curNote.spawnMoment; }
        }

        float pressFloor
        {
            get { return curNote.pressMoment - curNote.pressMomentPadding; }
        }

        float pressCeil
        {
            get { return curNote.pressMoment + curNote.pressMomentPadding; }
        }

        float excellentFloor
        {
            get { return pressFloor - curNote.excellentTolerance; }
        }

        float excellentCeil
        {
            get { return pressCeil + curNote.excellentTolerance; }
        }

        float goodFloor
        {
            get { return excellentFloor - curNote.goodTolerance; }
        }

        float goodCeil
        {
            get { return excellentCeil + curNote.excellentTolerance; }
        }

        float fairFloor
        {
            get { return goodFloor - curNote.fairTolerance; }
        }

        float fairCeil
        {
            get { return goodCeil + curNote.fairTolerance; }
        }

        float missingFloor
        {
            get { return fairFloor - curNote.missingTolerance; }
        }

        float missingCeiling
        {
            get { return fairCeil + curNote.missingTolerance; }
        }

        #endregion

        private Note curNote
        {
            get { return _notes[_currentNoteIdx]; }
        }

        public Lane(InputAction actionChannel, Note[] notes, Transform spawnTransform, float length = 25,
            float visualOffset = 0f, int noteStartIdx = 0)
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

            ScheduleNoteSpawning(0f);

            // foreach (Note note in _notes)
            // {
            //     Debug.Log($"[Lane] {actionChannel.name} {note}");
            // }
        }

        void HandlePress(InputAction.CallbackContext context)
        {
            //GameObject.Instantiate(new GameObject("ASD"));

            //float adjustedStartTime = (float)(context.startTime - _actionChannel.action.time);
            float actionInGameTime = _laneTime;

            Debug.Log(
                $"[{nameof(HandlePress)}]  {context.action.name} Pressed {curNote.GetHashCode()} at {actionInGameTime} : {curNote.pressMoment} " +
                $"missingFloor: {missingFloor}, missingCeiling: {missingCeiling}, " +
                $"excellentFloor: {excellentFloor}, excellentCeiling: {excellentCeil} " +
                $"goodFloor: {goodFloor}, goodCeiling: {goodCeil}" +
                $"fairFloor: {fairFloor}, fairCeiling: {fairCeil}");

            if (actionInGameTime < missingFloor)
            {
                Debug.Log(
                    $"[{nameof(HandlePress)}][KeyPressed: {context.action.name}] Do Nothing ... on {curNote.GetHashCode()}");
                return;
            }


            Debug.Log($"[{nameof(NextNote)}] Trying to kill the timeout of {_notes[_currentNoteIdx].GetHashCode()}");
            DOTween.Kill(_notes[_currentNoteIdx].GetHashCode());


            if (excellentFloor < actionInGameTime && actionInGameTime < excellentCeil)
            {
                Debug.Log(
                    $"[{nameof(HandlePress)}][KeyPressed: {context.action.name}] Excellent Press  on {curNote.GetHashCode()}!!!!");
            }
            else if (goodFloor < actionInGameTime && actionInGameTime < goodCeil)
            {
                Debug.Log(
                    $"[{nameof(HandlePress)}][KeyPressed: {context.action.name}] Good Press on {curNote.GetHashCode()}!!!!");
            }
            else if (fairFloor < actionInGameTime && actionInGameTime < fairCeil)
            {
                Debug.Log(
                    $"[{nameof(HandlePress)}][KeyPressed: {context.action.name}] Fair Press on {curNote.GetHashCode()}!!!!");
            }
            else if (actionInGameTime < missingCeiling)
            {
                Debug.Log(
                    $"[{nameof(HandlePress)}][KeyPressed: {context.action.name}]  Miss Press on {curNote.GetHashCode()}!!!!");
            }

            Debug.Log(
                $"[{nameof(HandlePress)}][KeyPressed: {{context.action.name}}] Press {curNote.GetHashCode()}, Try to go to Next Note ...!!!!");

            if (!curNote.IsHoldNote)
            {
                NextNote();
            }
            else
            {
                HandleHoldStart();
            }
        }

        void ScheduleNoteSpawning(float readyDelay)
        {
            DOVirtual.DelayedCall(readyDelay, () =>
            {
                initTime = Time.time;

                Debug.Log($"[{nameof(ScheduleNoteSpawning)}]{_actionChannel.name} initTime is {initTime}");

                foreach (var note in _notes)
                {
                    float delay = note.spawnMoment;
                    //float delay = note.spawnMoment - Time.time;
                    Debug.Log(
                        $"[{nameof(ScheduleNoteSpawning)}]{_actionChannel.name}{note.GetHashCode()} RealTime .s tims is BEFORE IS   {Time.realtimeSinceStartup}");
                    Debug.Log(
                        $"[{nameof(ScheduleNoteSpawning)}]{_actionChannel.name}{note.GetHashCode()} Time.time BEFORE IS   {Time.time}");
                    DOVirtual.DelayedCall(delay, () =>
                    {
                        _timingOffset = delay - (Time.time - initTime);

                        SpawnNote(note);
                        Debug.Log(
                            $"[{nameof(ScheduleNoteSpawning)}]{_actionChannel.name} spawns {note.GetHashCode()} delay is {delay}");
                        Debug.Log(
                            $"[{nameof(ScheduleNoteSpawning)}]{_actionChannel.name} spawns {note.GetHashCode()} Time.time is  {Time.time}, LaneTime: {_laneTime}");
                    }, false);
                    //.OnComplete( ()=>Debug.Log($"[{nameof(ScheduleNoteSpawning)}]{_actionChannel.name}{note.GetHashCode()} Time On Compelete {Time.realtimeSinceStartup}"));
                }
            }, false);
        }

        void SpawnNote(Note note)
        {
            Debug.Log(
                $"[{nameof(SpawnNote)}] Spawning Note {note.GetHashCode()} at time: {Time.time}, it is supposed to {note.spawnMoment}, the offset is {_timingOffset}");
            var go = Object.Instantiate(note.gameObject as GameObject, _spawnTransform);
            go.name = $"{note.GetHashCode()}";
            float length = (note.releaseMoment - note.pressMoment + 2 * note.missingTolerance) * note.speed;
            go.transform.localScale = new Vector3(1.5f, 0.4f, length);
            go.transform.position = _spawnTransform.position + new Vector3(0, 0, length / 2);

            float fullLength = _length + _lengthPadding;
            float velocity = _length / (note.pressMoment - note.spawnMoment + _visualOffset);
            
            go.transform.DOLocalMoveZ(-fullLength, fullLength / velocity).SetEase(Ease.Linear);

            //Debug.Log($"[]");

            float progress = 0f;


            DOVirtual.DelayedCall(note.missingCeiling + initTime - Time.time, () => { HandleTimeout(note); }, false)
                .SetId(note.GetHashCode());

            // DOTween.To(() => progress, x => progress = x, 1f,   note.missingCeiling + initTime - Time.time)
            //     .SetId(note.GetHashCode()).OnComplete(() =>
            //         {
            //             HandleTimeout(note);
            //         }
            //     );
        }

        void HandleRelease(InputAction.CallbackContext context)
        {
            if (!curNote.IsHoldNote) return;
            float time = _laneTime;
            float releaseTime = curNote.releaseMoment;
            
            Debug.Log(
                $"[{nameof(HandleRelease)}]  {context.action.name} Pressed {curNote.GetHashCode()} at {time} : {curNote.pressMoment}, {curNote.releaseMoment} " +
                $"missingFloor: {missingFloor}, missingCeiling: {missingCeiling}, " +
                $"excellentFloor: {excellentFloor}, excellentCeiling: {excellentCeil} " +
                $"goodFloor: {goodFloor}, goodCeiling: {goodCeil}" +
                $"fairFloor: {fairFloor}, fairCeiling: {fairCeil}");

            // avoid multi next within one beat
            if (time < missingFloor)
            {
                Debug.Log(
                    $"[{nameof(HandleRelease)}][KeyReleased: {context.action.name}] Do Nothing ... on {curNote.GetHashCode()}");
                return;
            }

            Debug.Log($"[{nameof(NextNote)}] Trying to kill the timeout of {_notes[_currentNoteIdx].GetHashCode()}");
            DOTween.Kill(_notes[_currentNoteIdx].GetHashCode());
            
            if (Math.Abs(time - releaseTime) < curNote.excellentTolerance)
            {
                Debug.Log(
                    $"[{nameof(HandleRelease)}][KeyReleased: {context.action.name}] Excellent Press  on {curNote.GetHashCode()}!!!!");
            }
            else if (Math.Abs(time - releaseTime) < curNote.goodTolerance)
            {
                Debug.Log(
                    $"[{nameof(HandleRelease)}][KeyReleased: {context.action.name}] Good Press on {curNote.GetHashCode()}!!!!");
            }
            else if (Math.Abs(time - releaseTime) < curNote.fairTolerance)
            {
                Debug.Log(
                    $"[{nameof(HandleRelease)}][KeyReleased: {context.action.name}] Fair Press on {curNote.GetHashCode()}!!!!");
            }
            else if (Math.Abs(time - releaseTime) < curNote.missingTolerance)
            {
                Debug.Log(
                    $"[{nameof(HandleRelease)}][KeyReleased: {context.action.name}]  Miss Press on {curNote.GetHashCode()}!!!!");
            }

            Debug.Log(
                $"[{nameof(HandleRelease)}][KeyReleased: {{context.action.name}}] Press {curNote.GetHashCode()}, Try to go to Next Note ...!!!!");
        
            HandleHoldEnd();
            NextNote();
        }

        void HandleTimeout(Note note)
        {
            Debug.Log(
                $"[{nameof(HandleTimeout)}] {nameof(missingCeiling)} of {note.GetHashCode()} is {missingCeiling} Timeout ... At {_laneTime}");
            NextNote();
        }

        void HandleHoldStart()
        {
            
        }

        void HandleHoldEnd()
        {
            //if(Math.Abs(curNote.releaseMoment - Time.time) <  ;
        }

        void NextNote(int noteIdx = -1)
        {
            // if (_currentNoteIdx - 1 >= 0)
            // {
            //     Debug.Log($"[{nameof(NextNote)}] Trying to kill the timeout of {_notes[_currentNoteIdx].GetHashCode()}");
            //     DOTween.Kill(_notes[_currentNoteIdx].GetHashCode());
            // }

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

            Debug.Log(
                $"[{nameof(NextNote)}] {_actionChannel.name} Cur Note is set to {_currentNoteIdx} {_notes[_currentNoteIdx]}");

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
        public GameObject gameObject { get; }

        private float score;
        private float scoreScale;

        public float speed { get; }
        public float spawnMoment { get; }
        public float pressMoment { get; }
        //public float releaseMoment { get;}

        public float releaseMoment = -1f;

        public bool IsHoldNote
        {
            get { return releaseMoment != pressMoment; }
        }

        public float pressMomentPadding { get; }

        // excellent time period for press: [pressMoment - excellentTolerance, pressMoment + excellentTolerance]
        public float excellentTolerance { get; }

        // good time period for press: [pressMoment - goodTolerance, pressMoment + goodTolerance]
        public float goodTolerance { get; }

        // fair time period for press: [pressMoment - fairTolerance, pressMoment + fairTolerance]
        public float fairTolerance { get; }
        public float missingTolerance { get; }


        private UnityAction<float> onPress;
        UnityAction<float> onRelease;

        public Note(GameObject obj, float speed, float pressMoment, float releaseMoment, float pressMomentPadding,
            float excellentTolerance, float goodTolerance, float fairTolerance,
            float missingTolerance, float laneLength = 25f)
        {
            this.gameObject = obj;
            this.speed = speed;
            this.pressMoment = pressMoment;
            this.releaseMoment = releaseMoment;
            this.pressMomentPadding = pressMomentPadding;
            this.excellentTolerance = excellentTolerance;
            this.goodTolerance = goodTolerance;
            this.fairTolerance = fairTolerance;
            this.missingTolerance = missingTolerance;

            this.spawnMoment = this.pressMoment - laneLength / speed;
        }

        public float pressFloor
        {
            get { return pressMoment - pressMomentPadding; }
        }

        public float pressCeil
        {
            get { return pressMoment + pressMomentPadding; }
        }

        public float excellentFloor
        {
            get { return pressFloor - excellentTolerance; }
        }

        public float excellentCeil
        {
            get { return pressCeil + excellentTolerance; }
        }

        public float goodFloor
        {
            get { return excellentFloor - goodTolerance; }
        }

        public float goodCeil
        {
            get { return excellentCeil + excellentTolerance; }
        }

        public float fairFloor
        {
            get { return goodFloor - fairTolerance; }
        }

        public float fairCeil
        {
            get { return goodCeil + fairTolerance; }
        }

        public float missingFloor
        {
            get { return fairFloor - missingTolerance; }
        }

        public float missingCeiling
        {
            get { return fairCeil + missingTolerance; }
        }

        public override string ToString()
        {
            return $"[{nameof(Note)}] {this.GetHashCode()} pressMoment:{pressMoment}";
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