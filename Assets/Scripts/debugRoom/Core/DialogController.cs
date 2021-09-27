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

    [SerializeField] private TMP_Text _fullTmpTextToUpdate;
    [SerializeField] private DotweenBroadcasterComponent _dialogBoxOpenTween;
    [SerializeField] private DotweenBroadcasterComponent _dialogBoxCloseTween;
    [SerializeField] private float _timeBetweenTypewriterTyping = 0.1f;
    [SerializeField] private DialogButton _choice1TmpText;
    [SerializeField] private DialogButton _choice2TmpText;

    private bool _dialogBoxLoading;
    private int _currentLocationInDialog;
    private Dialog _dialogToDisplay;
    private const string _dialogBoxOpen = "dialogBoxOpen";
    private const string _dialogBoxClosed = "dialogBoxClose";
    private bool _typewriterTyping;
    private Coroutine _currentTypewriterCoroutine;
    private WaitForSeconds _timeBetweenTypewriterTypingWait;
    private int currentDialogSelection;
    private bool choiceMade = false;

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
        _fullTmpTextToUpdate.gameObject.SetActive(false);
        _fullTmpTextToUpdate.text = dialogToGoThrough.LinesOfDialog[_currentLocationInDialog].Dialog;
        _dialogToDisplay = dialogToGoThrough;
        choiceMade = false;
        DOTween.Play(_dialogBoxOpen);
    }


    private void AdvanceDialog()
    {
        if (_dialogToDisplay.LinesOfDialog[_currentLocationInDialog].IsChoice && !choiceMade)
        {
            _choice1TmpText.UpdateButtonText(_dialogToDisplay.LinesOfDialog[_currentLocationInDialog].ChoiceOptions[0]);
            _choice2TmpText.UpdateButtonText(_dialogToDisplay.LinesOfDialog[_currentLocationInDialog].ChoiceOptions[1]);
            _choice1TmpText.gameObject.SetActive(true);
            _choice2TmpText.gameObject.SetActive(true);
            return;
        }
        if (_currentLocationInDialog + 1 >= _dialogToDisplay.LinesOfDialog.Length)
        {
            DOTween.Play(_dialogBoxClosed);
            return;
        }
        _currentLocationInDialog++;
        if (_dialogToDisplay.LinesOfDialog[_currentLocationInDialog].SelectionChoice == 0 || _dialogToDisplay.LinesOfDialog[_currentLocationInDialog].SelectionChoice == currentDialogSelection)
        {
            _fullTmpTextToUpdate.text = _dialogToDisplay.LinesOfDialog[_currentLocationInDialog].Dialog;
            StartDialogPlayerInteraction(_dialogToDisplay);
        }
        else
        {
            AdvanceDialog();
        }
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
        _fullTmpTextToUpdate.gameObject.SetActive(true);
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
        _currentTypewriterCoroutine = StartCoroutine(RevealCharacters(_fullTmpTextToUpdate));
    }


    private void StopTypewriter()
    {
        if (!_typewriterTyping)
            return;
        StopCoroutine(_currentTypewriterCoroutine);
        _typewriterTyping = false;
        RevealAllCharacters(_fullTmpTextToUpdate);


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

    public void UpdateSelectionNumber(int selection)
    {
        currentDialogSelection = selection;
        choiceMade = true;
        _choice1TmpText.gameObject.SetActive(false);
        _choice2TmpText.gameObject.SetActive(false);

        AdvanceDialog();
    }


}
