using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
                Hide();
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            });
        }

        // show panel
        public void Show(PanelStatus status)
        {
            Time.timeScale = 0;
            DOTween.timeScale = 0;
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
            Time.timeScale = 1;
            DOTween.timeScale = 1;
            gameObject.SetActive(false);
        }
    }
}