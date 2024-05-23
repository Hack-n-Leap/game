using UnityEngine;
using TMPro;
using InterpreterLib;
using UnityEngine.SceneManagement;

public class InputText : MonoBehaviour
{
    private TMP_InputField inputField;

    void Start() // On récupère l'objet qui contient le texte au lancement du jeu pour le manipuler par la suite
    {
        GameObject inputFieldObject = GameObject.FindGameObjectWithTag("texteCode");
        inputField = inputFieldObject.GetComponent<TMP_InputField>();
    }

    public void RetrieveText() { // Cette fonction est appelée à chaque fois que le bouton est pressé.
        if (inputField != null) {
            string code = inputField.text;
            string currentScene = SceneManager.GetActiveScene().name;

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            GameManager gameManager = player.GetComponent<GameManager>();

            Interpreter interpreter = new Interpreter();

            if (currentScene == "Level1") {
                code += "\n\navancer(2)\navancer(3)";
                interpreter.EvaluateCode(code);

                if (interpreter.FunctionsExecutionList[0].Variables["x"].Value == "3" && interpreter.FunctionsExecutionList[1].Variables["x"].Value == "4") {
                    gameManager.gameData.playerUnlockedFunctions[0] = true;
                    gameManager.SaveGame();
                }

            } else if (currentScene == "Level2") {
                code += "\n\navancer_droite(5, \"D\")\navancer_droite(5, \"G\")";

                interpreter.EvaluateCode(code);

                if (interpreter.FunctionsExecutionList[0].Variables["x"].Value == "6" && interpreter.FunctionsExecutionList[1].Variables["x"].Value == "5") {
                    gameManager.gameData.playerUnlockedFunctions[1] = true;
                    gameManager.SaveGame();
                }
            } else if (currentScene == "Level3") {
                code += "\n\navancer_gauche(5, \"Q\")\navancer_gauche(3, \"D\")";

                interpreter.EvaluateCode(code);

                if (interpreter.FunctionsExecutionList[0].Variables["x"].Value == "4" && interpreter.FunctionsExecutionList[1].Variables["x"].Value == "3") {
                    gameManager.gameData.playerUnlockedFunctions[2] = true;
                    gameManager.SaveGame();
                }
            } else if (currentScene == "Level4") {
                code += "\n\nsaut(0, \"space\")\nsaut(0, \"Q\")";

                interpreter.EvaluateCode(code);

                if (interpreter.FunctionsExecutionList[0].Variables["y"].Value == "5" && interpreter.FunctionsExecutionList[1].Variables["y"].Value == "0") {
                    gameManager.gameData.playerUnlockedFunctions[3] = true;
                    gameManager.SaveGame();
                }
            }

        }
    }
}