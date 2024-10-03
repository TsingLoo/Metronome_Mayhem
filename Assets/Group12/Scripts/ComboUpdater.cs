using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Group12
{
    public class ComboUpdater : MonoBehaviour
    {
        public TextMeshProUGUI scoreText;

        // Update is called once per frame
        void Update()
        {
            scoreText.text = "x " + GameManager.Instance.getCombo().ToString();
        }
    }
}
