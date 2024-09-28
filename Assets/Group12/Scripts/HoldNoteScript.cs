using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Group12
{
    public class HoldNoteScript : MonoBehaviour, INote
    {
        public GameObject perfectSparkPrefab;
        private bool beenHit;
        private float speed, lifetime;

        public void init(float speed)
        {
            this.speed = speed;
        }

        void Update()
        {
            lifetime -= Time.deltaTime;
            if (beenHit && lifetime < 0)
            {
                if (perfectSparkPrefab != null)
                {
                    GameObject spark = Instantiate(perfectSparkPrefab, transform.position, Quaternion.identity);
                    Destroy(spark, 2);
                }

                Destroy(gameObject);
            }
        }

        void FixedUpdate()
        {
            transform.position -= speed * Time.fixedDeltaTime * transform.forward;
        }

        public float hit(bool holding)
        {
            kill();
            return beenHit ? 10 : 100;
        }

        public bool hasBeenHit()
        {
            return beenHit;
        }

        private void kill()
        {
            beenHit = true;
        }
    }
}