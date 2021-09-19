using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    [SerializeField] private TMP_Text _textBoxToUpdate;
    public bool inDialog;
    private int _currentLocationInDialog;



    /// <summary>
    /// Triggers the interaction dialog for interacting with dialogs
    /// </summary>
    /// <param name="dialogToGoThrough">The Scriptable object dialog that will be gone though</param>
    /// <returns>Returns true if the dialog should continue, and false if the dialog is ending</returns>
    public bool TriggerInteractionDialog(Dialog dialogToGoThrough)
    {
        if (inDialog)
        {
            return AdvanceDialog(dialogToGoThrough);
        }
        inDialog = true;
        _currentLocationInDialog = 0;
        DisplayDialog(dialogToGoThrough);
        return true;
    }


    public bool AdvanceDialog(Dialog dialogToGoThrough)
    {
        if (_currentLocationInDialog + 1 >= dialogToGoThrough.LinesOfDialog.Length)
        {
            EndDialog();
            return false;
        }
        _currentLocationInDialog++;
        DisplayDialog(dialogToGoThrough);
        return true;
    }

    private void EndDialog()
    {
        _textBoxToUpdate.text = "";
        inDialog = false;
        _currentLocationInDialog = 0;
    }

    private void DisplayDialog(Dialog dialogToGoThrough)
    {
        _textBoxToUpdate.text = dialogToGoThrough.LinesOfDialog[_currentLocationInDialog].Dialog;
    }

}
