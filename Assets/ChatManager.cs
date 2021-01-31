using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public enum Character {
    Traveller,
    Player,
};

public class ChatManager : MonoBehaviour {
    public Canvas choicesCanvas;
    public Canvas dialogueCanvas;
    public GameObject playerDialogue;
    public GameObject travellerDialogue;
    public Button whiteButton;
    List<Button> choiceButtons = new List<Button>();
    List<GameObject> dialogueEntries = new List<GameObject>();

    Button CreateChoice(string text, Action OnSelect) {
        Button button = Instantiate(whiteButton);
        button.transform.SetParent(choicesCanvas.transform, false);

        // Gets the text from the button prefab
        Text choiceText = button.GetComponentInChildren<Text>();
        choiceText.text = text;

        if (OnSelect != null) {
            button.onClick.AddListener(delegate {
                OnSelect();
            });
        }

        return button;
    }

    public void ClearChoices() {
        foreach (Button button in choiceButtons) {
            Destroy(button.gameObject);
        }
        choiceButtons.Clear();
    }

    public void ClearDialogue() {
        foreach (GameObject entry in dialogueEntries) {
            Destroy(entry);
        }
        dialogueEntries.Clear();
    }

    GameObject CreateDialogue(string text, Character character) {
        GameObject dialogue = Instantiate(character == Character.Player ? playerDialogue : travellerDialogue);
        dialogue.transform.SetParent(dialogueCanvas.transform, false);

        // Gets the text from the button prefab
        Text choiceText = dialogue.GetComponentInChildren<Text>();
        choiceText.text = text;

        return dialogue;
    }

    public void AddDialogue(String text, Character character) {
        if (dialogueEntries.Count > 6) {
            GameObject oldest = dialogueEntries[0];
            Destroy(oldest);
            dialogueEntries.RemoveAt(0);
        }

        dialogueEntries.Add(CreateDialogue(text, character));
    }

    public void AddChoice(String text, Action OnSelect) {
        choiceButtons.Add(CreateChoice(text, OnSelect));
    }
}
