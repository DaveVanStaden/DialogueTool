using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using SFB;

public class SaveAndLoad : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public GameObject newSentencePrefab;

    public void SaveListToFile()
    {
        DialogueData dialogueData = new DialogueData();
        dialogueData.xCords = new List<float>();
        dialogueData.yCords = new List<float>();
        for (int i = 0; i < dialogueManager.sentenceAmount; i++)
        {
            dialogueData.xCords.Add(dialogueManager.sentenceHolders[i].transform.position.x);
            dialogueData.yCords.Add(dialogueManager.sentenceHolders[i].transform.position.y);
        }

        dialogueData.dialogueLines = dialogueManager.dialogueLines;
        dialogueData.dialogueTitles = dialogueManager.dialogueTitles;
        dialogueData.dialogueAmount = dialogueManager.sentenceAmount;
        string json = JsonUtility.ToJson(dialogueData);

        // Use the file browser to select the save file path
        string saveFilePath = StandaloneFileBrowser.SaveFilePanel("Save Dialogue Data", "", "listData", "json");
        if (!string.IsNullOrEmpty(saveFilePath))
        {
            File.WriteAllText(saveFilePath, json);
        }
    }

    public void OnLoadButtonClicked()
    {
        // Use the file browser to select the file to load
        string[] loadFilePaths = StandaloneFileBrowser.OpenFilePanel("Load Dialogue Data", "", "json", false);
        if (loadFilePaths.Length > 0)
        {
            string loadFilePath = loadFilePaths[0];
            LoadListFromFile(loadFilePath, newSentencePrefab, dialogueManager);
        }
    }

    public void LoadListFromFile(string filePath, GameObject newSentencePrefab, DialogueManager dialogueManager)
    {

        string json = File.ReadAllText(filePath);
        DialogueData dialogueData = JsonUtility.FromJson<DialogueData>(json);


            // Remove active sentences
            foreach (GameObject sentenceHolder in dialogueManager.sentenceHolders)
            {
                Destroy(sentenceHolder);
            }

            dialogueManager.sentenceHolders.Clear();
            dialogueManager.dialogueLines.Clear();
            dialogueManager.dialogueTitles.Clear();
            dialogueManager.sentenceAmount = dialogueData.dialogueAmount;

            GameObject dialogueHolder = GameObject.Find("DialogueHolder");
            for (int i = 0; i < dialogueData.dialogueLines.Count; i++)
            {
                string title = dialogueData.dialogueTitles[i];
                string sentence = dialogueData.dialogueLines[i];
                int existingIndex = dialogueManager.dialogueLines.FindIndex(line => line == sentence);

                if (existingIndex >= 0)
                {
                    GameObject sentenceHolder = dialogueManager.sentenceHolders[existingIndex];
                    Transform sentenceTransform = sentenceHolder.transform.Find("Sentence");

                    if (sentenceTransform != null)
                    {
                        TMP_InputField sentenceInputField = sentenceTransform.GetComponent<TMP_InputField>();

                        if (sentenceInputField != null)
                        {
                            sentenceInputField.text = sentence;
                        }
                    }
                }
                else
                {
                    GameObject newSentenceObject = Instantiate(newSentencePrefab, dialogueHolder.transform);
                    newSentenceObject.transform.position =
                        new Vector3(dialogueData.xCords[i], dialogueData.yCords[i], 0);
                    newSentenceObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

                    AddSentenceToManager addSentenceToManager = newSentenceObject.GetComponent<AddSentenceToManager>();
                    PanelDragHandler panelDragHandler = newSentenceObject.GetComponent<PanelDragHandler>();
                    addSentenceToManager.dialogueManager = dialogueManager;

                    if (addSentenceToManager != null)
                    {
                        addSentenceToManager.dialogueManager = dialogueManager;
                        panelDragHandler.dialogueManager = dialogueManager;
                    }
                    else
                    {
                        Debug.LogError(
                            "AddSentenceToManager component not found on the instantiated newSentence object.");
                    }

                    dialogueManager.sentenceHolders.Add(newSentenceObject);
                    dialogueManager.dialogueLines.Add(sentence);
                    dialogueManager.dialogueTitles.Add(title);

                    Transform sentenceTransform = newSentenceObject.transform.Find("Sentence");

                    if (sentenceTransform != null)
                    {
                        TMP_InputField sentenceInputField = sentenceTransform.GetComponent<TMP_InputField>();

                        if (sentenceInputField != null)
                        {
                            sentenceInputField.text = sentence;
                        }
                    }

                    Transform charNameTransform = newSentenceObject.transform.Find("CharName");

                    if (charNameTransform != null)
                    {
                        TMP_InputField charNameInputField = charNameTransform.GetComponent<TMP_InputField>();

                        if (charNameInputField != null)
                        {
                            charNameInputField.text = title;
                        }
                    }

                    addSentenceToManager.sentenceIndex = i; 
                }
            }
            dialogueManager.UpdateAllIndices();
            dialogueManager.inputFieldActive = false;
    }
    public void ExportFile()
    {
        string filePath = StandaloneFileBrowser.SaveFilePanel("Save Dialogue File", "", "dialogue", "json");

        if (!string.IsNullOrEmpty(filePath))
        {
            ExportData exportData = new ExportData();
            exportData.dialogueLines = dialogueManager.dialogueLines;
            exportData.dialogueTitles = dialogueManager.dialogueTitles;

            string json = JsonUtility.ToJson(exportData);
            File.WriteAllText(filePath, json);

            Debug.Log("Dialogue file exported: " + filePath);
        }
    }
}