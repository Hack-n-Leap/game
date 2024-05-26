using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OptionMenu : MonoBehaviour
{
    private GameManager gameManager;
    private TMP_Text textComponent;

    public void Start() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        gameManager = player.GetComponent<GameManager>();

        textComponent = gameObject.GetComponent<TMP_Text>();

    }

    public void Update() {
        if (textComponent.name.EndsWith("1") && gameManager.gameData.playerUnlockedFunctions[0]) {
            textComponent.text = "Avancer vers la droite (tout seul)";
        } else if (textComponent.name.EndsWith("2") && gameManager.gameData.playerUnlockedFunctions[1]) {
            textComponent.text = "Avancer vers la droite (avec une touche)";
        } else if (textComponent.name.EndsWith("3") && gameManager.gameData.playerUnlockedFunctions[2]) {
            textComponent.text = "Avancer vers la gauche";
        } else if (textComponent.name.EndsWith("4") && gameManager.gameData.playerUnlockedFunctions[3]) {
            textComponent.text = "Saut";
        } else if (textComponent.name.EndsWith("5") && gameManager.gameData.playerUnlockedFunctions[4]) {
            textComponent.text = "Tuer ennemi";
        } else if (textComponent.name.EndsWith("6") && gameManager.gameData.playerUnlockedFunctions[5]) {
            textComponent.text = "Reset";
        } 
    }
}
