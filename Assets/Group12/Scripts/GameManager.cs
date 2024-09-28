using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int totalNotes;
    [SerializeField] private int missedNum, perfectNum, excellentNum, greatNum, okNum;
    private int combo;
    //add variables that keep track of the current score, combo, number of Good/Bad, etc\

    public static GameManager Instance { get; private set; }

    private void Awake() {         
        if (Instance != null && Instance != this) { 
            Destroy(this); 
        } else { 
            Instance = this; 
        } 
    }
    public void noNote() {
        // remove combo, decrease health
    }
    public void missedNote() {
        // add a Miss, remove combo, decrease health
        missedNum++;
    }
    public void okNote() {
        // add an Ok, remove combo, decrease health
        okNum++;
    }
    public void greatNote() {
        // add a Great, remove combo, decrease health
        greatNum++;
    }
    public void excellentNote() {
        // add an Excellent, decrease health, increase combo
        excellentNum++;
    }
    public void perfectNote() {
        // add a Perfect, increase combo\
        perfectNum++;
    }

}
