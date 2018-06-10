using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum ValueType
{
    BAD,
    GARBAGE,
    WORD
}

public class GameplayManager : MonoBehaviour
{

    public static List<Button> AllButtonsList = new List<Button>();
    public static Dictionary<string, Button> WordsButtons = new Dictionary<string, Button>();
    private static GameplayManager _Instance;
    private static int tries = 4;

    public static GameplayManager Instance
    {
        get
        {
            if (_Instance == null) //create GameObject with KeyboardEventManager if world lacks this instance
            {
                _Instance = GameObject.FindObjectOfType(typeof(GameplayManager)) as GameplayManager;
                if (_Instance == null)
                {

                    _Instance = new GameObject("GameplayManager Instance", typeof(GameplayManager)).GetComponent<GameplayManager>();
                    _Instance.Initialize();
                    
                }
            }
            return _Instance;
        }
    }

    public CanvasManager canvasManager;
    public ScreenBuilder screenBuilder;

    public GuessableWord[] Words;
    bool bIsGameOver = false;

    public GuessableWord currentWord;
    // Use this for initialization
    void Start()
    {
        Initialize();
        AllButtonsList.Clear();
        WordsButtons.Clear();

        canvasManager = GameObject.FindObjectOfType<CanvasManager>();
        screenBuilder = GameObject.FindObjectOfType<ScreenBuilder>();
        int randomIndex = Random.Range(0, Words.Length);
        print(Words.Length);
        print(randomIndex);
        currentWord = Words[randomIndex];
        screenBuilder.Build(currentWord);
    }

    // Update is called once per frame
    void Update()
    {
        if (bIsGameOver)
        {
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private void Initialize()
    {
        AllButtonsList = new List<Button>();
        tries = 4;
    }

    public void registerButton(Button btn)
    {
        if (btn != null)
        {
            AllButtonsList.Add(btn);
        }

    }

    public void OnWordClicked()
    {
        var go = EventSystem.current.currentSelectedGameObject;
        if (go != null)
        {
            string selectedWord = go.GetComponentInChildren<Text>().text;
            if (selectedWord.ToUpper().Equals(currentWord.CorrectWord.ToUpper()))
            {
                bIsGameOver = true;
                canvasManager.ShowGameOver("WINNER");
            }
            else
            {
                DecreaseTries(selectedWord);
            }
        }
        else
            Debug.Log("currentSelectedGameObject is null");
    }
    public void OnGarbageClicked()
    {
        var go = EventSystem.current.currentSelectedGameObject;
        Button btn = go.GetComponent<Button>();
        btn.interactable = false;
        removeDud();
    }

    private void increaseTries()
    {
        tries++;
        canvasManager.updateTries(tries);
    }

    private void removeDud()
    {
        int randomIndex = Random.Range(0, currentWord.FalseWords.Length);
        string randomWord = currentWord.FalseWords[randomIndex];
        Button dud = WordsButtons[randomWord.ToUpper()];
        if (dud)
        {
            if (dud.interactable)
            {
                dud.interactable = false;
            }
            else
            {
                increaseTries();
            }
        }
    }

    public void OnBadValueClicked()
    {
        var go = EventSystem.current.currentSelectedGameObject;
        if (go != null)
        {
            string selectedWord = go.GetComponentInChildren<Text>().text;
            DecreaseTries(selectedWord);
        }

        Debug.Log("You clicked on a wrong word");
    }

    private void DecreaseTries(string word)
    {
        string correctWord = currentWord.CorrectWord.ToUpper();
        string guessedWord = word.ToUpper();
        int similar = 0;
        int inWord = 0;

        for (int i = 0; i < guessedWord.Length; i++)
        {
            if (guessedWord[i].Equals(correctWord[i]))
            {
                similar++;
            }

            if (correctWord.Contains(guessedWord[i].ToString()))
            {
                inWord++;
            }
        }
        tries--;
        canvasManager.updateCorrectLetters(similar);
        canvasManager.updateInWordLetters(inWord);
        canvasManager.updateTries(tries);
        if (tries <= 0)
        {
            Failed();
        }
    }

    private void Failed()
    {
        Debug.Log("You're dead");
        foreach (Button btn in AllButtonsList)
        {
            btn.interactable = false;
        }

        bIsGameOver = true;
        canvasManager.ShowGameOver("GAME OVER");
        tries = 4;
    }

}
