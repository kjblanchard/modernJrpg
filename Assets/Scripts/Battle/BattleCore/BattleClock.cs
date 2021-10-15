using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BattleClock : MonoBehaviour
{

    public static void GenerateTurnList(Battler[] battlersToCombine, HorizontalLayoutGroup hlg)
    {
        var fullBattlerTurnList = new List<battlerWithTime>();
        foreach (var _battler in battlersToCombine)
        {
            if (!_battler)
                continue;
            var fullListOfValuesForBattler = _battler.GetNext20Turns();
            fullBattlerTurnList.AddRange(fullListOfValuesForBattler.Select(battlerTimes =>
                new battlerWithTime { battler = _battler, timeValue = battlerTimes }));
        }
        var newBattlerList = fullBattlerTurnList.OrderBy(item => item.timeValue).Take(20).ToList();
        newBattlerList.ForEach(item => Debug.Log($"The battler is {item.battler.BattlerStats.BattlerName} and his turn time is {item.timeValue}"));
        for (int i = 0; i < 10; i++)
        {
            var gameObj = new GameObject();
            var imageComp = gameObj.AddComponent<Image>();
            imageComp.sprite = newBattlerList[i].battler.BattlerSprite.sprite;
            gameObj.GetComponent<RectTransform>().SetParent(hlg.transform);
            gameObj.SetActive(true);

        }
    }

    private class battlerWithTime
    {
        public Battler battler;
        public float timeValue;
    }

}
