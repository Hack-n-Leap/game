using UnityEngine;

public class KillMob : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject player = GameObject.FindWithTag("Player");;
        GameManager gameManager = player.GetComponent<GameManager>();

        if (collision.CompareTag("Player") && gameManager.gameData.playerUnlockedFunctions[4])
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
