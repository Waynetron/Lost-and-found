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
    float BUTTON_SPACING = 0.2f;

    void Start() {
        AddChoice("North", () => { });
        AddChoice("South", () => { });
        AddChoice("East", () => { });
        AddChoice("West", () => { });

        AddDialogue("Hello?", Character.Player);
        AddDialogue("On, you're alive!", Character.Traveller);
        AddDialogue("Yep, still breathing. Where are you? ", Character.Player);
        AddDialogue("I’ve no idea... I just woke up here.", Character.Traveller);
        AddDialogue("Can you check what's up ahead. I have a feeling in my bones. Greeeease lighting!", Character.Player);
        AddDialogue("On, you're alive!", Character.Traveller);
        AddDialogue("Yep, still breathing. Where are you? ", Character.Player);
        AddDialogue("I’ve no idea... I just woke up here.", Character.Traveller);
        AddDialogue("Hello?", Character.Player);
        AddDialogue("On, you're alive!", Character.Traveller);
        AddDialogue("Yep, still breathing. Where are you? ", Character.Player);
        AddDialogue("I’ve no idea... I just woke up here.", Character.Traveller);
        AddDialogue("Hello?", Character.Player);
        AddDialogue("On, you're alive!", Character.Traveller);
        AddDialogue("Yep, still breathing. Where are you? ", Character.Player);
        AddDialogue("I’ve no idea... I just woke up here.", Character.Traveller);
    }

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

    void ClearChoices() {
        foreach (Button button in choiceButtons) {
            Destroy(button);
        }
        choiceButtons.Clear();
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
        if (dialogueEntries.Count > 9) {
            int lastIndex = dialogueEntries.Count - 1;
            GameObject oldest = dialogueEntries[lastIndex];
            Destroy(oldest);
            dialogueEntries.RemoveAt(lastIndex);
        }

        dialogueEntries.Add(CreateDialogue(text, character));
    }

    public void AddChoice(String text, Action OnSelect) {
        choiceButtons.Add(CreateChoice(text, OnSelect));
    }
}
