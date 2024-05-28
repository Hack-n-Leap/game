using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyBind : MonoBehaviour
{
    private GameObject playerObject;
    private GameManager gameManager;
    private TMP_Text buttonText;
    private bool isBinding;
    public int functionIndex;

    private void Start()
    {
        playerObject = GameObject.FindWithTag("Player");
        gameManager = playerObject.GetComponent<GameManager>();
        buttonText = gameObject.GetComponentInChildren<TMP_Text>();
    }

    private void Update() {
        if (Input.anyKey) {
            if (isBinding) {
                isBinding = false;
                foreach (KeyCode kc in System.Enum.GetValues(typeof(KeyCode))) {
                    if (Input.GetKey(kc)) {
                        gameManager.gameData.playerFunctionsKey[0] = kc;
                        buttonText.text = kc.ToString();
                        break;
                    }
                }
            }

        }
    }

    public void KeyBinding() {
        isBinding = !isBinding;
    }

}
