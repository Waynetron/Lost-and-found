using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    Traveller traveller;

    void Start() {
        traveller = GameObject.FindGameObjectWithTag("Traveller").GetComponent<Traveller>();
    }

    void Update() {

    }
}
