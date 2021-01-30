using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public Tile [][] tiles;

    void Awake()
    {
        System.Array tileArray = System.Enum.GetValues(typeof(Tile.type));

        tiles  = new Tile[8][];
        for(int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = new Tile[8];
            for(int j = 0; j < tiles[i].Length; j++)
            {
                tiles[i][j] = new Tile();
                Tile.type tileType = (Tile.type)tileArray.GetValue(UnityEngine.Random.Range(0,tileArray.Length));
                tiles[i][j].tileType = tileType;
            }
        }

        for(int i = 0; i < tiles.Length; i++)
        {
            string row = i +": ";
            for(int j = 0; j < tiles[i].Length; j++)
            {
                row = row + " " + tiles[i][j].tileType;
            }
            Debug.Log(row);
        }
    }

    public Tile getTile(int x, int y)
    {
        return tiles[x][y];
    }

    public bool IsInMap(int x, int y)
    {
        if(x < 0 || y < 0 || x >= tiles.Length || y >= tiles[0].Length)
            return false;
        else
            return true;
    }
}