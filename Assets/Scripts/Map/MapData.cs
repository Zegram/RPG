using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{
    public List<TileData> mapTiles;
    public List<BattleCharacter> characters;

    void Awake()
    {
        mapTiles = new List<TileData>();
        characters = new List<BattleCharacter>();

        Transform map = GameObject.Find("Map").transform;
        for(int i = 0; i < map.childCount; i++)
        {
            if (map.GetChild(i).GetComponent<TileData>() != null)
                mapTiles.Add(map.GetChild(i).GetComponent<TileData>());

            else if (map.GetChild(i).GetComponent<BattleCharacter>() != null)
                characters.Add(map.GetChild(i).GetComponent<BattleCharacter>());
        }


    }

    public void UpdateOccupation()
    {
        for(int i = 0; i < mapTiles.Count; i++)
        {
            bool tileOccupied = false;

            for(int j = 0; j < characters.Count; j++)
            {
                if (mapTiles[i] == characters[j].currentPos)
                    tileOccupied = true;
            }

            if (tileOccupied)
                mapTiles[i].occupied = true;

            else
                mapTiles[i].occupied = false;
        }


    }

    public TileData GetTile(int x, int z)
    {
        TileData tile = null;

        for(int i = 0; i < mapTiles.Count; i++)
        {
            if (mapTiles[i].x == x && mapTiles[i].z == z)
                tile = mapTiles[i];
        }

        return tile;
    }

    public static MapData GetResource()
    {
        return GameObject.Find("GlobalBattleData").GetComponent<MapData>();
    }

}
