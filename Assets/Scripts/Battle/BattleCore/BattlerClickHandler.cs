using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]
public class BattlerClickHandler : MonoBehaviour, IPointerDownHandler
{
    public BattleButtonBroadcaster _battleButtonBroadcaster;

    void Awake()
    {
        _battleButtonBroadcaster = new BattleButtonBroadcaster();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _battleButtonBroadcaster.OnButtonPressed();
    }
}
