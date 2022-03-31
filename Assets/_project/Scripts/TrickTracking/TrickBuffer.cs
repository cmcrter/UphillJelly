using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickBuffer : MonoBehaviour
{
    private struct ScorableActionInProgress
    {
        public float startTime;
        public int ID;
        public ScoreableAction scoreableActionData;

        public ScorableActionInProgress(float startTime, int iD, ScoreableAction scoreableActionData)
        {
            this.startTime = startTime;
            ID = iD;
            this.scoreableActionData = scoreableActionData;
        }
    }

    private int currentID = 0;

    /// <summary>
    /// The score-able actions that have been started but not finished
    /// </summary>
    List<ScorableActionInProgress> scoreableActionsInProgress;
    /// <summary>
    /// The score-able actions that have been complete and how long there where performed for
    /// </summary>
    List<KeyValuePair<ScoreableAction, float>> scoreableActionsCompleted;

    /// <summary>
    /// Adds a Scoreable action to the list of ones in progress
    /// </summary>
    /// <param name="scoreableActionData">The data on the scoreable action to add</param>
    /// <returns>An id for the action in progress that was just added, used to access it later externally</returns>
    public int AddScoreableActionInProgress(ScoreableAction scoreableActionData)
    {
        // Add the new scoreable action then increment the current Id for the next usage
        scoreableActionsInProgress.Add(new ScorableActionInProgress(Time.time, currentID, scoreableActionData));
        return currentID++;
    }

    /// <summary>
    /// Called to mark a scoreable action in progress as finished
    /// </summary>
    /// <param name="actionInProgressID">The Id of the action to finish, should have been given when the score was first added</param>
    public void FinishScorableActionInProgress(int actionInProgressID)
    {
        // Find the action in progress that matches the id
        ScorableActionInProgress actionFinished = new ScorableActionInProgress();
        for (int i = 0; i < scoreableActionsInProgress.Count; ++i)
        {
            if (scoreableActionsInProgress[i].ID == actionInProgressID)
            {
                actionFinished = scoreableActionsInProgress[i];
            }
        }

        scoreableActionsCompleted.Add(new KeyValuePair<ScoreableAction, float>(actionFinished.scoreableActionData, Time.time - actionFinished.startTime));
    }

    /// <summary>
    /// Called to add a scoreable action that has no duration but has been completed (air tricks being an example of this
    /// </summary>
    /// <param name="scoreableActionData">The action to be added</param>
    public void AddOneShotCompletedAction(ScoreableAction scoreableActionData)
    {
        scoreableActionsCompleted.Add(new KeyValuePair<ScoreableAction, float>(scoreableActionData, 0f));
    }

    public float GetScoreFromCurrentCombo()
    {
        float baseScore = 0f;
        for (int i = 0; i < scoreableActionsCompleted.Count; ++i)
        {
            baseScore += scoreableActionsCompleted[i].Key.initalScoreValue;
            baseScore += scoreableActionsCompleted[i].Key.scorePerSecond * scoreableActionsCompleted[i].Value;
        }
        for (int i = 0; i < scoreableActionsInProgress.Count; ++i)
        {
            baseScore += scoreableActionsInProgress[i].scoreableActionData.initalScoreValue;
            baseScore += scoreableActionsInProgress[i].scoreableActionData.scorePerSecond * (Time.time - scoreableActionsInProgress[i].startTime);
        }
        return baseScore * (scoreableActionsCompleted.Count + scoreableActionsInProgress.Count);
    }

    public string GetComboText()
    {
        string comboText = GetScoreFromCurrentCombo().ToString() + " X " + (scoreableActionsCompleted.Count + scoreableActionsInProgress.Count).ToString() + '/n';
        for (int i = 0; i < scoreableActionsCompleted.Count; ++i)
        {
            comboText += scoreableActionsCompleted[i].Key.name;
            comboText += " + ";
        }
        for (int i = 0; i < scoreableActionsInProgress.Count; ++i)
        {
            comboText += scoreableActionsInProgress[i].scoreableActionData.name;
            comboText += " + ";
        }
        return comboText.Trim(new char[] {' ', '+' });
    }
}
