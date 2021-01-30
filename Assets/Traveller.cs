using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traveller : MonoBehaviour {
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

    public void Move(Vector3 direction) {
        transform.localPosition = transform.localPosition + direction;
    }
}
