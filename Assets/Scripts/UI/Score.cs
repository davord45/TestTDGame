using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    private static Score _instance;

    public static Score instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Score>();
            }

            return _instance;
        }
    }
    
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    private int currentScore = 0;

    [SerializeField] int winningAmount=20;

    public void IncreaseScore()
    {
        currentScore++;
        scoreText.text = currentScore.ToString();
        if(isGoalReached())GameOver.instance.ShowGameOver(true);
    }

    public void SetWinningScore(int amount)
    {
        winningAmount = amount;
        ResetScore();
    }

    public bool isGoalReached()
    {
        return winningAmount <= currentScore;
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }

    public void ResetScore()
    {
        currentScore = 0;
        scoreText.text = currentScore.ToString();
    }
}
