using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Group12
{
    public class InputScript : MonoBehaviour
    {
        private int laneSize = 4;

        //MainInputactto be fine tuned
        public float keyReleaseInputCooldown;

        //to be fine tuned
        public float holdNoteInputCheckCooldown;

        public Transform[] laneEndPos;

        public GameObject perfectSparkPrefab;
        public GameObject excellentSparkPrefab;
        public GameObject greatSparkPrefab;
        public GameObject okSparkPrefab;

        MainInput mainInput;
        private InputAction[] inputChannels;

        private float[] holdNoteTimer, cooldownTimer, noNoteCooldownTimer;
        private bool[] channelIsHold;


        void Awake()
        {
            mainInput = new MainInput();
            mainInput.Enable();

            inputChannels = new InputAction[8]
            {
                mainInput.inLevel.inputChannel0,
                mainInput.inLevel.inputChannel1,
                mainInput.inLevel.inputChannel2,
                mainInput.inLevel.inputChannel3,
                mainInput.inLevel.inputChannel4,
                mainInput.inLevel.inputChannel5,
                mainInput.inLevel.inputChannel6,
                mainInput.inLevel.inputChannel7
            };

            channelIsHold = new bool[inputChannels.Length];

            bindInputAction(inputChannels);
        }

        void Start()
        {
            holdNoteTimer = new float[laneSize];
            cooldownTimer = new float[laneSize];
            noNoteCooldownTimer = new float[laneSize];
        }

        void Update()
        {
            //TODO: replace with new input system
            if (inputChannels != null)
            {
                for (int i = 0; i < inputChannels.Length; i++)
                {
                    int laneIdx = i % laneSize;
                    if (channelIsHold[i] && !inputChannels[i].WasPressedThisFrame() &&
                        !inputChannels[laneIdx].WasPressedThisFrame())
                    {
                        //Debug.Log("inputChannels[" + laneIdx + "] " + inputChannels[i].name + " is hold");
                        pressed(laneIdx, true);
                    }
                }
            }

            /*
            if (Input.GetKeyDown(KeyCode.Z)) {
                pressed(0, false);
            } else if (Input.GetKey(KeyCode.Z)) {
                pressed(0, true);
            } else if (Input.GetKeyUp(KeyCode.Z)) {
                cooldownTimer[0] = keyReleaseInputCooldown;
                //deactivate laser
            }

            if (Input.GetKeyDown(KeyCode.X)) {
                pressed(1, false);
            } else if (Input.GetKey(KeyCode.X)) {
                pressed(1, true);
            } else if (Input.GetKeyUp(KeyCode.X)) {
                cooldownTimer[1] = keyReleaseInputCooldown;
                //deactivate laser
            }

            if (Input.GetKeyDown(KeyCode.N)) {
                pressed(2, false);
            } else if (Input.GetKey(KeyCode.N)) {
                pressed(2, true);
            } else if (Input.GetKeyUp(KeyCode.N)) {
                cooldownTimer[2] = keyReleaseInputCooldown;
                //deactivate laser
            }

            if (Input.GetKeyDown(KeyCode.M)) {
                pressed(3, false);
            } else if (Input.GetKey(KeyCode.M)) {
                pressed(3, true);
            } else if (Input.GetKeyUp(KeyCode.M)) {
                cooldownTimer[3] = keyReleaseInputCooldown;
                //deactivate laser
            }
            */
            //don't touch
            TimerFunc(holdNoteTimer);
            TimerFunc(cooldownTimer);
            TimerFunc(noNoteCooldownTimer);
        }

        void bindInputAction(InputAction[] actions)
        {
            foreach (var action in actions)
            {
                action.started += HandleInputStart;
                action.canceled += HandleInputCancel;
            }
        }

        private void HandleInputStart(InputAction.CallbackContext context)
        {
            Debug.Log($"InputAction {context.action.name} started at time {context.time}");

            int channelIdx = GetChannelIdx(context.action.name);

            pressed(GetLaneIdxByChannelIdx(channelIdx), false);

            channelIsHold[channelIdx] = true;
        }

        // private void performed(InputAction.CallbackContext context)
        // {
        //     pressed(GetLaneByKeycord(context.action.name), true);
        // }

        private void HandleInputCancel(InputAction.CallbackContext context)
        {
            int channelIdx = GetChannelIdx(context.action.name);
            int laneIdx = GetLaneIdxByChannelIdx(channelIdx);
            cooldownTimer[laneIdx] = keyReleaseInputCooldown;
            channelIsHold[channelIdx] = false;
        }

        int GetChannelIdx(string inputChannelName)
        {
            int channelIdx = Int32.Parse(inputChannelName.Substring(12));
            //Debug.Log($"{inputChannelName} is mapped to Lane {lane}");
            return channelIdx;
        }

        int GetLaneIdxByChannelIdx(int channelIdx)
        {
            return channelIdx % laneSize;
        }

        private void pressed(int lane, bool hold)
        {
            if (cooldownTimer[lane] > 0.001f) return;
            if (!hold)
            {
                //activate laser
            }

            Collider[] hitColliders = Physics.OverlapSphere(laneEndPos[lane].position, 0.8f);
            if (hitColliders.Length <= 0 && !(hold && holdNoteTimer[lane] > 0.001f))
            {
                if (noNoteCooldownTimer[lane] < 0.01f)
                {
                    noNoteCooldownTimer[lane] = 0.2f;
                    laneEndPos[lane].gameObject.GetComponent<ParticleSystem>().Emit(16);
                    GameManager.Instance.noNote();
                }

                return;
            }

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.TryGetComponent<INote>(out INote note))
                {
                    float timing = note.hit(hold);
                    if (timing > 99)
                    {
                        GameManager.Instance.perfectNote();
                        holdNoteTimer[lane] = holdNoteInputCheckCooldown;
                        return;
                    }
                    else if (timing > 9)
                    {
                        holdNoteTimer[lane] = holdNoteInputCheckCooldown;
                        return;
                    }

                    timing = Mathf.Abs(timing);
                    // Timing Windows, adjust as necessary;
                    if (timing > 0.09)
                    {
                        spawnSpark(okSparkPrefab, hitCollider.transform.position);
                        GameManager.Instance.okNote();
                    }
                    else if (timing > 0.036f)
                    {
                        spawnSpark(greatSparkPrefab, hitCollider.transform.position);
                        GameManager.Instance.greatNote();
                    }
                    else if (timing > 0.012f)
                    {
                        spawnSpark(excellentSparkPrefab, hitCollider.transform.position);
                        GameManager.Instance.excellentNote();
                    }
                    else
                    {
                        spawnSpark(perfectSparkPrefab, hitCollider.transform.position);
                        GameManager.Instance.perfectNote();
                    }

                    holdNoteTimer[lane] = holdNoteInputCheckCooldown / 2;
                }
            }
        }

        void OnTriggerEnter(Collider c)
        {
            GameManager.Instance.missedNote();
            //Debug.Log("here");
            Destroy(c.gameObject, 2);
        }

        void spawnSpark(GameObject sparkPrefab, Vector3 pos)
        {
            GameObject spark = Instantiate(sparkPrefab, pos, Quaternion.identity);
            Destroy(spark, 2);
        }

        private void TimerFunc(float[] val)
        {
            for (int i = 0; i < val.Length; i++)
            {
                if (val[i] > 0)
                {
                    val[i] -= Time.deltaTime;
                }
            }
        }
    }
}