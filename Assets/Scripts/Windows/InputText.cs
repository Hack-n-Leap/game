using UnityEngine;
using TMPro;
using InterpreterLib;

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
            Interpreter interpreter = new Interpreter();

            interpreter.EvaluateCode(inputField.text);

            
        }
    }
}