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
    int travelerX;

    [SerializeField]
    int travelerY;
    
    // Start is called before the first frame update
    void Start()
    {
        travelerX = Random.Range(0, map.tiles.Length);
        travelerY = Random.Range(0, map.tiles[0].Length);

        inkManager.SetTileContext(map, travelerX, travelerY);
        inkManager.StartStory();
    }

    public void Move(int x, int y)
    {
        travelerX = travelerX + x;
        travelerY = travelerY + y;
        inkManager.SetTileContext(map, travelerX, travelerY);
    }
}
