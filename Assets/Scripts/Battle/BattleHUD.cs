using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour {

    public Text movementText = null;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void UpdateMovementText()
    {
        if (BattleModeCore.GetResource().turnTable.currentCharacterTurn != null)
        {
            CharacterStats currStats = BattleModeCore.GetResource().turnTable.currentCharacterTurn.stats;
            movementText.text = currStats.currentMovement + " / " + currStats.movement;
        }
    }

    public static BattleHUD GetResource()
    {
        return GameObject.Find("GlobalBattleData").GetComponent<BattleHUD>();
    }
}
