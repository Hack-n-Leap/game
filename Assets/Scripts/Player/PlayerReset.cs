using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerReset : MonoBehaviour
{
    public string sceneToLoad;
    private bool next;
    private GameManager gameManager;
    private GameData gameData;

    private void Start() {
        gameManager = GetComponent<GameManager>();
        gameData = gameManager.gameData;
    }
    private void Update()
    {
        if (Input.GetKey(gameData.playerFunctionsKey[5]) && gameData.playerUnlockedFunctions[5])
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
    
    
}
