using System.Linq;
using TMPro;
using UnityEngine;

public class TurnOrderGui : MonoBehaviour
{
    private const byte _numberOfTurnsToDisplay = 20;

    [SerializeField] private TMP_Text[] turnOrderTmpTexts = new TMP_Text[_numberOfTurnsToDisplay];


    public void UpdateBattlerNamesInTurnOrderGui(Battler[] battlers)
    {
        var battlerNames = battlers.ToList().Select(battler => battler.BattleStats.BattlerDisplayName).ToArray();
        var battlerColors = battlers.ToList().Select(battler => battler.BattleStats.PortraitColor).ToArray();
        InitializeTurnOrderTexts(battlerNames,battlerColors);

    }

    /// <summary>
    /// Modifies the text displayed For the turn order UI when it changes
    /// </summary>
    /// <param name="namesToInput">The array of names that are going to be input</param>
    private void InitializeTurnOrderTexts(string[] namesToInput, Color32[] textColor)
    {
        for (var i = 0; i < namesToInput.Length; i++)
        {
            turnOrderTmpTexts[i].text = namesToInput[i];
            turnOrderTmpTexts[i].color = textColor[i];
        }
    }
}
