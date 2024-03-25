using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputText : MonoBehaviour
{
    private string input;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReadInput(string inp)
    {
        input = inp;
        Debug.Log(input);
    }
}
