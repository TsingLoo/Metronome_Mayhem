using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Group12
{
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

        public float perfectTolerance { get; }

        // excellent time period for press: [pressMoment - excellentTolerance, pressMoment + excellentTolerance]
        public float excellentTolerance { get; }

        // good time period for press: [pressMoment - goodTolerance, pressMoment + goodTolerance]
        public float greatTolerance { get; }

        // fair time period for press: [pressMoment - fairTolerance, pressMoment + fairTolerance]
        public float okTolerance { get; }
        public float missedTolerance { get; }


        private UnityAction<float> onPress;
        UnityAction<float> onRelease;

        public Note(GameObject obj, float speed, float pressMoment, float releaseMoment, float pressMomentPadding,
            float perfectTolerance,
            float excellentTolerance, float greatTolerance, float okTolerance,
            float missedTolerance, float laneLength)
        {
            this.gameObject = obj;
            this.speed = speed;
            this.pressMoment = pressMoment;
            this.releaseMoment = releaseMoment;
            this.pressMomentPadding = pressMomentPadding;
            this.perfectTolerance = perfectTolerance;
            this.excellentTolerance = excellentTolerance;
            this.greatTolerance = greatTolerance;
            this.okTolerance = okTolerance;
            this.missedTolerance = missedTolerance;

            this.spawnMoment = this.pressMoment - laneLength / speed;
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