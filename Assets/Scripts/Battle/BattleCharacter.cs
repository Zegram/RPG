using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCharacter : MonoBehaviour {
    public enum Team { PlayerForces, EnemyForces }
    public enum CharacterType { Player, NPC }


    public CharacterType charType = CharacterType.NPC;
    public Team team = Team.EnemyForces;

    public CharacterStats stats;
    public TileData currentPos = new TileData();


    void OnEnable()
    {
        stats = gameObject.GetComponent<CharacterStats>();
        transform.localPosition = new Vector3(currentPos.x * 2, currentPos.height + 1, currentPos.z * 2);
    }

    void Update()
    {
        //transform.localPosition = new Vector3(currentPos.x * 2, currentPos.height + 1, currentPos.z * 2);
    }
}
