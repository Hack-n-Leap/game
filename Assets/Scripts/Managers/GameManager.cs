using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private SaveSystem saveSystem;
    private Rigidbody2D rb;
    public GameData gameData;


    void Start()
    {
        // Initialisation saveSystem et player
        saveSystem = gameObject.AddComponent<SaveSystem>();

        if (saveSystem == null)
        {
            Debug.LogError("SaveSystem is not found in the scene.");
            return;
        }

        gameData = saveSystem.LoadGame(); // Chargement de la sauvegarde

        if (gameData == null) { // Cas où aucune sauvegarde n'existe
            gameData = new GameData();
        } else { 
            rb = GetComponent<Rigidbody2D>();

            // SceneManager.LoadScene(gameData.playerLevel); // Mise à jour de la scène
            rb.MovePosition(gameData.playerPosition); // Mise à jour de la position du joueur
        }
    }

    void Update() {
        if (rb != null) {
            gameData.playerPosition = rb.position;
        }
    }

    public void UpdateScene(string name) {
        gameData.playerLevel = name;
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
