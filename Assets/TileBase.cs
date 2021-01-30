using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBase : MonoBehaviour {
    public List<string> closeDescriptions = new List<string>(){
        "I am standing in a beautiful meadow."
    };
    public List<string> farDescriptions = new List<string>(){
        "is a meadow.",
        "I can see a meadow."
    };

    virtual public void OnEnter(Traveller traveller) {
        // does nothing
    }

    string GetRandomDescription(List<string> descriptions) {
        int randomIndex = Random.Range(0, descriptions.Count - 1);
        return descriptions[randomIndex];
    }

    public string GetCloseDescription() {
        return GetRandomDescription(closeDescriptions);
    }

    public string GetFarDescription() {
        return GetRandomDescription(farDescriptions);
    }
}
