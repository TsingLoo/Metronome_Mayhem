using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Group12
{
    
    public class GameManager : MonoBehaviour
    {
        public Health MyHealth; 
        private int totalNotes;
        [SerializeField] private int missedNum, perfectNum, excellentNum, greatNum, okNum;
        private int combo;
        //add variables that keep track of the current score, combo, number of Good/Bad, etc\

        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
            combo = 0;
        }

        public void noNote()
        {
            // remove combo, decrease health
            MyHealth.ChangeHealth(-5);
            //combo = 0;
        }

        public void missedNote()
        {
            // add a Miss, remove combo, decrease health
            missedNum++;
            MyHealth.ChangeHealth(-5);
            //combo = 0;

        }

        public void okNote()
        {
            // add an Ok, remove combo, decrease health
            okNum++;
            MyHealth.ChangeHealth(-3);
            combo++;
            Debug.Log("Combo: " + combo);
        }

        public void greatNote()
        {
            // add a Great, remove combo, decrease health
            greatNum++;
            MyHealth.ChangeHealth(-1);
            combo++;
            Debug.Log("Combo: " + combo);
        }

        public void excellentNote()
        {
            // add an Excellent, decrease health, increase combo
            excellentNum++;
            MyHealth.ChangeHealth(1);
            combo++;
            Debug.Log("Combo: " + combo);
        }

        public void perfectNote()
        {
            // add a Perfect, increase combo\
            perfectNum++;
            MyHealth.ChangeHealth(5);
            combo++;
            Debug.Log("Combo: " + combo);
        }
    }
}