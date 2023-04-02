using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreAmount : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI amountText;

    public void SetAmount(int amount)
    {
        amountText.text = amount.ToString();
    }
}
