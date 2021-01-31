using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Traveller : MonoBehaviour {

    [SerializeField]
    Tilemap tilemap;

    [SerializeField]
    Vector3Int tileMapPosition;

    public Vector3 direction;
    public bool canSeeAdjacent = true;
    List<Vector3> cardinalDirections = new List<Vector3>() {
        Vector3.forward,
        Vector3.back,
        Vector3.left,
        Vector3.right
    };

    void Start() {
        RandomizeDirection();
    }

    public void RandomizeDirection() {
        int randomIndex = Random.Range(0, cardinalDirections.Count - 1);
        direction = cardinalDirections[randomIndex];
    }

    public void Move(int x, int y) {
        tileMapPosition.x = tileMapPosition.x + x;
        tileMapPosition.y = tileMapPosition.y + y;
        UpdateDisplayPosition();
    }

    public void SetPosition(int x, int y)
    {
        tileMapPosition.x = x;
        tileMapPosition.y = y;
        UpdateDisplayPosition();
    }

    private void UpdateDisplayPosition()
    {
        Vector3 displayPosition = tilemap.CellToWorld(tileMapPosition);
        displayPosition.x += 0.5f;
        displayPosition.y += 0.5f;
        transform.position = displayPosition;
    }

    public Vector3Int GetPosition()
    {
        return tileMapPosition;
    }
}
