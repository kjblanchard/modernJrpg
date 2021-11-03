using System.Linq;
using UnityEngine;

public class TurnOrderGui : MonoBehaviour
{
    private const byte _numberOfTurnsToDisplay = 20;
    public void DisplayCanvas(bool isEnabled) => _canvas.enabled = isEnabled;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private TurnOrderSlot[] turnorderSprites = new TurnOrderSlot[_numberOfTurnsToDisplay];


    public void UpdateBattlerPicturesInTurnOrderGui(Battler[] battlers)
    {
        var battlerSprites = battlers.ToList().Select(battler => battler.BattleStats.BattlerPortrait).ToArray();
        var battlerColors = battlers.ToList().Select(battler => battler.BattleStats.PortraitColor).ToArray();
        var battlerNames = battlers.ToList().Select(battler => battler.BattleStats.BattlerDisplayName).ToArray();
        InitializeTurnOrderPictures(battlerSprites, battlerNames ,battlerColors);

    }

    /// <summary>
    /// Modifies the text displayed For the turn order UI when it changes
    /// </summary>
    /// <param name="namesToInput">The array of names that are going to be input</param>
    private void InitializeTurnOrderPictures(Sprite[] spritesToInput, string[] namesToInput, Color32[] textColor)
    {
        for (var i = 0; i < namesToInput.Length; i++)
        {
            turnorderSprites[i].InitializeTurnOrderBox(namesToInput[i],textColor[i],spritesToInput[i]);
            //turnorderSprites[i].sprite = spritesToInput[i];
            //turnOrderTmpTexts[i].text = namesToInput[i];
            //turnOrderTmpTexts[i].color = textColor[i];
        }
    }
}
