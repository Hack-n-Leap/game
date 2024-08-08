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

    public void RunInterpreter (string code) {
        if (InterpreterID == 0)  {
            Interpreter interpreter = new Interpreter();

            interpreter.EvaluateCode(code);
        }
    }
}
