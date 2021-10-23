using TMPro;
using UnityEngine;

public class TurnOrderGui : MonoBehaviour
{
    private const byte _numberOfTurnsToDisplay = 20;

    [SerializeField] private TMP_Text[] turnOrderTmpTexts = new TMP_Text[_numberOfTurnsToDisplay];


    /// <summary>
    /// Modifies the text displayed For the turn order UI when it changes
    /// </summary>
    /// <param name="namesToInput">The array of names that are going to be input</param>
    public void InitializeTurnOrderTexts(string[] namesToInput)
    {
        for (var i = 0; i < namesToInput.Length; i++)
        {
            turnOrderTmpTexts[i].text = namesToInput[i];
        }
    }
}
