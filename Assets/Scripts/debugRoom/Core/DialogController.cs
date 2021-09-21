using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    [SerializeField] private GameObject _dialogTextbox;
    [SerializeField] private TMP_Text _textBoxToUpdate;
    private bool _inDialog;
    private int _currentLocationInDialog;



    /// <summary>
    /// Triggers the interaction dialog for interacting with dialogs
    /// </summary>
    /// <param name="dialogToGoThrough">The Scriptable object dialog that will be gone though</param>
    /// <returns>Returns true if the dialog should continue, and false if the dialog is ending</returns>
    public bool TriggerInteractionDialog(Dialog dialogToGoThrough)
    {
        if (_inDialog)
        {
            return AdvanceDialog(dialogToGoThrough);
        }

        return InitializeDialog(dialogToGoThrough);
    }

    private bool InitializeDialog(Dialog dialogToGoThrough)
    {
        _inDialog = true;
        _currentLocationInDialog = 0;
        _dialogTextbox.SetActive(true);
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
        _inDialog = false;
        _currentLocationInDialog = 0;
        _dialogTextbox.SetActive(false);
    }

    private void DisplayDialog(Dialog dialogToGoThrough)
    {
        _textBoxToUpdate.text = dialogToGoThrough.LinesOfDialog[_currentLocationInDialog].Dialog;
    }

}
