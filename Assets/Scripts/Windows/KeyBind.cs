using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBind : MonoBehaviour
{
    private GameObject playerObject;
    private GameManager gameManager;
    public int functionIndex;

    void Start()
    {
        playerObject = GameObject.FindWithTag("Player");
        gameManager = playerObject.GetComponent<GameManager>();
    }

    public void KeyBinding() {
        if (Input.anyKey) {
            foreach (KeyCode kc in System.Enum.GetValues(typeof(KeyCode))) {
                if (Input.GetKey(kc)) {
                    gameManager.gameData.playerFunctionsKey[0] = kc;
                    break;
                }
            }
        }
    }

}
