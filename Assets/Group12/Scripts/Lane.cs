using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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

    public enum timingGrade
    {
        Perfect,
        Excellent,
        Great,
        Ok,
        Missed,
        Undefined
    }

    public class Lane
    {
        private float initTime;

        Transform _spawnTransform;
        int _currentNoteIdx = -1;

        float _timingOffset;


        public float _laneTime
        {
            get { return Time.time - initTime + _timingOffset; }
        }

        private float _laneLength;
        private float _lengthPadding = 100f;
        private float _visualOffset;

        //Game game;

        private Note[] _notes;
        private float _score;
        private float _currentProgress;
        private InputAction _actionChannel;


        #region Timing Definitions

        float fullLength
        {
            get { return _laneLength + _lengthPadding; }
        }

        float pressMoment
        {
            get { return curNote.pressMoment; }
        }

        float spawnMoment
        {
            get { return curNote.spawnMoment; }
        }

        #endregion

        timingGrade GetTimingGrade(float inputTime, float refernceTime)
        {
            if (Math.Abs(inputTime - refernceTime) < curNote.perfectTolerance)
            {
                Debug.Log(
                    $"[{nameof(HandleRelease)}][KeyReleased: Perfect Action  on {curNote.GetHashCode()}!!!!");
                GameManager.Instance.DoPerfectNote();
                return timingGrade.Excellent;
            }
            else if (Math.Abs(inputTime - refernceTime) < curNote.excellentTolerance)
            {
                Debug.Log(
                    $"[{nameof(HandleRelease)}][KeyReleased: Excellent Action  on {curNote.GetHashCode()}!!!!");
                GameManager.Instance.DoExcellentNote();
                return timingGrade.Excellent;
            }
            else if (Math.Abs(inputTime - refernceTime) < curNote.greatTolerance)
            {
                Debug.Log(
                    $"[{nameof(HandleRelease)}][KeyReleased: Great Action on {curNote.GetHashCode()}!!!!");
                GameManager.Instance.DoGreatNote();
                return timingGrade.Great;
            }
            else if (Math.Abs(inputTime - refernceTime) < curNote.okTolerance)
            {
                Debug.Log(
                    $"[{nameof(HandleRelease)}][KeyReleased: Ok Action on {curNote.GetHashCode()}!!!!");
                GameManager.Instance.DoOkNote();
                return timingGrade.Ok;
            }
            else if (Math.Abs(inputTime - refernceTime) < curNote.missedTolerance)
            {
                Debug.Log(
                    $"[{nameof(HandleRelease)}][KeyReleased: Miss Action on {curNote.GetHashCode()}!!!!");
                GameManager.Instance.DoMissedNote();
                return timingGrade.Missed;
            }

            return timingGrade.Undefined;
        }

        private Note curNote
        {
            get { return _notes[_currentNoteIdx]; }
        }

        public Lane(InputAction actionChannel, Note[] notes, Transform spawnTransform, float laneLength,
            float visualOffset = 0f, int noteStartIdx = 0)
        {
            _actionChannel = actionChannel;
            _notes = notes;

            _spawnTransform = spawnTransform;
            _laneLength = laneLength;
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
                $"[{nameof(HandlePress)}]  {context.action.name} Pressed {curNote.GetHashCode()} at {actionInGameTime} : {curNote.pressMoment} ");

            if (actionInGameTime < curNote.pressMoment - curNote.missedTolerance)
            {
                Debug.Log(
                    $"[{nameof(HandlePress)}][KeyPressed: {context.action.name}] Do Nothing ... on {curNote.GetHashCode()}");
                return;
            }

            Debug.Log($"[{nameof(NextNote)}] Trying to kill the timeout of {_notes[_currentNoteIdx].GetHashCode()}");
            DOTween.Kill(_notes[_currentNoteIdx].GetHashCode());

            GetTimingGrade(actionInGameTime, curNote.pressMoment);

            // if (excellentFloor < actionInGameTime && actionInGameTime < excellentCeil)
            // {
            //     Debug.Log(
            //         $"[{nameof(HandlePress)}][KeyPressed: {context.action.name}] Excellent Press  on {curNote.GetHashCode()}!!!!");
            // }
            // else if (goodFloor < actionInGameTime && actionInGameTime < goodCeil)
            // {
            //     Debug.Log(
            //         $"[{nameof(HandlePress)}][KeyPressed: {context.action.name}] Good Press on {curNote.GetHashCode()}!!!!");
            // }
            // else if (fairFloor < actionInGameTime && actionInGameTime < fairCeil)
            // {
            //     Debug.Log(
            //         $"[{nameof(HandlePress)}][KeyPressed: {context.action.name}] Fair Press on {curNote.GetHashCode()}!!!!");
            // }
            // else if (actionInGameTime < missingCeiling)
            // {
            //     Debug.Log(
            //         $"[{nameof(HandlePress)}][KeyPressed: {context.action.name}]  Miss Press on {curNote.GetHashCode()}!!!!");
            // }

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
            float noteLength = Math.Max(note.excellentTolerance, note.releaseMoment - note.pressMoment) * note.speed;
            //float noteLength = (note.releaseMoment - note.pressMoment ) * note.speed;
            go.transform.localScale = new Vector3(1.5f, 0.4f, noteLength);
            go.transform.position = _spawnTransform.position + new Vector3(0, 0, noteLength / 2);

            float fullMoveLength = _laneLength + _lengthPadding;

            Debug.Log(
                $"[Arrive] {note.GetHashCode()} Suppose to arrive at {_laneLength / note.speed + note.spawnMoment}, lanelength is {_laneLength}, note.speed is {note.speed}, note.spwanMoment is {note.spawnMoment}");

            //float velocity = _length / (note.pressMoment - note.spawnMoment + _visualOffset);

            go.transform.DOLocalMoveZ(-fullMoveLength + noteLength / 2, fullMoveLength / note.speed)
                .SetEase(Ease.Linear).OnUpdate(
                    () =>
                    {
                        if (_laneTime == note.pressMoment)
                        {
                            Debug.Log($"[NotePos] {note.GetHashCode()} is at Pos {go.transform.localPosition}");
                        }
                    }
                );

            //Debug.Log($"[]");

            //float progress = 0f;


            DOVirtual.DelayedCall(note.pressMoment + note.missedTolerance + initTime - Time.time,
                    () => { HandleTimeout(note); }, false)
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

            // Debug.Log(
            //     $"[{nameof(HandleRelease)}]  {context.action.name} Pressed {curNote.GetHashCode()} at {time} : {curNote.pressMoment}, {curNote.releaseMoment} " +
            //     $"missingFloor: {missingFloor}, missingCeiling: {missingCeiling}, " +
            //     $"excellentFloor: {excellentFloor}, excellentCeiling: {excellentCeil} " +
            //     $"goodFloor: {goodFloor}, goodCeiling: {goodCeil}" +
            //     $"fairFloor: {fairFloor}, fairCeiling: {fairCeil}");

            // avoid multi next within one beat

            //if (actionInGameTime < curNote.pressMoment - curNote.excellentTolerance)
            if (time < curNote.pressMoment - curNote.excellentTolerance)
            {
                Debug.Log(
                    $"[{nameof(HandleRelease)}][KeyReleased: {context.action.name}] Do Nothing ... on {curNote.GetHashCode()}");
                return;
            }

            Debug.Log($"[{nameof(NextNote)}] Trying to kill the timeout of {_notes[_currentNoteIdx].GetHashCode()}");
            DOTween.Kill(_notes[_currentNoteIdx].GetHashCode());

            GetTimingGrade(time, releaseTime);

            // if (Math.Abs(time - releaseTime) < curNote.excellentTolerance)
            // {
            //     Debug.Log(
            //         $"[{nameof(HandleRelease)}][KeyReleased: {context.action.name}] Excellent Press  on {curNote.GetHashCode()}!!!!");
            // }
            // else if (Math.Abs(time - releaseTime) < curNote.goodTolerance)
            // {
            //     Debug.Log(
            //         $"[{nameof(HandleRelease)}][KeyReleased: {context.action.name}] Good Press on {curNote.GetHashCode()}!!!!");
            // }
            // else if (Math.Abs(time - releaseTime) < curNote.fairTolerance)
            // {
            //     Debug.Log(
            //         $"[{nameof(HandleRelease)}][KeyReleased: {context.action.name}] Fair Press on {curNote.GetHashCode()}!!!!");
            // }
            // else if (Math.Abs(time - releaseTime) < curNote.missingTolerance)
            // {
            //     Debug.Log(
            //         $"[{nameof(HandleRelease)}][KeyReleased: {context.action.name}]  Miss Press on {curNote.GetHashCode()}!!!!");
            // }

            Debug.Log(
                $"[{nameof(HandleRelease)}][KeyReleased: {{context.action.name}}] Press {curNote.GetHashCode()}, Try to go to Next Note ...!!!!");

            HandleHoldEnd();
            NextNote();
        }

        void HandleTimeout(Note note)
        {
            GameManager.Instance.DoMissedNote();
            //Debug.Log($"[{nameof(HandleTimeout)}] {nameof()} of {note.GetHashCode()} is {missingCeiling} Timeout ... At {_laneTime}");
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
}