using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    public bool inputFieldActive = false;
    public List<string> dialogueTitles;
    public List<string> dialogueLines;
    public GameObject newSentence;
    public List<GameObject> sentenceHolders;
    public int sentenceAmount;
    public string selectedSentence;
    public int selectedIndex;

    // Update is called once per frame
    private void Start()
    {
        sentenceAmount = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !inputFieldActive)
        {
            GameObject newSentenceObject = Instantiate(newSentence, gameObject.transform.position, Quaternion.identity, gameObject.transform);
            AddSentenceToManager addSentenceToManager = newSentenceObject.GetComponent<AddSentenceToManager>();
            addSentenceToManager.dialogueManager = this;
            addSentenceToManager.SetIndex(sentenceHolders.Count);
            PanelDragHandler panelDragHandler = newSentenceObject.GetComponent<PanelDragHandler>();

            if (addSentenceToManager != null)
            {
                addSentenceToManager.dialogueManager = this;
                panelDragHandler.dialogueManager = this;
            }
            else
            {
                Debug.LogError("AddSentenceToManager component not found on the instantiated newSentence object.");
            }

            sentenceHolders.Add(newSentenceObject);
            AddSentence(string.Empty, string.Empty); 
            AddSentenceAmount(1);
            
        }
    }
    public void AddSentence(string newSentence, string title)
    {
        if (dialogueLines == null)
        {
            dialogueLines = new List<string>();
        }
        if (dialogueTitles == null)
        {
            dialogueTitles = new List<string>();
        }

        dialogueLines.Add(newSentence);
        dialogueTitles.Add(title);
    }

    public void RemoveSentence(string sentenceToRemove)
    {
        dialogueLines.Remove(sentenceToRemove);
    }
    public void RemoveTitle(string titleToRemove)
    {
        dialogueTitles.Remove(titleToRemove);
    }

    private void ModifySentenceByText(int index, string newText)
    {
        dialogueLines[index] = newText;
    }
    private void ModifyNameByText(int index, string newText)
    {
        dialogueTitles[index] = newText;
    }
    public void OnInputFieldClicked(string text, int index)
    {
        if (!string.IsNullOrEmpty(text))
        {
            selectedSentence = text;
            selectedIndex = index;
        }
    }
    public void UpdateAllIndices()
    {
        for (int i = 0; i < sentenceHolders.Count; i++)
        {
            AddSentenceToManager sentenceToManager = sentenceHolders[i].GetComponent<AddSentenceToManager>();
            sentenceToManager.SetIndex(i);
        }
    }
    public void RemoveFromSentenceHolders(GameObject sentenceObject)
    {
        sentenceHolders.Remove(sentenceObject);
    }
    public void UpdateNumbering()
    {
        for (int i = 0; i < sentenceHolders.Count; i++)
        {
            AddSentenceToManager sentenceToManager = sentenceHolders[i].GetComponent<AddSentenceToManager>();
            sentenceToManager.textNumber.text = (i + 1).ToString();
        }
    }

    public void OnInputFieldEndEdit(string text,string name, AddSentenceToManager addSentenceToManager)
    {
        if (!string.IsNullOrEmpty(text))
        {
            ModifySentenceByText(addSentenceToManager.sentenceIndex, text);
            addSentenceToManager.previousSentence = text;
        }
        if (!string.IsNullOrEmpty(name))
        {
            ModifyNameByText(addSentenceToManager.sentenceIndex, name);
            addSentenceToManager.previousName = name;
        }
    }
    public void AddSentenceAmount(int amount)
    {
        sentenceAmount += amount;
    }
    public void RemoveSentenceAmount()
    {
        sentenceAmount--;
    }
}
