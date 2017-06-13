﻿using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (GameManager.gameManager.GameStateCheck() == GameManager.GameState.START)
        {
            if (Input.GetButtonDown("Submit"))
            {
                GameManager.gameManager.GameStateSet(GameManager.GameState.PLAY);
            }
        }
	}
}
