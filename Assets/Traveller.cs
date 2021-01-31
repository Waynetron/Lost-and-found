using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Traveller : MonoBehaviour {

    [SerializeField]
    Tilemap tilemap;

    [SerializeField]
    Vector3Int tileMapPosition;

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

    public void SetPosition(int x, int y) {
        tileMapPosition.x = x;
        tileMapPosition.y = y;
        UpdateDisplayPosition();
    }

    private void UpdateDisplayPosition() {
        Vector3 displayPosition = tilemap.CellToWorld(tileMapPosition);
        displayPosition.x += 0.5f;
        displayPosition.y += 0.5f;
        transform.position = displayPosition;
    }

    public Vector3Int GetPosition() {
        return tileMapPosition;
    }
}
