using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Handles TurnTable and the visual part "VisualTurnTable" in the HUD.
public class TurnTable : MonoBehaviour
{

    MapData mapData = new MapData();

    public List<BattleCharacter> characters;
    public BattleCharacter currentCharacterTurn;
    //public GameObject visualTurnTable = null;
    public int turnNumber = 0;

    [Header("Visual Turntable")]
    public GameObject displayBlock = null;
    public List<GameObject> displayBlocks = new List<GameObject>();
    public Vector3 startingPos = new Vector3(-370, 200, 0);


    void Start()
    {
        displayBlock = (GameObject)Resources.Load("HUD/Displayblock") as GameObject;

        if (displayBlock == null)
            Debug.Log("Could not locate displayBlock!");
    }
    void Update()
    {

    }

    public void InitializeTable()
    {
        characters = new List<BattleCharacter>();
        currentCharacterTurn = new BattleCharacter();

        mapData = MapData.GetResource();
        characters = mapData.characters;

        // Sort characters by their SPEED value.
        if (characters[1].stats != null)
        {
            characters.Sort((x, y) => x.stats.speed.CompareTo(y.stats.speed));
            characters.Reverse();
        }

        currentCharacterTurn = characters[0];
        UpdateVisualTurnTable();
    }

    // Keep the table updated if characters have died they are removed.

    public void UpdateTable()
    {
        // Remove dead characters.
        for (int i = 0; i < characters.Count; i++)
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

    public void ClearTable()
    {
        characters.Clear();

        for(int i = 0; i < displayBlocks.Count; i++)
        {
            GameObject.Destroy(displayBlocks[i]);
        }

        displayBlocks.Clear();
        currentCharacterTurn = null;
    }

    // Update the visual turntable (blocks)
    // 1. Remove extra blocks. 2. Add blocks if needed. 3. Arrange blocks into right positions, 4. Update blocks with correct sprites.
    public void UpdateVisualTurnTable()
    {
        // Remove extra blocks if any
        if (displayBlocks.Count > characters.Count)
        {
            // Destroy GameObject block
            for (int j = 0; j < displayBlocks.Count - characters.Count; j++)
            {
                GameObject.Destroy(displayBlocks[characters.Count + j]);
                
            }

            // Remove empty blocks from list
            for(int i = 0; i < displayBlocks.Count; i++)
            {
                if(displayBlocks[i] == null)
                {
                    displayBlocks.RemoveAt(i);
                }
            }
        }

        // Add and/or update blocks for existing characters.              
        if(characters.Count > displayBlocks.Count)
        {
            for(int i = displayBlocks.Count; i < characters.Count; i++)
            {
                GameObject block = new GameObject();
                block = Instantiate(displayBlock);
                block.transform.SetParent(GameObject.Find("TurnTable").transform);
                displayBlocks.Add(block);
            }
        }

        // Arrange blocks
        for (int j = 0; j < displayBlocks.Count; j++)
        {
            displayBlocks[j].transform.localPosition = new Vector3(startingPos.x + (40 * j), startingPos.y, 0);
        }

        // Update blocks with the character information
        for (int i = 0; i < displayBlocks.Count; i++)
        {
            //displayBlocks[i].GetComponent<DisplayBlock>().representing = characters[i];
            if (characters[i].stats.charIcon != null)
                displayBlocks[i].GetComponent<Image>().sprite = characters[i].stats.charIcon;

            else
                Debug.Log("Character " + characters[i].name + " has no character icon.");
        }
    }

    public static TurnTable GetResource()
    {
        return GameObject.Find("GlobalBattleData").GetComponent<TurnTable>();
    }
}
