using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPage : MonoBehaviour
{
    private static StartPage _instance;

    public static StartPage instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<StartPage>();
            }

            return _instance;
        }
    }
    [SerializeField] private TMPro.TMP_InputField amountInputField;
    [SerializeField] private GameObject panel;

    private void Awake()
    {
        ActivateStartPage(true);
    }

    public void OnStartClick()
    {
        int fieldAmount = 0;
        bool isNumeric = int.TryParse(amountInputField.text,out fieldAmount);
        if (fieldAmount > 0)
        {
            Score.instance.SetWinningScore(fieldAmount);
            EnemySpawner.instance.StartSpawning();
            PlayerController.instance.RespawnPlayer();
            ActivateStartPage(false);
        }
    }

    public void ActivateStartPage(bool setTo)
    {
        panel.SetActive(setTo);
        if (setTo)
        {
            amountInputField.Select();
            amountInputField.text = "";
        }
    }

    public bool isPageActive()
    {
        return panel.activeInHierarchy;
    }
}
