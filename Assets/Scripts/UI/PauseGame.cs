using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    [SerializeField] private List<Sprite> icons;
    [SerializeField] private Image buttonIcon;
    public void Pause()
    {
        if (Time.timeScale == 1f)
        {
            Time.timeScale = 0f;
            buttonIcon.sprite = icons[1];
        }
        else
        {
            Time.timeScale = 1f;
            buttonIcon.sprite = icons[0];
        }
    }
}
