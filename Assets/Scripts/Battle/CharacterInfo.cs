using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfo : MonoBehaviour
{
    BattleModeCore bCore = null;

    public Texture charIcon = null;

    Slider healthSlider = null;
    Text healthNumber = null;

    Slider specialSlider = null;
    Text specialNumber = null;

    Text name = null;

    CharacterStats stats = null;

    public enum Side { left, right }

    public Side side = Side.left;

    void Start()
    {
        bCore = BattleModeCore.GetResource();
        healthSlider = transform.Find("HealthSlider").GetComponent<Slider>();
        healthNumber = transform.Find("Health").GetComponent<Text>();
        name = transform.Find("Name").GetComponent<Text>();
        

        if(side == Side.left)
            stats = bCore.turnTable.currentCharacterTurn.stats;

    }

    void Update()
    {
        if (side == Side.left)
            stats = bCore.turnTable.currentCharacterTurn.stats;

        name.text = stats.name;

        healthSlider.maxValue = stats.hitPoints;
        healthSlider.value = stats.currentHitPoints;
        healthNumber.text = "Health: " + stats.currentHitPoints + " / " + stats.hitPoints;
    }

    public void UpdateStats(BattleCharacter character)
    {
        stats = character.stats;
    }

    //public void UpdateCharInfo()
    //{       
    //    //CharacterStats stats = bCore.turnTable.currentCharacterTurn.stats;
        
    //    //healthSlider.maxValue = stats.hitPoints;
    //    //healthSlider.value = stats.currentHitPoints;
    //    //healthNumber.text = "Health: " + stats.currentHitPoints +  " / " + stats.hitPoints;
    //}

    //public void UpdateCharInfo(BattleCharacter character)
    //{
    //    CharacterStats stats = character.stats;

    //    healthSlider.maxValue = stats.hitPoints;
    //    healthSlider.value = stats.currentHitPoints;
    //    healthNumber.text = "Health: " + stats.currentHitPoints + " / " + stats.hitPoints;
    //}
}
