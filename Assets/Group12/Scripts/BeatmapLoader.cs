using System;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

namespace Group12
{
    public class BeatmapLoader
    {
        
        [System.Serializable]
        public class Beat
        {
            public float beat;
            public float hold;
            public int key;
            public int speed;
        }
        
        public static Beat[][] load(string fileName)
        {
            TextAsset jsonFile = Resources.Load<TextAsset>("Data/beatmaps/" + fileName + "/data");

            if (jsonFile == null)
            {
                Debug.LogError("Can not load beatmap：Data/beatmaps/" + fileName + "/data");
                return Array.Empty<Beat[]>();
            }

            Beat[] beats = JsonConvert.DeserializeObject<Beat[]>(jsonFile.text);
            var keyToBeats =  beats.GroupBy(b => b.key)
                .ToDictionary(g => g.Key, g => g.ToArray());
            
            Beat[][] beatGroups = new Beat[keyToBeats.Count][];
            for (int i = 0; i < keyToBeats.Count; ++i)
            {
                beatGroups[i] = keyToBeats[i];
            }
            return beatGroups;
        }
    }
}