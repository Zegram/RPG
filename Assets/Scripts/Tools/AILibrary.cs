using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class AILibrary : ScriptableObject
{
    public enum gambit
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

    [System.Serializable]
    public class AIGambit
    {
        public List<gambit> gambits = new List<gambit>();
        
        public AIGambit() { }
    }

    static readonly string assetName = "AILibrary";
    static readonly string resourcePath = "Libraries/";

    static AILibrary instance = null;

    public static AILibrary GetResource()
    {
        if (instance == null)

            instance = Resources.Load<AILibrary>(resourcePath + assetName);

        return instance;
    }

    public List<AIGambit> AIGambits = new List<AIGambit>();


}
