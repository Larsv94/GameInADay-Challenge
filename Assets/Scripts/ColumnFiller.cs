using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ColumnFiller : MonoBehaviour
{
    [SerializeField] public LineFiller lineTemplate;

    [SerializeField] private int MaxLines;
    private int randomLines;

    private List<string> AllWords;
    

    private List<string> lines;

    private int[] garbagePerLine ;


   

    public void BuildColumn(List<string> wordsList,int garbage)
    {
        AllWords = wordsList;
        
        SplitGarbage(garbage);

        DefineLines();
        
        CreateLines();
    }

    private void CreateLines()
    {
        for (int i = 0; i < lines.Count; i++)
        {
            string word = lines[i];
            LineFiller newLine = Instantiate(lineTemplate, this.transform);
            switch (word)
            {
                case "":
                    newLine.BuildLine(garbagePerLine[i]);
                    break;
                default:
                    newLine.BuildLine(word.ToUpper(), garbagePerLine[i]);
                    break;
            }
        }
    }

    private void SplitGarbage(int totalGarbage)
    {
        garbagePerLine = new int[MaxLines];
        for (int i = 0; i < garbagePerLine.Length; i++)
        {
            garbagePerLine[i] = 0;
        }
        for (int i = 0; i < totalGarbage; i++)
        {
            int index = Random.Range(0, MaxLines);
            garbagePerLine[index]++;
        }
    }

    private void DefineLines()
    {
        lines = new List<string>();
            randomLines = MaxLines - AllWords.Count;
        
        for (int i = 0; i < randomLines; i++)
        {
            lines.Add("");
        }
        foreach (string Fake in AllWords)
        {
            int range = lines.Count;
            int randPosition = (int)Random.Range(0.0f, range);
            lines.Insert(randPosition, Fake);
        }

        
        
    }
}
