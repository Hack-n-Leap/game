using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerReset : MonoBehaviour
{
    public string sceneToLoad;
    private bool next;
    private GameManager gameManager;

    private void Start() {
        gameManager = GetComponent<GameManager>();
    }
    private void Update()
    {
        if ((Input.GetKey(gameManager.gameData.playerFunctionsKey[5]) && SceneManager.GetActiveScene().name != "Level7") || gameManager.gameData.playerUnlockedFunctions[5] && Input.GetKey(gameManager.gameData.playerFunctionsKey[5]))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
    
    
}
