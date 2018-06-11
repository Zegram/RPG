using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttacking : MonoBehaviour {

    public static List<TileData> ShowAttackOptions(SkillLibrary.Skill skill)
    {
        BattleCharacter character = TurnTable.GetResource().currentCharacterTurn;
        List<TileData> avaibleTiles = new List<TileData>();
        MapData mapData = MapData.GetResource();

        if(skill.FreeCastOnRange)
        {
            // Since its freecast we need all tiles from every direction
            for(int i = 0; i < skill.range; i++)
            {
                // X Z
                for(int j = 0; j < skill.range; j++)
                {
                    for(int k = 0; k < skill.range; k++)
                    {
                        if (mapData.GetTile(j, k) != null)
                            avaibleTiles.Add(mapData.GetTile(j, k));
                    }
                }

                // X -Z
                for (int j = 0; j < skill.range; j++)
                {
                    for (int k = 0; k < skill.range; k++)
                    {
                        if (mapData.GetTile(j, -k) != null)
                            avaibleTiles.Add(mapData.GetTile(j, -k));
                    }
                }

                // -X Z
                for (int j = 0; j < skill.range; j++)
                {
                    for (int k = 0; k < skill.range; k++)
                    {
                        if (mapData.GetTile(-j, k) != null)
                            avaibleTiles.Add(mapData.GetTile(-j, k));
                    }
                }

                // -X -Z
                for (int j = 0; j < skill.range; j++)
                {
                    for (int k = 0; k < skill.range; k++)
                    {
                        if (mapData.GetTile(-j, -k) != null)
                            avaibleTiles.Add(mapData.GetTile(-j, -k));
                    }
                }
            }

        }

        return avaibleTiles;
    }
}
