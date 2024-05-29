using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptWindow : MonoBehaviour
{
    
    public GameObject scriptMenu;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Semicolon)) // ca correspond a la touche M mais ca prend le clavier QWERTY donc c'est semicolon (;)
        {
            scriptMenu.SetActive(!scriptMenu.activeSelf);
        }
    }
}





