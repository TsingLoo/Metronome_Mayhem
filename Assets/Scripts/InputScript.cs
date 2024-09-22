using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputScript : MonoBehaviour
{
    //to be fine tuned
    public float keyReleaseInputCooldown;
    //to be fine tuned
    public float holdNoteInputCheckCooldown;

    public Transform[] laneEndPos;

    public GameObject perfectSparkPrefab;
    public GameObject greatSparkPrefab;
    public GameObject goodSparkPrefab;
    public GameObject badSparkPrefab;

    private float[] holdNoteTimer, cooldownTimer;

    void Start()
    {
        holdNoteTimer = new float[4];
        cooldownTimer = new float[4];
    }

    void Update()
    {
        //replace with updated input system and preferred keys

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

        TimerFunc(holdNoteTimer);
        TimerFunc(cooldownTimer);
    }

    private void pressed(int lane, bool hold) {
        if (cooldownTimer[lane] > 0.001f || (hold && holdNoteTimer[lane] > 0.001f)) return;
        if (!hold) {
            //activate laser
        }
        
        Collider[] hitColliders = Physics.OverlapSphere(laneEndPos[lane].position, 0.8f);
        if (hitColliders.Length <= 0) {
            GameManager.Instance.noNote();
            return;
        }
        foreach (var hitCollider in hitColliders) {
            if (hitCollider.gameObject.TryGetComponent<INote>(out INote note)) {
                float timing = note.hit(hold);
                if (timing > 99) {
                    GameManager.Instance.perfectNote();
                    holdNoteTimer[lane] = holdNoteInputCheckCooldown;
                    return;
                } else if (timing > 9) {
                    holdNoteTimer[lane] = holdNoteInputCheckCooldown;
                    return;
                }

                timing = Mathf.Abs(timing);
                // Timing Windows, adjust as necessary;
                if (timing > 0.09) {
                    //instantiate spark
                    GameManager.Instance.okNote();
                } else if (timing > 0.036f) {
                    //instantiate spark
                    GameManager.Instance.greatNote();
                } else if (timing > 0.012f) {
                    //instantiate spark
                    GameManager.Instance.excellentNote();
                } else {
                    //instantiate spark
                    GameManager.Instance.perfectNote();
                }
                holdNoteTimer[lane] = holdNoteInputCheckCooldown / 2;
            }
        }
    }

    void OnTriggerEnter(Collider c) {
        GameManager.Instance.missedNote();
        Destroy(c.gameObject, 1);
    }


    private void TimerFunc(float[] val){
        for (int i = 0; i < val.Length; i++) {
            if (val[i] > 0){
                val[i] -= Time.deltaTime;
            }
        }
    }
}
