using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIGameOver : GUIElement
{
    [SerializeField] GameObject[] gameOverElements;

    public override void OnGameOver()
    {
        base.OnGameOver();

        for (int i = 0; i < gameOverElements.Length; i++)
        {
            gameOverElements[i].SetActive(true);
        }
    }
}
