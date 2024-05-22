using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public string sceneToLoad;
    private bool next;

    private void Update()
    {
        GameManager gameManager = GetComponent<GameManager>();

        if ((next && SceneManager.GetActiveScene().name != "Level8") || next && gameManager.gameData.playerUnlockedFunctions[6])
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        next = collision.gameObject.CompareTag("Player");
    }
    
}
