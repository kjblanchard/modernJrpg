using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnOrderGui : MonoBehaviour
{
    private const byte _numberOfTurnsToDisplay = 20;

    [SerializeField] private TMP_Text[] turnOrderTmpTexts = new TMP_Text[_numberOfTurnsToDisplay];


    public void InitializeTurnOrderTexts(string[] namesToInput)
    {
        for (int i = 0; i < namesToInput.Length; i++)
        {
            turnOrderTmpTexts[i].text = namesToInput[i];
        }
    }
}
