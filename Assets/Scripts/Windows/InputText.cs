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

            Interpreter interpreter = new Interpreter();

            if (currentScene == "Level1") {
                code += "\n\navancer(2)\navancer(3)";
                interpreter.EvaluateCode(code);

                if (interpreter.FunctionsExecutionList[0].Variables["position"].Value == "3" && interpreter.FunctionsExecutionList[1].Variables["position"].Value == "4") {
                    Debug.Log(1);
                }

            }

        }
    }
}