using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentKeyUpdate : MonoBehaviour
{
    private TMP_Text text;
    private GameObject player;
    private GameManager gameManager;
    public int functionIndex;

    private void Start() {
        player = GameObject.FindWithTag("Player");

        text = GetComponent<TMP_Text>();
        gameManager = player.GetComponent<GameManager>();

    }
    private void Update() {
        if (gameManager.gameData.playerUnlockedFunctions[functionIndex]) {
            text.text = gameManager.gameData.playerFunctionsKey[functionIndex].ToString();
        }
    }
}
