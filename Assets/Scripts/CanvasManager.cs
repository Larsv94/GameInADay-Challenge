using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public CanvasGroup GameOverGroup;

    public Text GameOverText;

    public Text Tries;

    public Text correctLetters;

    public Text inWordLetters;



    void Start()
    {
        GameOverGroup.alpha = 0f;
        GameOverGroup.blocksRaycasts = false;
    }

    public void ShowGameOver(string message)
    {
        GameOverGroup.alpha = 1f;
        GameOverGroup.blocksRaycasts = true;
        GameOverText.text = message;
    }

    public void updateCorrectLetters(int amount)
    {
        correctLetters.text = amount.ToString();
    }

    public void updateInWordLetters(int amount)
    {
        inWordLetters.text = amount.ToString();
    }

    public void updateTries(int amount)
    {
        Tries.text = amount.ToString();
    }
}
