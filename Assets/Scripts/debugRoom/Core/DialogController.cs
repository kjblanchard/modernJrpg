using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    [SerializeField] private GameObject _dialogTextbox;
    [SerializeField] private TMP_Text _textBoxToUpdate;
    [SerializeField] private DotweenBroadcasterComponent dialogBoxTween;
    private bool _inDialog;
    private int _currentLocationInDialog;
    private Dialog _dialogToDisplay;

    private void Start()
    {
        dialogBoxTween.DotweenCompleteEvent += OnDotweenCompletedEvent;
        dialogBoxTween.DotweenRewindEvent += OnDotweenRewindEvent;
    }


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

        _inDialog = true;
        _currentLocationInDialog = 0;
        _dialogTextbox.SetActive(true);
        _textBoxToUpdate.gameObject.SetActive(false);
        _textBoxToUpdate.text = dialogToGoThrough.LinesOfDialog[_currentLocationInDialog].Dialog;
        _dialogToDisplay = dialogToGoThrough;
        return true;
    }


    public bool AdvanceDialog(Dialog dialogToGoThrough)
    {
        if (_currentLocationInDialog + 1 >= dialogToGoThrough.LinesOfDialog.Length)
        {
            //dialogBoxTween.AnimationToControl.DORewind();
            dialogBoxTween.AnimationToControl.DOPlayBackwards();
            //EndDialog();
            return false;
        }
        _currentLocationInDialog++;
        DisplayDialog(dialogToGoThrough);
        return true;
    }

    private void EndDialog()
    {
        _inDialog = false;
        _currentLocationInDialog = 0;
        _dialogTextbox.SetActive(false);
    }

    private void DisplayDialog(Dialog dialogToGoThrough)
    {
        _textBoxToUpdate.gameObject.SetActive(true);
    }

    public void OnDotweenCompletedEvent(object thing, EventArgs e)
    {
        DisplayDialog(_dialogToDisplay);
    }

    public void OnDotweenRewindEvent(object thing, EventArgs e)
    {

        EndDialog();
    }

}
