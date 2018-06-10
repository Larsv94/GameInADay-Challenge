using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenBuilder : MonoBehaviour
{

    public int ColumnAmount;
    public ColumnFiller ColumnPrefab;
    public int maxGarbagePerColumn = 1;

    private List<string>[] WordsPerColumn;
    private int[] GarbagePerColumn;

    private GuessableWord currentGuessableWord;

    void Start()
    {
        
    }

    public void Build(GuessableWord word)
    {
        currentGuessableWord = word;
        Initialize();
        SplitWords();
        SplitGarbageValues();
        BuildColumns();
    }

    void BuildColumns()
    {
        for (int i = 0; i < ColumnAmount; i++)
        {
            ColumnFiller newColumn = Instantiate(ColumnPrefab, this.transform);
            newColumn.BuildColumn(WordsPerColumn[i], GarbagePerColumn[i]);
            LayoutRebuilder.MarkLayoutForRebuild(newColumn.transform as RectTransform);
        }
        LayoutRebuilder.MarkLayoutForRebuild(this.transform as RectTransform);
    }

    void Initialize()
    {
        


        WordsPerColumn= new List<string>[ColumnAmount];
        for (int i = 0; i < WordsPerColumn.Length; i++)
        {
            WordsPerColumn[i]= new List<string>();
        }

        GarbagePerColumn = new int[ColumnAmount];
        for (int y = 0; y < GarbagePerColumn.Length; y++)
        {
            GarbagePerColumn[y] = 0;
        }
    }

    void SplitWords()
    {
        foreach (string word in currentGuessableWord.FalseWords)
        {
            int randomIndex = Random.Range(0, ColumnAmount);
            WordsPerColumn[randomIndex].Add(word);
        }

        int correctIndex = Random.Range(0, ColumnAmount - 1);
        WordsPerColumn[correctIndex].Add(currentGuessableWord.CorrectWord);

    }

    void SplitGarbageValues()
    {
        int count = currentGuessableWord.GarbageAmount;
        while (count >0)
        {
            int randIndex = Random.Range(0, ColumnAmount);
            int randAmount = Random.Range(0, maxGarbagePerColumn + 1);
            if (count < randAmount)
            {
                randAmount = count;
            }
            GarbagePerColumn[randIndex] += randAmount;
            count -= randAmount;
        }

        
    }
}
