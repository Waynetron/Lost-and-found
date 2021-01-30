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
    int travellerX;

    [SerializeField]
    int travellerY;
    
    // Start is called before the first frame update
    void Start()
    {
        travellerX = Random.Range(0, map.tiles.Length);
        travellerY = Random.Range(0, map.tiles[0].Length);

        inkManager.SetTileContext(map, travellerX, travellerY);
        inkManager.StartStory();
    }

    public void Move(int x, int y)
    {
        travellerX = travellerX + x;
        travellerY = travellerY + y;
        inkManager.SetTileContext(map, travellerX, travellerY);
    }
}
