using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Handles TurnTable and the visual part "VisualTurnTable" in the HUD.
public class TurnTable : MonoBehaviour {

    MapData mapData = new MapData();
   
    public List<BattleCharacter> characters = new List<BattleCharacter>();
    public BattleCharacter currentCharacterTurn = new BattleCharacter();
    //public GameObject visualTurnTable = null;
    public int turnNumber = 0;


    void Update()
    {

    }

    public void InitializeTable()
    {
        mapData = MapData.GetResource();
        characters = mapData.characters;

        // Sort characters by their SPEED value.
        characters.Sort((x, y) => x.stats.speed.CompareTo(y.stats.speed));
        characters.Reverse();

        currentCharacterTurn = characters[0];
        UpdateVisualTurnTable();
    }

    // Keep the table updated if characters have died they are removed.

    public void UpdateTable()
    {
        // Remove dead characters.
        for(int i = 0; i < characters.Count; i++)
        {
            if (characters[i].stats.DEAD)
                characters.RemoveAt(i);
        }
        UpdateVisualTurnTable();
    }

    public void NextTurn()
    {
        // Move first index to last.. (AKA delete & add it again)
        BattleCharacter oldIndex = characters[0];
        characters.RemoveAt(0);
        characters.Add(oldIndex);

        turnNumber++;
        currentCharacterTurn = characters[0];
        UpdateVisualTurnTable();
    }

    public void UpdateVisualTurnTable()
    {
       /* for (int i = 0; i < visualTurnTable.transform.childCount; i++)
        {
            visualTurnTable.transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < characters.Count; i++)
        {
            visualTurnTable.transform.GetChild(i).GetComponent<Image>().sprite = characters[i].stats.charIcon;
            visualTurnTable.transform.GetChild(i).gameObject.SetActive(true);


        }*/
    }

    public static TurnTable GetResource()
    {
        return GameObject.Find("GlobalBattleData").GetComponent<TurnTable>();
    }
}
