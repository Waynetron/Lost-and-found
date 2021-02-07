using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.Tilemaps;

public class InkManager : MonoBehaviour {
    [SerializeField]
    GameManager gameManager;

    [SerializeField]
	private TextAsset tileJSON;
    
    [SerializeField]
    private ChatManager chatManager;

    [SerializeField]
    WeatherEffects weatherEffects;
    
    [SerializeField]
    private Map map;
    
    [SerializeField]
    private Traveller traveller;

    Story story;

    private Vector3Int goalLocation;

    void Awake() {
        story = new Story(tileJSON.text);
        story.ObserveVariable("stormRemaining", (string varName, object newValue) =>
        {
            weatherEffects.updateWeatherState(newValue);
        });
        goalLocation = map.findGoalLocation();
        Debug.Log(goalLocation);
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
			TileBase aheadTile = map.getTile(ahead.x, ahead.y);
			story.variablesState["aheadTile"] = aheadTile.name;
			story.variablesState["aheadPassable"] = map.IsPassable(aheadTile);
		} else {
			story.variablesState["aheadPassable"] = false;
        }
        
		if(map.IsInMap(left)) {
			TileBase leftTile = map.getTile(left.x, left.y);
			story.variablesState["leftTile"] = leftTile.name;
			story.variablesState["leftPassable"] = map.IsPassable(leftTile);
		} else
		{
			story.variablesState["leftPassable"] = false;
        }
        
		if(map.IsInMap(right)) {
			TileBase rightTile = map.getTile(right.x, right.y);
			story.variablesState["rightTile"] = rightTile.name;
			story.variablesState["rightPassable"] =  map.IsPassable(rightTile);
		} else {
			story.variablesState["rightPassable"] = false;
        }
        
		if(map.IsInMap(behind)) {
			TileBase backTile = map.getTile(behind.x, behind.y);
			story.variablesState["backTile"] = backTile.name;
			story.variablesState["backPassable"] =  map.IsPassable(backTile);
		} else {
            story.variablesState["backPassable"] = false;
        }

        TileBase travellerTile = map.getTile(tileMapPosition.x, tileMapPosition.y);
        story.variablesState["currentTile"] = travellerTile.name;

        int xTileDistance = Mathf.Abs(tileMapPosition.x - goalLocation.x);
        int yTileDistance = Mathf.Abs(tileMapPosition.y - goalLocation.y);
        int tileDistance = xTileDistance + yTileDistance;
        story.variablesState["tileDistance"] = tileDistance;
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
        // Remove direction arrows for chatbox version of choice
        string trimmedText = choice.text.Trim();
        if(trimmedText.StartsWith("↑") || trimmedText.StartsWith("↶") ||
        trimmedText.StartsWith("↰") || trimmedText.StartsWith("↱"))
        {
            trimmedText = trimmedText.Substring(1);
        }
        chatManager.AddDialogue(trimmedText, Character.Player);

        string result = ProcessMovement(choice);
        if (result == "success") {
            story.ChooseChoiceIndex(choice.index);
        }
        if (result == "swept_downstream") {
            story.ChoosePathString("swept_downstream");
        }
        else if (result == "success_crossed_river") {
            story.ChoosePathString("success_crossed_river");
        }
        else if (result == "failed_river_crossing") {
            story.ChoosePathString("failed_river_crossing");
        }
        else if (result == "failed_movement") {
            story.ChoosePathString("failed_movement");
        }

        UpdateStoryVariables();
        StartCoroutine(ContinueStory(0.25f));
    }

    List<Vector2Int> GetAdjacentPositions(Vector2Int position) {
        return new List<Vector2Int>() {
            new Vector2Int(position.x + 1, position.y),
            new Vector2Int(position.x - 1, position.y),
            new Vector2Int(position.x, position.y + 1),
            new Vector2Int(position.x, position.y - 1),
        };
    }

    // Returns a list of all river tile positions connected to the given river tile position
    // Looks only in a single direction (does not turn corners)
    List<Vector2Int> FindConnectedRiverPositions(Vector2Int position, List<Vector2Int> currentPositions, Vector2Int direction) {
        TileBase tile = map.getTile(position.x, position.y);
        if (tile.name == "River") {
            currentPositions.Add(position);
            Vector2Int nextPosition = position + direction;
            return FindConnectedRiverPositions(nextPosition, currentPositions, direction);
        } else {
            return currentPositions;
        }
    }

    // Takes as input a list of river tile positions.
    // From that list, returns any adjacent tile positions where the traveller could be potentially washed ashore.
    List<Vector2Int> FindAdjacentRiverBankPositions(List<Vector2Int> riverPositions) {
        List<Vector2Int> riverBanks = new List<Vector2Int>();

        foreach (Vector2Int riverPosition in riverPositions) {
            foreach (Vector2Int adjacent in GetAdjacentPositions(riverPosition)) {
                TileBase tile = map.getTile(adjacent.x, adjacent.y);
                if (map.IsInMap(adjacent) && map.IsPassable(tile) && tile.name != "River") {
                    riverBanks.Add(adjacent);
                }
            }
        }

        return riverBanks;
    }

    // Dumps the traveller somewhere down river
    void DumpTravellerDownstream(Vector2Int riverPosition) {
        List <Vector2Int> diagonals = new List<Vector2Int>() {
            new Vector2Int(1, 1),
            new Vector2Int(-1, 1),
            new Vector2Int(1, -1),
            new Vector2Int(-1, -1),
        };

        List<Vector2Int> connectedRiverPositions = new List<Vector2Int>();
        foreach (Vector2Int direction in diagonals) {
            List<Vector2Int> riversInDirection = FindConnectedRiverPositions(riverPosition, new List<Vector2Int>(), direction);
            connectedRiverPositions.AddRange(riversInDirection);
        }

        List<Vector2Int> riverBankPositions = FindAdjacentRiverBankPositions(connectedRiverPositions);

        // remove current position from potential river bank positions
        riverBankPositions.FindAll((Vector2Int position) => position.Equals(riverPosition));

        // from the remaining river bank positions, choose one at random
        // if for some reason there are no valid positions remaining, then the traveller remains in place
        int randomIndex = Random.Range(0, riverBankPositions.Count - 1);
        if (riverBankPositions.Count > 0) {
            traveller.SetPosition(riverBankPositions[randomIndex]);
        }
        traveller.RandomizeDirection();
    }

    string ProcessMovement(Choice choice) {
        Vector2Int direction = GetDirectionFromChoiceText(choice.text);
        Vector2Int targetPosition = traveller.GetTileMapPosition() + direction;
        TileBase targetTile = map.getTile(targetPosition.x, targetPosition.y);

        // impassable terrain
        if (!map.IsPassable(targetTile)) {
            traveller.SetDirection(-direction);
            return "failed_movement";
        }

        // river crossing
        if (targetTile.name == "River") {
            // chance to be swept downstream 
            // NOTE: Random.Range(1, 5) is equal to a 1 in 4 chance as the max value is exclusive.
            if (Random.Range(1, 5) == 1) {
                // TODO: find a position downstream and set the traveller down there
                DumpTravellerDownstream(targetPosition);
                return "swept_downstream";
            }

            Vector2Int oppositeRiverBank = targetPosition + direction;
            TileBase oppositeRiverBankTile = map.getTile(oppositeRiverBank.x, oppositeRiverBank.y);
            
            // other side of the river is blocked by impassable terrain
            if (!map.IsPassable(oppositeRiverBankTile)) {
                traveller.SetDirection(-direction);
                return "failed_river_crossing";
            }

            // crossed the river
            traveller.SetPosition(oppositeRiverBank);
            return "success_crossed_river";
        }

        // standard move
        traveller.Move(direction);
        return "success";
    }

    Vector2Int GetDestinationChoiceText(string choiceText) {
        Vector2Int currentDirection = traveller.direction;

        if (choiceText.Contains("↑")) {
            return currentDirection;
        } else if (choiceText.Contains("↶")) {
            return -currentDirection;
        } else if (choiceText.Contains("↰")) {
            Vector2 left = Vector2.Perpendicular(currentDirection);
            return new Vector2Int((int) left.x, (int) left.y);
        }

        Vector2 right = -Vector2.Perpendicular(currentDirection);
        return new Vector2Int((int) right.x, (int) right.y);
    }

    Vector2Int GetDirectionFromChoiceText(string choiceText) {
        Vector2Int currentDirection = traveller.direction;

        if (choiceText.Contains("↑")) {
            return currentDirection;
        } else if (choiceText.Contains("↶")) {
            return -currentDirection;
        } else if (choiceText.Contains("↰")) {
            Vector2 left = Vector2.Perpendicular(currentDirection);
            return new Vector2Int((int) left.x, (int) left.y);
        }

        Vector2 right = -Vector2.Perpendicular(currentDirection);
        return new Vector2Int((int) right.x, (int) right.y);
    }
}
