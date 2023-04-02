using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeaderboardData
{
    public List<int> playerScores;

    public LeaderboardData(List<int> playerScores)
    {
        this.playerScores = playerScores;
    }

    public LeaderboardData()
    {
        playerScores = new List<int>();
    }

    public bool AddScoreData(int amount)
    {
        if (!playerScores.Contains(amount))
        {
            playerScores.Add(amount);
            playerScores.Sort();
            playerScores.Reverse();
            return true;
        }

        return false;
    }

    public int GetScoreByIndex(int index)
    {
        return playerScores[index];
    }

    public int GetScoreAmount()
    {
        return playerScores.Count;
    }
}

public class Leaderboard : MonoBehaviour
{
    private static Leaderboard _instance;

    public static Leaderboard instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Leaderboard>();
            }

            return _instance;
        }
    }

    private LeaderboardData data;
    [SerializeField] private GameObject panel;
    [SerializeField] private List<ScoreAmount> _scoreAmounts;
    private string key = "playerData";

    private void Awake()
    {
        if (PlayerPrefs.HasKey(key))
        {
            data = JsonUtility.FromJson<LeaderboardData>(PlayerPrefs.GetString(key));
        }
        else
        {
            data = new LeaderboardData();
        }
    }

    public void AddScoreToLeaderboard(int score)
    {
        if (data.AddScoreData(score))
        {
            PlayerPrefs.SetString(key,JsonUtility.ToJson(data));
        }
    }

    public void SetLeadeboardAmount()
    {
        for (int i = 0; i < _scoreAmounts.Count; i++)
        {
            bool scoreActive = data.GetScoreAmount() > i;
            _scoreAmounts[i].gameObject.SetActive(scoreActive);
            if (scoreActive)
            {
                _scoreAmounts[i].SetAmount(data.GetScoreByIndex(i));
            }
        }
    }

    public void ActivateLeaderboard(bool activate)
    {
        panel.SetActive(activate);
        if(activate)SetLeadeboardAmount();
    }
}
