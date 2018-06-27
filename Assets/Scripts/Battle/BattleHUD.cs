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
            if (currStats != null)               
                movementText.text = currStats.currentMovement + " / " + currStats.movement;
        }
    }


    public void ActivateBattleChoices(bool active)
    {
        GameObject battleChoices = GameObject.Find("BattleChoices");

        for (int i = 0; i < battleChoices.transform.childCount; i++)
        {
            var child = battleChoices.transform.GetChild(i).gameObject;
            if (child != null)
                child.SetActive(active);
        }
    }

    public static BattleHUD GetResource()
    {
        return GameObject.Find("GlobalBattleData").GetComponent<BattleHUD>();
    }
}
