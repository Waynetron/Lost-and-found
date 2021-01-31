using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    InkManager inkManager;

    [SerializeField]
    Map map;

    [SerializeField]
    Traveller traveller;
    
    // Start is called before the first frame update
    void Start() {
        traveller.SetPosition(13,10);
        inkManager.UpdateStoryAtCurrentPosition();
        inkManager.StartStory();
    }
}
