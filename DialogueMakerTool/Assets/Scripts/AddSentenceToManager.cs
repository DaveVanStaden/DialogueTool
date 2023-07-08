using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddSentenceToManager : MonoBehaviour
{
    public TMP_InputField inputField;
    public TMP_InputField inputFieldName;
    public TMP_Text textNumber;
    public DialogueManager dialogueManager;
    public string previousSentence;
    public string previousName;
    public int sentenceIndex;

    private void Start()
    {
        textNumber.overflowMode = TextOverflowModes.Overflow;
        textNumber.enableWordWrapping = true;
        inputField.onValueChanged.AddListener(OnSentenceValueChanged);
        inputField.onEndEdit.AddListener(OnSentenceEndEdit);
        inputFieldName.onValueChanged.AddListener(OnCharNameValueChanged);
        inputFieldName.onEndEdit.AddListener(OnCharNameEndEdit);

        SetDialogueManager(dialogueManager);
    }
    private void UpdateSentenceText()
    {
        textNumber.text = (sentenceIndex + 1).ToString(); 
    }

    public void OnSentenceValueChanged(string text)
    {
        dialogueManager.OnInputFieldClicked(text, sentenceIndex);
        dialogueManager.inputFieldActive = true;
    }

    public void OnSentenceEndEdit(string text)
    {
        dialogueManager.OnInputFieldEndEdit(text, inputFieldName.text, this);
        dialogueManager.inputFieldActive = false;
    }

    public void OnCharNameValueChanged(string name)
    {
        dialogueManager.OnInputFieldClicked(name, sentenceIndex);
        dialogueManager.inputFieldActive = true;
    }

    public void OnCharNameEndEdit(string name)
    {
        dialogueManager.OnInputFieldEndEdit(inputField.text, name, this);
        dialogueManager.inputFieldActive = false;
    }

    public void SetDialogueManager(DialogueManager manager)
    {
        dialogueManager = manager;
        inputField.onValueChanged.RemoveAllListeners();
        inputField.onEndEdit.RemoveAllListeners();
        inputField.onValueChanged.AddListener(delegate { dialogueManager.OnInputFieldClicked(inputField.text, sentenceIndex); });
        inputField.onEndEdit.AddListener(delegate { dialogueManager.OnInputFieldEndEdit(inputField.text, inputFieldName.text, this); });

        inputFieldName.onValueChanged.RemoveAllListeners();
        inputFieldName.onEndEdit.RemoveAllListeners();
        inputFieldName.onValueChanged.AddListener(delegate { dialogueManager.OnInputFieldClicked(inputFieldName.text, sentenceIndex); });
        inputFieldName.onEndEdit.AddListener(delegate { dialogueManager.OnInputFieldEndEdit(inputField.text, inputFieldName.text, this); });
    }

    public void SetIndex(int index)
    {
        sentenceIndex = index;
        UpdateSentenceText();
    }
    
    
}

