using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

namespace ChadLads.Scoreboards
{

    //to create the scoreboards, i followed a tutorial which can be found here:
    //https://www.youtube.com/watch?v=FSEbPxf0kfs&ab_channel=DapperDino
    //it stores the scores in a .json file which can be found in the unity persistent data path under DownhillJam/highscores.json

    public class Scoreboard : MonoBehaviour
    {
        [SerializeField] 
        private int maxScoreboardEntries = 10;
        [SerializeField]
        private Transform highscoresHolderTransform = null;
        [SerializeField]
        private GameObject scoreboardEntryObject = null;

        [Header("Test")]
        [SerializeField]
        ScoreboardEntryData testEntrydata = new ScoreboardEntryData();

        private string SavePath => $"{Application.persistentDataPath}/highscores.json";

        public HoverboardController player;
        public GameController gameMaster;

        private void Start()
        {
            ScoreboardSaveData savedScores = GetSavedScores();

            SaveScores(savedScores);
            UpdateUI(savedScores);
        }

        [ContextMenu("Add Test Entry")]
        public void AddTestEntry()
        {
            AddEntry(testEntrydata);
        }

        public void AddPlayerEntry()
        {
            ScoreboardEntryData playerData = new ScoreboardEntryData();
            playerData.entryName = player.playerName;
            playerData.entrySpeed = gameMaster.playerTime;
            playerData.entryCringe = player.cringeScore;
            playerData.entryRadical = player.radicalScore;
            AddEntry(playerData);
        }

        private ScoreboardSaveData GetSavedScores()
        {
            if (!File.Exists(SavePath))
            {
                File.Create(SavePath).Dispose();
                return new ScoreboardSaveData();
            }

            using (StreamReader stream = new StreamReader(SavePath))
            {
                string json = stream.ReadToEnd();
                return JsonUtility.FromJson<ScoreboardSaveData>(json);
            }
        }

        public void AddEntry(ScoreboardEntryData scoreboardEntryData)
        {


            ScoreboardSaveData savedScores = GetSavedScores();
            bool scoreAdded = false;
            for (int i = 0; i < savedScores.highscores.Count; i++)
            {
                if(scoreboardEntryData.entryRadical > savedScores.highscores[i].entryRadical)
                {
                    savedScores.highscores.Insert(i, scoreboardEntryData);
                    scoreAdded = true;
                    break;
                }
            }

            if (!scoreAdded && savedScores.highscores.Count < maxScoreboardEntries)
            {
                savedScores.highscores.Add(scoreboardEntryData);
            }

            if (savedScores.highscores.Count > maxScoreboardEntries)
            {
                savedScores.highscores.RemoveRange(maxScoreboardEntries, savedScores.highscores.Count - maxScoreboardEntries);

            }

            UpdateUI(savedScores);
            SaveScores(savedScores);
        }

        private void UpdateUI(ScoreboardSaveData savedScores)
        {
            //FILTER CODE NEEDS IMPROVEMENT HERE.
            //REWORK THIS FUNCTION
            foreach (Transform child in highscoresHolderTransform)
            {
                Destroy(child.gameObject);
            }
            foreach (ScoreboardEntryData highscore in savedScores.highscores)
            {
                Instantiate(scoreboardEntryObject, highscoresHolderTransform).GetComponent<ScoreboardEntryUI>().Initialize(highscore);
            }
        }
        private void SaveScores(ScoreboardSaveData scoreboardSaveData)
        {
            using (StreamWriter stream = new StreamWriter(SavePath))
            {
                string json = JsonUtility.ToJson(scoreboardSaveData, true);
                stream.Write(json);
            }
        }

        public void ReloadScene()
        {
            SceneManager.LoadScene(2);
        }

        public void LoadMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}

