using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Group12
{
    public class Health : MonoBehaviour
    {
        public Slider slider;
        public int maxHealth = 100;
        private int curHealth;
        
        // Start is called before the first frame update
        void Start()
        {
            curHealth = maxHealth;
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
        }

        public void ChangeHealth(int change)
        {
            curHealth += change;
            if (curHealth > maxHealth)
            {
                curHealth = maxHealth;
            }
            else if (curHealth <= 0)
            {
                //lose the game
                curHealth = 0;
                // call the panel to show the lose panel
                Panel.Instance.Show(PanelStatus.LOSE);
                // stop the game
                Time.timeScale = 0;
                Debug.Log("Game Over");
            }

            slider.value = curHealth;
        }
        /*
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ChangeHealth(-10);
                Debug.Log("Health: " + curHealth);
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                ChangeHealth(10);
                Debug.Log("Health: " + curHealth);
            }
        }*/
    }
}
