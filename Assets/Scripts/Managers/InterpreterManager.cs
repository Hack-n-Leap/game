using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PseudoCodeInterpreter;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

public class InterpreterManager {
    public int InterpreterID;
    
    public InterpreterManager(int interpreterID) {
        InterpreterID = interpreterID;
    }

    public void RunCode (string code) {
        if (InterpreterID == 0)  {
            PseudoCodeInterpreter.Interpreter pseudoCodeEngine = new PseudoCodeInterpreter.Interpreter();

            pseudoCodeEngine.EvaluateCode(code);
        } else if (InterpreterID == 1) {
            ScriptEngine pythonEngine = Python.CreateEngine();

            pythonEngine.Execute(code);
        } else {
            Debug.LogError("Error. Unknow Interpretrer ID.");
        }
    }
} 


// Suppression temporaire

// public class Interpreter {
//    public int InterpreterID;
// }
