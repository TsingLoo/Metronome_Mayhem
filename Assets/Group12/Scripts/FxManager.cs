using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace  Group12
{
    public class FxManager : MonoBehaviour
    {
        // public static Color PerfectColor = new Color(76/255f, 201/255f, 240/255f, 1f);
        // public static Color ExcellentColor = new Color(67/255f, 97/255f, 238/255f, 1f);
        // public static Color OkColor = new Color(58/255f, 12/255f, 163/255f, 1f);
        // public static Color GreatColor = new Color(114/255f, 9/255f, 183/255f, 1f);
        // public static Color MissedColor = new Color(247/255f, 37/255f, 133/255f, 1f);
        public static Color NothingColor = new Color(173/255f, 181/255f, 189/255f, 1f);
        
        // public static Color PerfectColor = new Color(217/255f, 237/255f, 146/255f, 1f);
        // public static Color ExcellentColor = new Color(181/255f, 228/255f, 140/255f, 1f);
        // public static Color OkColor = new Color(153/255f, 217/255f, 140/255f, 1f);
        // public static Color GreatColor = new Color(114/255f, 9/255f, 183/255f, 1f);
        // public static Color MissedColor = new Color(247/255f, 37/255f, 133/255f, 1f);
        
        public static Color PerfectColor = new Color(200/255f, 85/255f, 61/255f, 1f);
        public static Color ExcellentColor = new Color(242/255f, 143/255f, 59/255f, 1f);
        public static Color OkColor = new Color(255/255f, 213/255f, 194/255f, 1f);
        public static Color GreatColor = new Color(252/255f, 246/255f, 189/255f, 1f);
        public static Color MissedColor = new Color(88/255f, 139/255f, 139/255f, 1f);
        
        [SerializeField] private GameObject _actionFxParent;
        [SerializeField] private GameObject _actionFxPrefab;
        
        public static FxManager Instance;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            //DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        public void PlayActionFx(int laneIndex, timingGrade grade = timingGrade.Undefined)
        {
            GameObject actionFx = Instantiate(_actionFxPrefab,_actionFxParent.transform.GetChild(laneIndex));
            var fx = actionFx.GetComponent<ActionFx>();
            fx.DoFx(grade);
        }

        public Color GetColorByGrade(timingGrade grade)
        {
            switch (grade)
            {
                case timingGrade.Perfect:
                    return PerfectColor;
                case timingGrade.Excellent:
                    return ExcellentColor;
                case timingGrade.Great:
                    return GreatColor;
                case timingGrade.Ok:
                    return OkColor;
                case timingGrade.Missed:
                    return MissedColor;
                
                case timingGrade.Undefined:
                
                default:
                    return NothingColor;
            }
        }

        public string GetStringByGrade(timingGrade grade)
        {
            if (grade == timingGrade.Undefined)
            {
                return "";
            }

            return grade.ToString();
        }
    }
}