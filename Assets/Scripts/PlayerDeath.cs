using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    public string sceneToReload;

    private bool kill;

    private void Update()
    {
        if (kill)
        {
            SceneManager.LoadScene(sceneToReload);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        kill = collision.gameObject.CompareTag("Ennemi");
    }
}
