using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDDebug : MonoBehaviour {

    public GameObject ModeBackground;
    public GameObject ModeText;
    GameCore gCore;
	// Use this for initialization
	void Start ()
    {
        gCore = GameCore.GetResource();

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (gCore.currentGameMode == GameCore.GameMode.Battle)
            ModeBackground.GetComponent<RawImage>().color = Color.red;

        else
            ModeBackground.GetComponent<RawImage>().color = Color.green;

        ModeText.GetComponent<Text>().text = gCore.currentGameMode.ToString();
    }
}
