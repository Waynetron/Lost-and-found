using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traveller : MonoBehaviour {
    public void Move(Vector3 direction) {
        transform.localPosition = transform.localPosition + direction;
    }
}
