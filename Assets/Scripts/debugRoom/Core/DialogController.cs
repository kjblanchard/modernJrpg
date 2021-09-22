using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

/// <summary>
/// The Controller that handles updating the gui from player events.
/// </summary>
public class DialogController : MonoBehaviour
{
    public bool InDialog { get; private set; }

    [SerializeField] private GameObject _dialogTextbox;
    [SerializeField] private TMP_Text _textBoxToUpdate;
    [SerializeField] private DotweenBroadcasterComponent dialogBoxOpenTween;
    [SerializeField] private DotweenBroadcasterComponent dialogBoxCloseTween;

    private bool _dialogBoxLoading;
    private int _currentLocationInDialog;
    private Dialog _dialogToDisplay;
    private const string dialogBoxOpen = "dialogBoxOpen";
    private const string dialogBoxClosed = "dialogBoxClose";

    /// <summary>
    /// Register to watch for dialogBox animation completion events. On startup
    /// </summary>
    private void Start()
    {
        dialogBoxOpenTween.DotweenCompleteEvent += OnDialogBoxOpenCompletedEvent;
        dialogBoxCloseTween.DotweenCompleteEvent += OnDialogBoxClosedCompletedEvent;
    }


    /// <summary>
    /// Triggers the interaction dialog for interacting with dialogs
    /// </summary>
    /// <param name="dialogToGoThrough">The Scriptable object dialog that will be gone though</param>
    /// <returns>Returns true if the dialog should continue, and false if the dialog is ending</returns>
    public void HandlePlayerRightClick(Dialog dialogToGoThrough)
    {
        if (_dialogBoxLoading)
            return;
        if (InDialog)
        {
            AdvanceDialog();
            return;
        }
        InitializeDialogInteraction(dialogToGoThrough);
    }

    private void InitializeDialogInteraction(Dialog dialogToGoThrough)
    {
        InDialog = true;
        _dialogBoxLoading = true;
        _currentLocationInDialog = 0;
        _textBoxToUpdate.gameObject.SetActive(false);
        _textBoxToUpdate.text = dialogToGoThrough.LinesOfDialog[_currentLocationInDialog].Dialog;
        _dialogToDisplay = dialogToGoThrough;
        DOTween.Play(dialogBoxOpen);
    }


    private void AdvanceDialog()
    {
        if (_currentLocationInDialog + 1 >= _dialogToDisplay.LinesOfDialog.Length)
        {
            DOTween.Play(dialogBoxClosed);
            return;
        }
        _currentLocationInDialog++;
        _textBoxToUpdate.text = _dialogToDisplay.LinesOfDialog[_currentLocationInDialog].Dialog;
        StartDialogPlayerInteraction(_dialogToDisplay);
    }

    private void EndDialog()
    {
        InDialog = false;
        _currentLocationInDialog = 0;
        DOTween.Rewind(dialogBoxClosed);
        DOTween.Rewind(dialogBoxOpen);
    }

    private void StartDialogPlayerInteraction(Dialog dialogToGoThrough)
    {
        _dialogBoxLoading = false;
        _textBoxToUpdate.gameObject.SetActive(true);
    }


    public void OnDialogBoxOpenCompletedEvent(object thing, EventArgs e)
    {

        StartDialogPlayerInteraction(_dialogToDisplay);
    }
    public void OnDialogBoxClosedCompletedEvent(object thing, EventArgs e)
    {
        EndDialog();
    }

}
