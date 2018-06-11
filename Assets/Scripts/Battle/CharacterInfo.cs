using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfo : MonoBehaviour
{
    BattleModeCore bCore = null;

    public Texture charIcon = null;

    public Slider healthSlider = null;
    public Text healthNumber = null;

    public Slider specialSlider = null;
    public Text specialNumber = null;

    void Start()
    {
        bCore = BattleModeCore.GetResource();
    }

    public void UpdateLeftCharInfo()
    {       
        CharacterStats stats = bCore.turnTable.currentCharacterTurn.stats;
        
        healthSlider.maxValue = stats.hitPoints;
        healthSlider.value = stats.currentHitPoints;
        healthNumber.text = stats.currentHitPoints +  " / " + stats.hitPoints;

        specialSlider.maxValue = stats.specialPoints;
        specialSlider.value = stats.currentSpecialPoints;
        specialNumber.text = stats.currentSpecialPoints + " / " + stats.specialPoints;

    }

    public void UpdateRightCharInfo(BattleCharacter character)
    {
        CharacterStats stats = character.stats;

        healthSlider.maxValue = stats.hitPoints;
        healthSlider.value = stats.currentHitPoints;
        healthNumber.text = stats.currentHitPoints + " / " + stats.hitPoints;

        specialSlider.maxValue = stats.specialPoints;
        specialSlider.value = stats.currentSpecialPoints;
        specialNumber.text = stats.currentSpecialPoints + " / " + stats.specialPoints;
    }
}
