using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAI : MonoBehaviour {

    public enum targetGambit
    {
        nearest,
        furthest,
        highestHP,
        highestMaxHP,
        lowestHP,
        lowestMaxHP,
        highestMovement,
        lowestMovement,
        highestJump,
        lowestJump
    }

    public enum aiState
    {
        targeting,
        moving,
        onMovement,
        attacking,
        end
    }

    public targetGambit tGambit = targetGambit.nearest;
    public BattleCharacter target = null;
    public BattleCharacter bCharacter = null;
    BattleModeCore bCore = null;
    BattleMovement bMovement = null;
    BattleAttacking bAttacking = null;
    public aiState state = aiState.targeting;

    void OnEnable()
    {
        bCharacter = transform.GetComponent<BattleCharacter>();
        bCore = BattleModeCore.GetResource();
        bMovement = BattleMovement.GetResource();
        bAttacking = BattleAttacking.GetResource();
    }

    public void ChooseTarget()
    {
        if(tGambit == targetGambit.nearest)
        {
            // 	Distance: c2 = a2 + b2
            List<BattleCharacter> characters = MapData.GetResource().characters;
            List<BattleCharacter> enemies = new List<BattleCharacter>();
            BattleCharacter closestEnemy = null;
            float closestDistance = 99999;
            float currentDistance = 0;

            for(int i = 0; i < characters.Count; i++)
            {
                if(characters[i].team != bCharacter.team)
                {
                    enemies.Add(characters[i]);
                }
            }

            for(int j = 0; j < enemies.Count; j++)
            {
                currentDistance = Mathf.Pow(bCharacter.currentPos.x - enemies[j].currentPos.x, 2) + Mathf.Pow(bCharacter.currentPos.z - enemies[j].currentPos.z, 2);
                if (currentDistance <= closestDistance)
                {
                    closestDistance = currentDistance;
                    closestEnemy = enemies[j];
                }
            }
            target = closestEnemy;
            state = aiState.moving;
        }
    }

    public void Movement()
    {
        state = aiState.onMovement;
        // Movement for enemies with attack range of 1
        bCore.movementTiles = bMovement.ShowMovementOptions();
        TileData closestTile = null;
        float closestDistance = 99999;
        float currentDistance = 0;

        for (int i = 0; i < bCore.movementTiles.Count; i++)
        {
            currentDistance = Mathf.Pow(target.currentPos.x - bCore.movementTiles[i].x, 2) + Mathf.Pow(target.currentPos.z - bCore.movementTiles[i].z, 2);
            if (currentDistance <= closestDistance)
            {
                closestDistance = currentDistance;
                closestTile = bCore.movementTiles[i];
            }
        }

        bMovement.MoveCharacterTo(closestTile);
    }

    public void Attack()
    {
        List<TileData> attackingTiles = BattleAttacking.ShowAttackOptions();

        for(int i = 0; i < attackingTiles.Count; i++)
        {
            if (attackingTiles[i] == target.currentPos)
                bAttacking.AttackCharacter(target);               
        }
        state = aiState.end;
    }
}
