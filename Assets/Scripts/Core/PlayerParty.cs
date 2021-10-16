using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParty : MonoBehaviour
{
    public BattlerStats[] CurrentParty => _playerPartyBattlers;
    [SerializeField] private BattlerStats[] _playerPartyBattlers;

}
