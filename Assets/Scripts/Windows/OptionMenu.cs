using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OptionMenu : MonoBehaviour
{
    private GameManager gameManager;
    private TMP_Text textComponent;
    public int functionIndex;

    public void Start() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        gameManager = player.GetComponent<GameManager>();

        textComponent = gameObject.GetComponent<TMP_Text>();

    }

    public void Update() {
        if (functionIndex == 1 && gameManager.gameData.playerUnlockedFunctions[1]) {
            textComponent.text = "Avancer vers la droite";
        } else if (functionIndex == 2 && gameManager.gameData.playerUnlockedFunctions[2]) {
            textComponent.text = "Avancer vers la gauche";
        } else if (functionIndex == 3 && gameManager.gameData.playerUnlockedFunctions[3]) {
            textComponent.text = "Saut";
        } else if (functionIndex == 4 && gameManager.gameData.playerUnlockedFunctions[4]) {
            textComponent.text = "Tuer ennemi";
        } else if (functionIndex == 5 && gameManager.gameData.playerUnlockedFunctions[5]) {
            textComponent.text = "Reset";
        } 
    }
}
