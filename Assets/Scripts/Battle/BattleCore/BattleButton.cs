using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleButton : Button
{

    public BattleButtonBroadcaster BattleButtonBroadcaster = new BattleButtonBroadcaster();


    public override void OnPointerClick(PointerEventData eventData)
    {
        BattleButtonBroadcaster.OnButtonPressed();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        BattleButtonBroadcaster.OnButtonHovered();
    }

    public void PrintHello()
    {
        Debug.Log("Hello");
    }

}
