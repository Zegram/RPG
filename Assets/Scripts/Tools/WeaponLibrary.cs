using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponLibrary : ScriptableObject
{
    public enum weaponType { Sword, Staff, Gun };
    public enum rarity { Common, Uncommon, Rare, Legendary };

    [System.Serializable]
    public class Weapon
    {
        public string name = "";
        public weaponType type = weaponType.Sword;
        public rarity rarity = rarity.Common;
        public Sprite weaponSprite = null;
        // 3D Model here???

        public int damage = 0;
        public int range = 0;

        public int goldCost = 0;

        public Weapon() { }

        // Editor
        [HideInInspector]
        public bool show = false;
    }

    static readonly string assetName = "WeaponLibrary";
    static readonly string resourcePath = "Libraries/";

    static WeaponLibrary instance = null;
    public static WeaponLibrary GetResource()
    {

        if (instance == null)

            instance = Resources.Load<WeaponLibrary>(resourcePath + assetName);

        return instance;

    }

    public List<Weapon> weapons = new List<Weapon>();
}
