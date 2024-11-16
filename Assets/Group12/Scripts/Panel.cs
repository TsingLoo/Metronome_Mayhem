using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Group12
{
    public enum PanelStatus
    {
        WIN,
        LOSE,
        PAUSE
    }
    
    public class Panel : MonoBehaviour
    {
        [SerializeField] private TMP_Text statusLabel;
        [SerializeField] private Button restartButton;
        
        public static Panel Instance { get; private set; }

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            
            DontDestroyOnLoad(gameObject);
            Instance = this;
            Hide();
            
            restartButton.onClick.AddListener(() =>
            {
                GameManager.Instance.ResetGame();
                // restart the game
                Time.timeScale = 1;
                Hide();
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            });
        }

        // show panel
        public void Show(PanelStatus status)
        {
            gameObject.SetActive(true);
            switch (status)
            {
                case PanelStatus.WIN:
                    statusLabel.text = "You Win!";
                    break;
                case PanelStatus.LOSE:
                    statusLabel.text = "You Lose!";
                    break;
                case PanelStatus.PAUSE:
                    statusLabel.text = "Paused";
                    break;
            }
        }

        // hide panel
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}