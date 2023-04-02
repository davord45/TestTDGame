using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    private static GameOver _instance;

    public static GameOver instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameOver>();
            }

            return _instance;
        }
    }

    [SerializeField] private GameObject gameOverPage;
    [SerializeField] private Image overlayImage;
    [SerializeField] private TMPro.TextMeshProUGUI gameOverText;
    [SerializeField] private TMPro.TextMeshProUGUI buttonText;

    public event Action GameOverActive;

    public void ShowGameOver(bool show)
    {
        if (gameOverPage.activeInHierarchy == show) return;
        gameOverPage.SetActive(show);
        if (show)
        {
            EnemySpawner.instance.DestroyAllEnemies();
            GameOverActive?.Invoke();
            overlayImage.DOFade(0, 1f).From();
            gameOverText.transform.DOMove(new Vector3(gameOverText.transform.position.x,
                gameOverText.transform.position.y+10, gameOverText.transform.position.z),1f).From().SetEase(Ease.OutExpo);
            SetGameOverText();
        }
    }

    public void SetGameOverText()
    {
        if (Score.instance.isGoalReached())
        {
            gameOverText.text = "YOU DESTROYED ALL TURRETS!";
            Leaderboard.instance.AddScoreToLeaderboard(Score.instance.GetCurrentScore());
            buttonText.text = "Back To Start";
        }
        else
        {
            gameOverText.text = "GAME OVER";
            buttonText.text = "Try Again";
        }
    }

    public void OnContinueButtonPress()
    {
        if (Score.instance.isGoalReached())
        {
            StartPage.instance.ActivateStartPage(true);
        }
        else
        {
            EnemySpawner.instance.StartSpawning();
            Score.instance.ResetScore();
        }
        ShowGameOver(false);
    }

    public void OnShowLeaderboard()
    {
        Leaderboard.instance.ActivateLeaderboard(true);
    }
}
