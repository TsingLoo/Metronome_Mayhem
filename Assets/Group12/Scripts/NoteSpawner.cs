using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Group12
{
    public class NoteSpawner : MonoBehaviour
    {
        public float distance, speed;
        public float songEndBeat;
        public int bpm;
        public GameObject note, holdNote;
        public Transform[] lanePos;
        private float secPerBeat, dsptimesong, timeDelay;
        private float songPosition, songPosInBeats;
        private int ind;
        private (double D, double X, char K)[] beatmap;
        private AudioSource AS;
        bool started, ended;

        // Start is called before the first frame update
        void Start()
        {
            timeDelay = distance / speed;
            AS = GetComponent<AudioSource>();
            secPerBeat = 60f / bpm;
            ind = 0;
            beatmap = SongData.excelsus;
            started = false;
            ended = false;

            startSong();
        }

        public void startSong()
        {
            started = true;
            dsptimesong = (float)AudioSettings.dspTime;
            //AS.Play();
        }

        public void endLevel()
        {
            ended = true;
            //AS.Stop();
            // ...
        }

        // Update is called once per frame
        void Update()
        {
            if (!started || ended) return;
            songPosition = (float)(AudioSettings.dspTime - dsptimesong);
            songPosInBeats = songPosition / secPerBeat;

            while (ind < beatmap.Length && (float)beatmap[ind].X < songPosInBeats + timeDelay)
            {
                //spawn((float)beatmap[ind].D * secPerBeat, beatmap[ind].K);
                ind++;
            }

            if (songPosInBeats > songEndBeat) endLevel();
        }

        private void spawn(float d, char k)
        {
            int lane;
            switch (k)
            {
                case 'D':
                    lane = 0;
                    break;
                case 'Z':
                    lane = 0;
                    break;
                case 'F':
                    lane = 1;
                    break;
                case 'X':
                    lane = 1;
                    break;
                case 'J':
                    lane = 2;
                    break;
                case 'N':
                    lane = 2;
                    break;
                case 'K':
                    lane = 3;
                    break;
                case 'M':
                    lane = 3;
                    break;
                default:
                    Debug.Log("spawning error");
                    lane = 0;
                    break;
            }

            if (d < 0.01f)
            {
                GameObject beat = Instantiate(note, lanePos[lane].position + new Vector3(0, 0.2f, 0),
                    Quaternion.Euler(-Vector3.forward));
                //beat.GetComponent<NoteScript>().init(speed, timeDelay);
            }
            else
            {
                StartCoroutine(holdNoteSpawner(d, lane));
            }
        }

        private IEnumerator holdNoteSpawner(float duration, int lane)
        {
            while (duration > -0.01f)
            {
                GameObject holdBeat = Instantiate(holdNote, lanePos[lane].position + new Vector3(0, 0.15f, 0),
                    Quaternion.Euler(-Vector3.forward));
                holdBeat.GetComponent<HoldNoteScript>().init(speed);
                duration -= secPerBeat / 4;
                yield return new WaitForSeconds(secPerBeat / 4);
            }
        }
    }
}
