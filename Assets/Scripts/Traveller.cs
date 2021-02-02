using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Traveller : MonoBehaviour {

    [SerializeField]
    Tilemap tilemap;

    [SerializeField]
    Vector2Int tileMapPosition;

    public Vector2Int direction;
    public bool canSeeAdjacent = true;
    List<Vector2Int> cardinalDirections = new List<Vector2Int>() {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

    void Start() {
        RandomizeDirection();
    }

    void UpdateGameObjectRotation() {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(
            transform.rotation.x,
            transform.rotation.y,
            angle
        );
    }

    public void RandomizeDirection() {
        int randomIndex = Random.Range(0, cardinalDirections.Count - 1);
        direction = cardinalDirections[randomIndex];
        UpdateGameObjectRotation();
    }

    public void Move(Vector2Int newDirection) {
        direction = newDirection;
        tileMapPosition.x = tileMapPosition.x + newDirection.x;
        tileMapPosition.y = tileMapPosition.y + newDirection.y;
        UpdateDisplayPosition();
        UpdateGameObjectRotation();
    }

    public void SetPosition(Vector2Int newPosition) {
        tileMapPosition = newPosition;
        UpdateDisplayPosition();
    }

    public void SetDirection(Vector2Int newDirection) {
        direction = newDirection;
        UpdateGameObjectRotation();
    }

    private void UpdateDisplayPosition() {
        Vector3 displayPosition = tilemap.CellToWorld(new Vector3Int(tileMapPosition.x, tileMapPosition.y, 0));
        displayPosition.x += 0.5f;
        displayPosition.y += 0.5f;
        transform.position = displayPosition;
    }

    public Vector2Int GetTileMapPosition() {
        return tileMapPosition;
    }
}
