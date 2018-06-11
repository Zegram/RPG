﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMovement : MonoBehaviour
{

    // DoubleClick
    float catchTime = 1.0f;
    float lastClickTime = 0f;
    BattleModeCore battleCore = null;
    int i = 0;

    public bool characterIsMoving = false;
    TileData tileToMove = new TileData();

    void Awake()
    {
        battleCore = BattleModeCore.GetResource();
    }

    public List<TileData> ShowMovementOptions()
    {
        BattleCharacter character = TurnTable.GetResource().currentCharacterTurn;
        List<TileData> avaibleTiles = new List<TileData>();


        if (character.stats.currentMovement <= 0)
            return null;

        int movement = 1;

        List<TileData> currTiles = new List<TileData>();
        currTiles.Add(character.currentPos);
        for (int i = 0; i < character.stats.currentMovement; i++)
        {
            for (int j = 0; j < currTiles.Count; j++)
            {
                for (int k = 0; k < currTiles[j].neighborTiles.Count; k++)
                {
                    TileData cTile = currTiles[j].neighborTiles[k];
                    if (!avaibleTiles.Contains(cTile) && cTile != character.currentPos && !cTile.unpassable && !cTile.occupied)
                    {

                        if (currTiles[j].height < cTile.height || currTiles[j].height > cTile.height)
                        {
                            if (cTile.height < currTiles[j].height)
                            {
                                if (currTiles[j].height - cTile.height <= character.stats.jump)
                                {
                                    currTiles[j].neighborTiles[k].movementCost = movement;
                                    currTiles[j].neighborTiles[k].tag = "Moveable";

                                    //Movement Path                                  
                                    // Add last tiles path
                                    // Add last tile
                                    // Add Itself                                 
                                    for (int x = 0; x < currTiles[j].movementPath.Count; x++)
                                        currTiles[j].neighborTiles[k].movementPath.Add(currTiles[j].movementPath[x]);

                                    currTiles[j].neighborTiles[k].movementPath.Add(currTiles[j].neighborTiles[k]);

                                    avaibleTiles.Add(currTiles[j].neighborTiles[k]);
                                }
                            }

                            else if (cTile.height > currTiles[j].height)
                            {
                                if (cTile.height - currTiles[j].height <= character.stats.jump)
                                {
                                    currTiles[j].neighborTiles[k].movementCost = movement;
                                    currTiles[j].neighborTiles[k].tag = "Moveable";

                                    //Movement Path
                                    
                                    for (int x = 0; x < currTiles[j].movementPath.Count; x++)
                                        currTiles[j].neighborTiles[k].movementPath.Add(currTiles[j].movementPath[x]);

                                    currTiles[j].neighborTiles[k].movementPath.Add(currTiles[j].neighborTiles[k]);

                                    avaibleTiles.Add(currTiles[j].neighborTiles[k]);
                                }
                            }
                        }
                        else
                        {
                            currTiles[j].neighborTiles[k].movementCost = movement;
                            currTiles[j].neighborTiles[k].tag = "Moveable";

                            //Movement Path                          
                            for (int x = 0; x < currTiles[j].movementPath.Count; x++)
                            {
                                currTiles[j].neighborTiles[k].movementPath.Add(currTiles[j].movementPath[x]);
                            }

                            currTiles[j].neighborTiles[k].movementPath.Add(currTiles[j].neighborTiles[k]);


                            avaibleTiles.Add(currTiles[j].neighborTiles[k]);
                        }
                    }
                }
            }
            for (int j = 0; j < avaibleTiles.Count; j++)
                currTiles.Add(avaibleTiles[j]);

            movement++;
        }

        return avaibleTiles;
    }

    void Update()
    {
        if (battleCore.showingMovementOptions)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (Time.time - lastClickTime < catchTime)
                {

                }
                else
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.tag == "Moveable")
                        {
                            MoveCharacterTo(hit.transform.GetComponent<TileData>());
                        }
                    }
                }
                lastClickTime = Time.time;
            }
        }

    }


    void MoveCharacterTo(TileData tileData)
    {
        tileToMove = new TileData(tileData);
        characterIsMoving = true;
        StartCoroutine("MoveBetweenTiles", tileToMove);

        // CLEAN
        battleCore.showingMovementOptions = false;
        battleCore.ClearMovement();

        

        // Reduce Movement
        battleCore.turnTable.currentCharacterTurn.stats.currentMovement -= tileData.movementCost;

        // Set Characters current tile to tile we are moving to + Update occupations.
        battleCore.turnTable.currentCharacterTurn.currentPos = tileData;
        battleCore.mapData.UpdateOccupation();

        // Update HUD while were at it
        battleCore.battleHUD.UpdateMovementText();
    }

    IEnumerator MoveBetweenTiles(TileData tileToMove)
    {
        while (true)
        {
            Transform currChar = battleCore.turnTable.currentCharacterTurn.transform;
            float step = 10 * Time.deltaTime;

                Vector3 target = new Vector3(tileToMove.movementPath[i].transform.localPosition.x, tileToMove.movementPath[i].transform.localPosition.y + 1, tileToMove.movementPath[i].transform.localPosition.z);
                currChar.localPosition = Vector3.MoveTowards(currChar.localPosition, target, step);

            // Rotation
            TileData currTile = battleCore.turnTable.currentCharacterTurn.currentPos;

            float xDir = currTile.x.CompareTo(tileToMove.movementPath[i].transform.localRotation.x);
            float zDir = currTile.z.CompareTo(tileToMove.movementPath[i].transform.localRotation.z);
            float rotation = 0;

            if (xDir == 0 && zDir > 0)
                rotation = 0;
            else if (xDir > 0 && zDir == 0)
                rotation = 90;
            else if (xDir == 0 && zDir < 0)
                rotation = 180;
            else if (xDir < 0 && zDir == 0)
                rotation = 270;

            currChar.localRotation = Quaternion.Euler(0.0f, rotation, 0.0f);

            //Vector3 targetDir = new Vector3(currChar.transform.localRotation.x, rotation, currChar.transform.localRotation.z);
            //Vector3 newDir = Vector3.RotateTowards(currChar.transform.up, targetDir, step, 0.0f);
            //currChar.transform.localRotation = Quaternion.LookRotation(newDir);


            //2
                if (currChar.localPosition == target)
                {
                    i++;
                        if(i >= tileToMove.movementPath.Count)
                        {
                            // Clear Movement Paths
                            for (int j = 0; j < battleCore.mapData.mapTiles.Count; j++)
                            {
                                if (battleCore.mapData.mapTiles[j].movementPath.Count != 0)
                                {
                                    battleCore.mapData.mapTiles[j].movementPath.Clear();
                                }
                            }

                            characterIsMoving = false;
                            i = 0;
                            yield break;
                        }
                }

                yield return null;
        }
    }

    public static BattleMovement GetResource()
    {
        return GameObject.Find("GlobalBattleData").GetComponent<BattleMovement>();
    }
}
