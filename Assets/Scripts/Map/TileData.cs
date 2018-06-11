using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData : MonoBehaviour
{
    MapData mapData = null;

    [Header("Name & Passable")]
    public string tileName = "";
    public bool unpassable = false;
    public bool occupied = false;

    [Header("General (Analyzed)")]
    public int x = 0;
    public int z = 0;
    public int height = 0;
    public int movementCost = 1;
    

    public List<TileData> neighborTiles = new List<TileData>();
    public List<TileData> movementPath = new List<TileData>();

    public TileData() { }
    public TileData(TileData tileData)
    {
        tileName = tileData.tileName;
        unpassable = tileData.unpassable;
        occupied = tileData.occupied;

        x = tileData.x;
        z = tileData.z;
        height = tileData.height;
        movementCost = tileData.movementCost;

        neighborTiles = tileData.neighborTiles;
        movementPath = tileData.movementPath;
    }


    void Awake()
    {
        AnalyzeTileData();
    }
    void Start()
    {
        mapData = MapData.GetResource();      
        GetNeighbors();
    }

    void AnalyzeTileData()
    {
        x =         (int)gameObject.transform.localPosition.x / 2;
        z = (int)gameObject.transform.localPosition.z / 2;
        height = (int)gameObject.transform.localPosition.y;
    }

    void GetNeighbors()
    {
        for(int i = 0; i < mapData.mapTiles.Count; i++)
        {
            TileData cTile = mapData.mapTiles[i];

            if (cTile.x + 1 == x && cTile.z == z)
                neighborTiles.Add(cTile);

            if (cTile.x - 1 == x && cTile.z == z)
                neighborTiles.Add(cTile);

            if (cTile.x == x && cTile.z + 1 == z)
                neighborTiles.Add(cTile);

            if (cTile.x == x && cTile.z - 1 == z)
                neighborTiles.Add(cTile);
        }
    }

    

}
