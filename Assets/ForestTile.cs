using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestTile : TileBase {
    void Start() {
        closeDescriptions = new List<string>(){
            "I am standing in a dense forest."
        };

        farDescriptions = new List<string>(){
            "is a dense forest",
            "is a dense forest of pine trees",
            "I can see a tree line"
        };
    }

    override public void OnEnter(Traveller traveller) {
        traveller.canSeeAdjacent = false;

        if (Random.Range(1, 5) < 1) {
            traveller.RandomizeDirection();
        }
    }
}
