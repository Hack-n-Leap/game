using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private SaveSystem saveSystem;
    private Rigidbody2D rb;
    public GameData gameData;
    private static bool hasLoadedGameData = false;


    void Start()
    {
        saveSystem = gameObject.AddComponent<SaveSystem>();
        rb = GetComponent<Rigidbody2D>();

        if (!hasLoadedGameData) {
            hasLoadedGameData = true;

            if (saveSystem == null)
            {
                Debug.LogError("SaveSystem is not found in the scene.");
                return;
            }

            gameData = saveSystem.LoadGame(); // Chargement de la sauvegarde

            if (gameData == null) { // Cas où aucune sauvegarde n'existe
                gameData = new GameData();
            } else { 
                SceneManager.LoadScene(gameData.playerLevel); // Mise à jour de la scène
                rb.MovePosition(gameData.playerPosition); // Mise à jour de la position du joueur
            }
        }
    }

    void Update() {
        gameData.playerPosition = rb.position;
        gameData.playerLevel = SceneManager.GetActiveScene().name;
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
