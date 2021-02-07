using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    [SerializeField]
    Tilemap tilemap;

    void Awake()
    {
        // Iterate through whole tilemap, printing out its contents
        /*
        for(int x = tilemap.cellBounds.min.x; x< tilemap.cellBounds.max.x;x++)
        {
            for(int y= tilemap.cellBounds.min.y; y< tilemap.cellBounds.max.y;y++)
            {
                for(int z= tilemap.cellBounds.min.z;z< tilemap.cellBounds.max.z;z++)
                {
                    // Debug.Log(x + " " + y + " " + z);
                    Debug.Log(x + " " + y + " " + z + " " + tilemap.GetTile( new Vector3Int(x,y,z)));
                }
            }
        }
        */
    }

    public UnityEngine.Tilemaps.TileBase getTile(int x, int y)
    {
        return tilemap.GetTile(new Vector3Int(x,y,0));
    }

    public bool IsInMap(Vector2Int position)
    {
        if(position.x < 0 || position.y < 0 || position.x >= tilemap.cellBounds.max.x || position.y >= tilemap.cellBounds.max.y)
            return false;
        else
            return true;
    }

    public bool IsPassable(UnityEngine.Tilemaps.TileBase tile)
    {
        if(tile.name == "Mountain" || tile.name == "Lake")
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public Vector3Int findGoalLocation()
    {
        for(int x = tilemap.cellBounds.min.x; x< tilemap.cellBounds.max.x;x++)
        {
            for(int y= tilemap.cellBounds.min.y; y< tilemap.cellBounds.max.y;y++)
            {
                for(int z= tilemap.cellBounds.min.z;z< tilemap.cellBounds.max.z;z++)
                {
                    UnityEngine.Tilemaps.TileBase tileBase = tilemap.GetTile( new Vector3Int(x,y,z));
                    if(tileBase != null && tileBase.name == "Goal")
                    {
                        return new Vector3Int(x,y,z); 
                    }
                }
            }
        }
        return new Vector3Int(-1, -1, -1);
    }

    void Update()
    {
        
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 13;
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector3Int tileCoordinate = tilemap.WorldToCell(mouseWorldPos);
            UnityEngine.Tilemaps.TileBase tileBase = tilemap.GetTile(tileCoordinate);
            if(tileBase != null)
                Debug.Log(tileCoordinate + " " + tileBase.name);
        }
    }

}