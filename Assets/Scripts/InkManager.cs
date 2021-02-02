using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class InkManager : MonoBehaviour {
    [SerializeField]
    GameManager gameManager;

    [SerializeField]
	private TextAsset tileJSON;
    
    [SerializeField]
    private ChatManager chatManager;
    
    [SerializeField]
    private Map map;
    
    [SerializeField]
    private Traveller traveller;

    Story story;

    void Awake() {
        story = new Story(tileJSON.text);
    }

    public void UpdateStoryVariables() {
        Vector2Int tileMapPosition = traveller.GetTileMapPosition();
        Vector2Int ahead = tileMapPosition + traveller.direction;
        Vector2Int behind = tileMapPosition - traveller.direction;

        Vector2 leftVector = Vector2.Perpendicular(traveller.direction);
        Vector2Int leftVectorInt = new Vector2Int((int) leftVector.x, (int) leftVector.y);
        Vector2Int left = tileMapPosition + leftVectorInt;

        Vector2Int rightVectorInt = -leftVectorInt;
        Vector2Int right = tileMapPosition + rightVectorInt;

       

		if(map.IsInMap(ahead)) {
			UnityEngine.Tilemaps.TileBase aheadTile = map.getTile(ahead.x, ahead.y);
			story.variablesState["aheadTile"] = aheadTile.name;
			story.variablesState["aheadPassable"] = map.IsPassable(aheadTile);
		} else {
			story.variablesState["aheadPassable"] = false;
        }
        
		if(map.IsInMap(left)) {
			UnityEngine.Tilemaps.TileBase leftTile = map.getTile(left.x, left.y);
			story.variablesState["leftTile"] = leftTile.name;
			story.variablesState["leftPassable"] = map.IsPassable(leftTile);
		} else
		{
			story.variablesState["leftPassable"] = false;
        }
        
		if(map.IsInMap(right)) {
			UnityEngine.Tilemaps.TileBase rightTile = map.getTile(right.x, right.y);
			story.variablesState["rightTile"] = rightTile.name;
			story.variablesState["rightPassable"] =  map.IsPassable(rightTile);
		} else {
			story.variablesState["rightPassable"] = false;
        }
        
		if(map.IsInMap(behind)) {
			UnityEngine.Tilemaps.TileBase backTile = map.getTile(behind.x, behind.y);
			story.variablesState["backTile"] = backTile.name;
			story.variablesState["backPassable"] =  map.IsPassable(backTile);
		} else {
            story.variablesState["backPassable"] = false;
        }

        UnityEngine.Tilemaps.TileBase travellerTile = map.getTile(tileMapPosition.x, tileMapPosition.y);
        story.variablesState["currentTile"] = travellerTile.name;
    }

    public void StartStory() {
        UpdateStoryVariables();
        StartCoroutine(ContinueStory(0));
    }

    IEnumerator ContinueStory(float delay) {
        yield return new WaitForSecondsRealtime(0.25f);

        if (story.canContinue) {
            string text = story.Continue();
            
            chatManager.AddDialogue(text.Trim(), Character.Traveller);

            if (story.currentTags.Contains("disoriented")) {
                traveller.RandomizeDirection();
                UpdateStoryVariables();
            }

            // recursively continue the story until all dialogue has been displayed
            StartCoroutine(ContinueStory(0.25f));
        }
        else if (HasWon()) {
            Debug.Log("Story Finished");
        } else {
            DisplayChoices();
        }
    }

    bool HasWon() {
        return story.currentChoices.Count == 0;
    }

    void DisplayChoices() {
        foreach (Choice choice in story.currentChoices) {
            chatManager.AddChoice(choice.text.Trim(), () => {
                OnClickChoiceButton(choice);
            });
        }
    }

    // When we click the choice button, tell the story to choose that choice!
    void OnClickChoiceButton(Choice choice) {
        chatManager.ClearChoices();
        chatManager.ClearDialogue();
        chatManager.AddDialogue(choice.text.Trim(), Character.Player);
        traveller.Move(TextToDirection(choice.text, traveller.direction));
        UpdateStoryVariables();
		story.ChooseChoiceIndex (choice.index);
        StartCoroutine(ContinueStory(0.25f));
    }

    Vector2Int TextToDirection(string text, Vector2Int currentDirection) {
        Vector2 direction = new Vector2(currentDirection.x, currentDirection.y);

        if (text == "forward") {
            return currentDirection;
        } else if (text == "back") {
            return -currentDirection;
        } else if (text == "left") {
            Vector2 left = Vector2.Perpendicular(currentDirection);
            return new Vector2Int((int) left.x, (int) left.y);
        }

        Vector2 right = -Vector2.Perpendicular(currentDirection);
        return new Vector2Int((int) right.x, (int) right.y);
    }
}
