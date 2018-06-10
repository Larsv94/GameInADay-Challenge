using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GuessableWord : ScriptableObject
{

    public string CorrectWord;

    public string[] FalseWords;

    public int GarbageAmount;
}
