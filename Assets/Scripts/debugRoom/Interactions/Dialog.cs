using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Dialog", order = 1)]
public class Dialog : ScriptableObject
{
    [System.Serializable]
    public class DialogParams
    {
        [SerializeField] public string Dialog;
        [SerializeField] public string Name;
    }
    [SerializeField] public DialogParams[] LinesOfDialog;
}
