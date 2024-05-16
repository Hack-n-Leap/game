using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    public GameObject player;
    public GameObject spawn;
    private bool kill;

    private void Update()
    {
        if (kill)
        {
            PlayerSpawn();
            kill=!kill;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        kill = collision.gameObject.CompareTag("Player");
    }
    private void PlayerSpawn()
    {
        player.transform.position = spawn.transform.position;
    }
}
