using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// The Controller that handles updating the gui from player events.
/// </summary>
public class DialogController : MonoBehaviour
{
    public bool InDialog { get; private set; }

    [SerializeField] private TMP_Text _textBoxToUpdate;
    [SerializeField] private DotweenBroadcasterComponent _dialogBoxOpenTween;
    [SerializeField] private DotweenBroadcasterComponent _dialogBoxCloseTween;
    [SerializeField] private float _timeBetweenTypewriterTyping = 0.1f;

    private bool _dialogBoxLoading;
    private int _currentLocationInDialog;
    private Dialog _dialogToDisplay;
    private const string _dialogBoxOpen = "dialogBoxOpen";
    private const string _dialogBoxClosed = "dialogBoxClose";
    private bool _typewriterTyping;
    private Coroutine _currentTypewriterCoroutine;
    private WaitForSeconds _timeBetweenTypewriterTypingWait;

    /// <summary>
    /// Register to watch for dialogBox animation completion events. On startup
    /// </summary>
    private void Start()
    {
        _dialogBoxOpenTween.DotweenCompleteEvent += OnDialogBoxOpenCompletedEvent;
        _dialogBoxCloseTween.DotweenCompleteEvent += OnDialogBoxClosedCompletedEvent;
        _timeBetweenTypewriterTypingWait = new WaitForSeconds(_timeBetweenTypewriterTyping);
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
        switch (InDialog)
        {
            case true when _typewriterTyping:
                StopTypewriter();
                return;
            case true when !_typewriterTyping:
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
        DOTween.Play(_dialogBoxOpen);
    }


    private void AdvanceDialog()
    {
        if (_currentLocationInDialog + 1 >= _dialogToDisplay.LinesOfDialog.Length)
        {
            DOTween.Play(_dialogBoxClosed);
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
        DOTween.Rewind(_dialogBoxClosed);
        DOTween.Rewind(_dialogBoxOpen);
    }

    private void StartDialogPlayerInteraction(Dialog dialogToGoThrough)
    {
        _dialogBoxLoading = false;
        _textBoxToUpdate.gameObject.SetActive(true);
        InitializeTypewriterEffect();
    }


    private void OnDialogBoxOpenCompletedEvent(object thing, EventArgs e)
    {

        StartDialogPlayerInteraction(_dialogToDisplay);
    }
    private void OnDialogBoxClosedCompletedEvent(object thing, EventArgs e)
    {
        EndDialog();
    }

    private void InitializeTypewriterEffect()
    {
        _typewriterTyping = true;
        _currentTypewriterCoroutine = StartCoroutine(RevealCharacters(_textBoxToUpdate));
    }


    private void StopTypewriter()
    {
        if (!_typewriterTyping)
            return;
        StopCoroutine(_currentTypewriterCoroutine);
        _typewriterTyping = false;
        RevealAllCharacters(_textBoxToUpdate);


    }
    /// <summary>
    /// Method revealing the text one character at a time.
    /// </summary>
    /// <returns></returns>
    IEnumerator RevealCharacters(TMP_Text textComponent)
    {
        textComponent.ForceMeshUpdate();
        var textInfo = textComponent.textInfo;
        var totalVisibleCharacters = textInfo.characterCount; // get # of visible character in text object
        var charactersVisible = 0;
        var shouldContinue = true;

        while (shouldContinue)
        {

            if (charactersVisible >= totalVisibleCharacters)
            {
                EndTypewriterText();
                shouldContinue = false;
            }

            textComponent.maxVisibleCharacters = charactersVisible; // How many characters should TextMeshPro display?

            charactersVisible += 1;

            yield return _timeBetweenTypewriterTypingWait;
        }
    }

    /// <summary>
    /// Modifies the values to stop the typewriter
    /// </summary>
    private void EndTypewriterText()
    {
        _currentTypewriterCoroutine = null;
        _typewriterTyping = false;
    }

    /// <summary>
    /// Reveals all the characters when the typewriter is typing and the player presses rightclick
    /// </summary>
    /// <param name="textComponent"></param>
    private void RevealAllCharacters(TMP_Text textComponent)
    {
        textComponent.maxVisibleCharacters = int.MaxValue;
    }


}
