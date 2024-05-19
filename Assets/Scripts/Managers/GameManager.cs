using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private SaveSystem saveSystem;
    private GameData gameData;


    void Start()
    {
        // Initialisation saveSystem et player
        saveSystem = gameObject.AddComponent<SaveSystem>();

        if (saveSystem == null)
        {
            Debug.LogError("SaveSystem is not found in the scene.");
            return;
        }

        gameData = saveSystem.LoadGame();

        if (gameData == null) {
            gameData = new GameData();
        } else {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            SceneManager.LoadScene(gameData.playerLevel);
            rb.MovePosition(gameData.playerPosition);

        }
        
    }

    public void SaveGame()
    {
        saveSystem.SaveGame(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
