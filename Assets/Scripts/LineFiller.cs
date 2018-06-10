using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Word
{
    public string text;
    public ValueType type;

    public Word(string text, ValueType type)
    {
        this.text = text;
        this.type = type;
    }
}

public class LineFiller : MonoBehaviour
{

    [SerializeField] private int maxLetters = 10;
    [SerializeField] private int maxWordSize = 3;
    public Button wordPrefab;

    private int letterAmount;


    public void BuildLine(int garbage)
    {
        letterAmount = maxLetters;
        letterAmount -= (2 * garbage);

        List<Word> wordList = new List<Word>();
        while (letterAmount > 0)
        {
            int randomSize = (int)Random.Range(1.0f, (float)maxWordSize);
            while (letterAmount - randomSize < 0)
            {
                randomSize--;
            }

            string btnText = RandomString(randomSize);
           wordList.Add(new Word(btnText, ValueType.BAD));
            
            letterAmount -= randomSize;

        }
        insertGarbage(wordList, garbage);
        foreach (Word word in wordList)
        {
            InitiateWord(word);
        }
        
    }


    public void BuildLine(string WordToInsert, int garbage)
    {
        letterAmount = maxLetters;
        letterAmount -= WordToInsert.Length;
        letterAmount -= (2 * garbage);
        List<Word> wordList = new List<Word>();
        while (letterAmount > 0)
        {
            int randomSize = (int)Random.Range(1.0f, (float)maxWordSize);
            while (letterAmount - randomSize < 0)
            {
                randomSize--;
            }

            string btnText = RandomString(randomSize);
            wordList.Add(new Word(btnText,ValueType.BAD));
            letterAmount -= randomSize;

        }
        wordList.Insert((int)Random.Range(0,wordList.Count),new Word(WordToInsert,ValueType.WORD));
        insertGarbage(wordList,garbage);
        foreach (Word word in wordList)
        {
            InitiateWord(word);
        }
    }

    void insertGarbage(List<Word> list, int amount)
    {
        string[] placeHolder = new[] {"<>", "()", "{}", "[]"};
        int randomGarbage = Random.Range(0, placeHolder.Length);
        int randomIndex = Random.Range(0, list.Count);
        for (int i = 0; i < amount; i++)
        {
            list.Insert(randomIndex,new Word(placeHolder[randomGarbage],ValueType.GARBAGE));
        }
    }
    public void OnButtonClick()
    {
        var go = EventSystem.current.currentSelectedGameObject;
        if (go != null)
            
            Debug.Log("Clicked on : " + go.GetComponentInChildren<Text>().text);
        else
            Debug.Log("currentSelectedGameObject is null");
    }

    private void InitiateWord(Word word)
    {
        Button newButton = Instantiate(wordPrefab, this.transform);
        GameplayManager.AllButtonsList.Add(newButton);
        ValueType type = word.type;
        switch (type)
        {
            case ValueType.BAD:
                newButton.onClick.AddListener(
                    GameplayManager.Instance.OnBadValueClicked
                );
            break;
            case ValueType.GARBAGE:
                newButton.onClick.AddListener(
                    GameplayManager.Instance.OnGarbageClicked
                );
                break;
            case ValueType.WORD:
                GameplayManager.WordsButtons.Add(word.text,newButton);
                newButton.onClick.AddListener(
                    GameplayManager.Instance.OnWordClicked
                );
                break;
        }
        
        newButton.GetComponentInChildren<Text>().text = word.text;
        LayoutRebuilder.MarkLayoutForRebuild(newButton.transform as RectTransform);
        LayoutRebuilder.MarkLayoutForRebuild(this.transform as RectTransform);
    }

    private string RandomString(int length)
    {
        const string pool = "~!@#$%^&*(_+-={]:;|\\>,.?/|";
        var chars = Enumerable.Range(0, length)
            .Select(x => pool[(int)Random.Range(0, pool.Length)]);
        return new string(chars.ToArray());
    }
}
