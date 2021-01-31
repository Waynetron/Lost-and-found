using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;

public class InkManager : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;

    [SerializeField]
	private TextAsset inkJSONAsset = null;
    Story story;
    [SerializeField]
    private ChatManager chatManager;

    void Awake()
    {
        // Remove the default message
		RemoveChildren();
        story = new Story(inkJSONAsset.text);
    }
	
	public void SetTileContext(Map map, int x, int y)
    {
		UnityEngine.Tilemaps.TileBase travellerTile = map.getTile(x,y);
		if(map.IsInMap(x, y - 1))
		{
			UnityEngine.Tilemaps.TileBase aheadTile = map.getTile(x,y - 1);
			story.variablesState["aheadTile"] = aheadTile.name;
			story.variablesState["aheadPassable"] = true;
		}
		else
		{
			story.variablesState["aheadPassable"] = false;
		}
		if(map.IsInMap(x - 1, y))
		{
			UnityEngine.Tilemaps.TileBase leftTile = map.getTile(x - 1,y);
			story.variablesState["leftTile"] = leftTile.name;
			story.variablesState["leftPassable"] = true;
		}
		else
		{
			story.variablesState["leftPassable"] = false;
		}
		if(map.IsInMap(x + 1, y))
		{
			UnityEngine.Tilemaps.TileBase rightTile = map.getTile(x + 1,y);
			story.variablesState["rightTile"] = rightTile.name;
			story.variablesState["rightPassable"] = true;
		}
		else
		{
			story.variablesState["rightPassable"] = false;
		}
		if(map.IsInMap(x, y + 1))
			story.variablesState["backPassable"] = true;
		else
			story.variablesState["backPassable"] = false;
        story.variablesState["currentTile"] = travellerTile.name;
    }

    public void StartStory()
    {
        RefreshView();
    }
	
	// This is the main function called every time the story changes. It does a few things:
	// Destroys all the old content and choices.
	// Continues over all the lines of text, then displays all the choices. If there are no choices, the story is finished!
	void RefreshView () {
        // Remove all the UI on screen
        RemoveChildren();
        chatManager.ClearChoices();

        // Read all the content until we can't continue any more
        while (story.canContinue) {
			// Get the next line of the story
			string text = story.Continue ();
            chatManager.AddDialogue(text.Trim(), Character.Traveller);
        }

        // Display all the choices, if there are any!
        foreach (Choice choice in story.currentChoices) {
            chatManager.AddChoice(choice.text.Trim(), () => {
                OnClickChoiceButton(choice);
            });
        }

        // If we've read all the content and there's no choices, the story is finished!
        if (story.currentChoices.Count == 0) {
            Debug.Log("Story Finished");
        }
	}

    // When we click the choice button, tell the story to choose that choice!
    void OnClickChoiceButton(Choice choice) {
        chatManager.AddDialogue(choice.text.Trim(), Character.Player);
        ProcessMove(choice.text);
		story.ChooseChoiceIndex (choice.index);
		RefreshView();
	}

    public void ProcessMove(string move)
    {
        int xChange = 0;
        int yChange = 0;
        switch(move)
        {
            case "NORTH":
            yChange = -1;
            break;
            case "SOUTH":
            yChange = 1;
            break;
            case "EAST":
            xChange = 1;
            break;
            case "WEST":
            xChange = -1;
            break;
        }
        gameManager.Move(xChange,yChange);
    }

	// Creates a button showing the choice text
	Button CreateChoiceView (string text) {
		// Creates the button from a prefab
		Button choice = Instantiate (buttonPrefab) as Button;
		choice.transform.SetParent (canvas.transform, false);
		
		// Gets the text from the button prefab
		Text choiceText = choice.GetComponentInChildren<Text> ();
		choiceText.text = text;

		// Make the button expand to fit the text
		HorizontalLayoutGroup layoutGroup = choice.GetComponent <HorizontalLayoutGroup> ();
		layoutGroup.childForceExpandHeight = false;

		return choice;
	}

	// Destroys all the children of this gameobject (all the UI)
	void RemoveChildren () {
		int childCount = canvas.transform.childCount;
		for (int i = childCount - 1; i >= 0; --i) {
			GameObject.Destroy (canvas.transform.GetChild (i).gameObject);
		}
	}

	[SerializeField]
	private Canvas canvas = null;

	// UI Prefabs
	[SerializeField]
	private Text textPrefab = null;
	[SerializeField]
	private Button buttonPrefab = null;
}
