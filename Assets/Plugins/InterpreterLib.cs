using System;
using System.Globalization;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InterpreterLib
{
    public class Type
    {
        public static readonly int BOOLEAN = 0;
        public static readonly int INTEGER = 1;
        public static readonly int FLOAT = 2;
        public static readonly int STRING = 3;
        public static readonly int OPERATION_NUMBER = 4;
        public static readonly int OPERATION_BOOL = 5;
        public static readonly int VARIABLE = 6;
        public static readonly int FUNCTION = 7;

    }

    public class Variable
    {
        public string Name { get; }
        public string Value { get; }
        public int Type { get; }

        public Variable(string name, string value, int type)
        {
            Name = name; Value = value; Type = type;
        }

        public override string ToString()
        {
            return $"{Value} ({Type})";
        }
    }


    public class Function
    {
        public string Name { get; }
        public string Code { get; }
        public string[] Var { get; }

        public Function(string name, string code, string[] var)
        {
            Name = name; Code = code; Var = var;
        }

        public Interpreter Execute(string[] parameters, Interpreter interpreter)
        {
            StringBuilder functionCode = new StringBuilder(Code); // Use of String Builder because the program need to replace a string by another.

            if (parameters.Length < Var.Length) { throw new Exception("No enought parameters was given."); }
            if (parameters.Length > Var.Length) { throw new Exception("Too much parameters was given."); }
            if (Var.Length == 1 && Var[0] == "") { parameters = new string[0]; } // Case of a function without parameters

            Interpreter functionInterpreter = new Interpreter(); // Create a new interpreter to execute the function code.

            for (int i = 0; i < parameters.Length; i++) // Create local variables with the parameters given in parameters.
            {
                int parameterType = interpreter.EvaluateType(parameters[i]); 
                
                if (parameterType == Type.VARIABLE) { parameterType = interpreter.Variables[parameters[i]].Type; parameters[i] = interpreter.Variables[parameters[i]].Value; } // Replace variable name with value and type with the value type
                if (parameterType == Type.STRING) { parameters[i] = parameters[i][1..(parameters[i].Length - 1)]; }

                functionInterpreter.Variables[Var[i]] = new Variable(Var[i], parameters[i], parameterType); 
            }

            foreach (string functionName in interpreter.Functions.Keys) // Add all functions created in the main interpreter to the function interpreter
            {
                functionInterpreter.Functions[functionName] = interpreter.Functions[functionName];
            }

            functionInterpreter.EvaluateCode(functionCode.ToString()); 

            return functionInterpreter;
        }

    }

    public class Interpreter
    {
        public Dictionary<string, Variable> Variables;
        public Dictionary<string, Function> Functions;
        public List<Interpreter> FunctionsExecutionList;

        public Interpreter()
        {
            Variables = new Dictionary<string, Variable>();
            Functions = new Dictionary<string, Function>();
            FunctionsExecutionList = new List<Interpreter>();
        }

        public void EvaluateCode(string code)
        {
            string[] lines = code.Split(new[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            int index = 0;

            while (index < lines.Length)
            {
                string line = lines[index];
                string trimmedLine = line.Trim();

                if (trimmedLine.StartsWith("var ")) // Case of the registration / assignation of a value to a variable
                {
                    EvaluateVariable(trimmedLine[4..]);
                }
                else if (trimmedLine.StartsWith("print ")) // Case of a print
                {
                    EvaluatePrint(trimmedLine[6..]);
                }
                else if (trimmedLine.StartsWith("func "))
                { // Case of the registration of a function
                    StringBuilder functionText = new StringBuilder(trimmedLine[5..]);

                    index++;

                    while (index < lines.Length && lines[index].StartsWith("\t"))
                    { // Get all lines until their are no tabulation and the line don't finish by the '}' character.
                        functionText.Append($"\n{lines[index][1..]}");
                        index++;
                    }

                    EvaluateFunctionRegister(functionText.ToString());

                    index--;

                } else if (trimmedLine.StartsWith("for ")) // for var from x to y {}
                { // Case of the registration of a loop
                    string loopFirstLine = trimmedLine[4..];
                    StringBuilder loopCode = new StringBuilder();

                    //if (!(firstLine.EndsWith('{'))) { throw new Exception("Error. Invalid Syntax."); } // Throw new error in case of invalid syntax.
                    string loopVarName = loopFirstLine.Split(' ')[0]; // Get the title of the loopVar
                    string loopFromName = loopFirstLine.Split(' ')[2]; // Get the title of the loopFrom
                    string loopToName = loopFirstLine.Split(' ')[4]; // Get the title of the loopTo
                    loopToName = loopToName[..(loopToName.Length - 1)];

                    if (!loopFirstLine.EndsWith(':')) { throw new Exception("Invalid syntax."); }

                    int loopFromType = EvaluateType(loopFromName);
                    int loopToType = EvaluateType(loopToName);

                    if (loopFromType == Type.VARIABLE)
                    {
                        loopFromType = Variables[loopFromName].Type;
                        loopFromName = Variables[loopFromName].Value;
                    }
                    if (loopToType == Type.VARIABLE)
                    {
                        loopToType = Variables[loopToName].Type;
                        loopToName = Variables[loopToName].Value;
                    }
                    if (loopFromType != Type.INTEGER || loopToType != Type.INTEGER)
                    {
                        throw new Exception($"Error. {trimmedLine} is not recognized as a correct expression. ");
                    }

                    Variables[loopVarName] = new Variable(loopVarName, loopFromName, Type.INTEGER);

                    index++;

                    while (index < lines.Length && lines[index].StartsWith("\t")) //
                    { // Get all lines until their are no tabulation and the line don't finish by the '}' character.
                        loopCode.Append($"\n{lines[index][1..]}");
                        index++;
                    }

                    int from = int.Parse(loopFromName);
                    int to = int.Parse(loopToName);

                    // Console.WriteLine(loopText.ToString());

                    for (int i = from;  i < to + 1;  i++)
                    {
                        Variables[loopVarName] = new Variable(loopVarName, i.ToString(), Type.INTEGER);
                        this.EvaluateCode(loopCode.ToString());
                    }

                    index--;

                } 
                else if (trimmedLine.StartsWith("if "))
                {
                    string loopFirstLine = trimmedLine[3..];
                    StringBuilder ifCode = new StringBuilder();

                    //if (!(firstLine.EndsWith('{'))) { throw new Exception("Error. Invalid Syntax."); } // Throw new error in case of invalid syntax.
                    string condition = loopFirstLine.Split(':')[0];

                    index++;

                    while (index < lines.Length && lines[index].StartsWith("\t")) //
                    { // Get all lines until their are no tabulation and the line don't finish by the '}' character.
                        ifCode.Append($"\n{lines[index][1..]}");
                        index++;
                    }

                    if (EvaluateBooleanOperations(condition))
                    {
                        EvaluateCode(ifCode.ToString());
                    }

                    index--;
                }
                
                else if (EvaluateType(trimmedLine) == Type.FUNCTION)
                {
                    EvaluateFunctionCall(trimmedLine);
                }
                else
                {
                    throw new Exception($"Error. {trimmedLine} is not recognized as a correct expression. ");
                }

                index++;
            }

        }

        public void EvaluateVariable(string line)
        {
            if (!line.Contains(" = ")) { throw new Exception("Unable to create a variable without value assignation."); }

            string[] var = line.Split(" = ");

            int varType = EvaluateType(var[1]);

            if (varType == Type.STRING)
            {
                var[1] = var[1][1..(var[1].Length - 1)];
            }
            else if (varType == Type.VARIABLE)
            {
                varType = Variables[var[1]].Type;
                var[1] = Variables[var[1]].Value;
            } else if (varType == Type.OPERATION_NUMBER)
            {
                var[1] = EvaluateOperations(var[1]).ToString();
                varType = EvaluateType(var[1]);
            } else if (varType == Type.OPERATION_BOOL)
            {
                var[1] = EvaluateBooleanOperations(var[1]).ToString();
                varType = Type.BOOLEAN;
            }

            Variables[var[0]] = new Variable(var[0], var[1], varType);
        }

        public void EvaluateFunctionRegister(string lines)
        {
            string firstLine = lines.Split("\n")[0]; // Store the first line to use it to get the function name and args
            string functionTitle = firstLine.Split('(')[0]; // Get the title of the function.

            int openBracketIndex = firstLine.IndexOf("(");
            int closeBracketIndex = firstLine.IndexOf(")");

            if (openBracketIndex ==  -1 || closeBracketIndex == -1 || !(firstLine.EndsWith(':')) || functionTitle.Length == 0) { throw new Exception("Error. Invalid Syntax."); } // Throw new error in case of invalid syntax.

            Function function = new Function(functionTitle, lines[(firstLine.Length + 1)..], firstLine[(openBracketIndex + 1)..closeBracketIndex].Split(", ")); // Create a new Function objet that store all the informations about the new function.
            Functions[function.Name] = function; // Register the created function into the program function dictionnary.

        }

        public void EvaluateFunctionCall(string line) 
        { 
            string functionName = line.Split('(')[0]; // Get the title of the function

            int openBracketIndex = line.IndexOf('(');
            int closeBracketIndex = line.IndexOf(')');

            FunctionsExecutionList.Add(Functions[functionName].Execute(line[(openBracketIndex + 1)..closeBracketIndex].Split(", "), this)); // Execute the function with the parameters gives in the function call.
        }

        public void EvaluatePrint(string line)
        {
            int lineType = EvaluateType(line);

            if (lineType == Type.VARIABLE)
            {
                Debug.Log(Variables[line].Value);
            }
            else if (lineType == Type.OPERATION_NUMBER)
            {
                Debug.Log(EvaluateOperations(line));
            } else if (lineType == Type.OPERATION_BOOL)
            {
                Debug.Log(EvaluateBooleanOperations(line));
            }
            else if (lineType == Type.STRING)
            {
                Debug.Log(line[1..(line.Length - 1)]);
            }
            else
            {
                Debug.Log(line);
            }
        }

        public double EvaluateOperations(string line)
        // This function manages the system of operations and redirects each operation to the corresponding operations function.
        // It take a calculation in string type as a parameter and return the result of it as a double.
        {
            int openBracketIndex = 0;
            int closeBracketIndex = 0;
            int expressionIndex = 0;
            string newExpression;

            StringBuilder expressionBuilder = new StringBuilder(line);

            if (line.StartsWith('(') && line.EndsWith(')')) { expressionBuilder = new StringBuilder(line[1..(line.Length - 1)]); } // Delete the bracket at the start and the end of the calculus.

            while (expressionIndex < expressionBuilder.Length)
            {
                if (expressionBuilder.ToString()[expressionIndex] == '(') { openBracketIndex = expressionIndex; }
                else if (expressionBuilder.ToString()[expressionIndex] == ')')
                {
                    closeBracketIndex = expressionIndex;
                    expressionBuilder.Replace(expressionBuilder.ToString()[(openBracketIndex)..(closeBracketIndex + 1)], EvaluateOperations(expressionBuilder.ToString()[(openBracketIndex + 1)..(closeBracketIndex)]).ToString());

                    expressionIndex = 0;
                }

                expressionIndex++;
            }

            newExpression = expressionBuilder.ToString();

            if (newExpression.Contains('+')) // Case of an addition
            {
                return EvaluateAddition(newExpression);

            }
            else if (newExpression.Contains('-')) // Case of a substraction
            {
                return EvaluateSubstraction(newExpression);
            }
            else if (newExpression.Contains('*')) // Case of a multiplication
            {
                return EvaluateMultiplication(newExpression);
            }
            else if (newExpression.Contains('/')) // Case of a division
            {
                return EvaluateDivision(newExpression);
            }
            else if (newExpression.Contains('^')) // Case of a power
            {
                return EvaluatePower(newExpression);
            }
            else if (double.TryParse(newExpression, out double _))
            {
                return double.Parse(newExpression);
            }
            else
            {
                throw new Exception($"Unexcepted operation : ${newExpression}");
            }
        }

        public double EvaluateAddition(string line)
        {
            string[] parts = line.Split(" + ");
            double sum = 0;

            foreach (string part in parts)
            {
                int partType = EvaluateType(part);
                if (partType == Type.VARIABLE && Variables[part].Type != Type.STRING)
                {
                    sum += double.Parse(Variables[part].Value, CultureInfo.InvariantCulture);
                }
                else if (partType == Type.INTEGER || partType == Type.FLOAT || partType == Type.OPERATION_NUMBER)
                {
                    sum += double.Parse(part, CultureInfo.InvariantCulture);
                }
                else
                {
                    throw new Exception($"Unable to add {partType} to double !");
                }
            }

            return sum;
        }

        public double EvaluateSubstraction(string line)
        {
            string[] parts = line.Split(" - ")[1..];
            double sum;


            if (EvaluateType(line.Split(" - ")[0]) == Type.VARIABLE && Variables[line.Split(" - ")[0]].Type != Type.STRING)
            {
                sum = double.Parse(Variables[line.Split(" - ")[0]].Value);
            }
            else
            {
                sum = double.Parse(line.Split(" - ")[0]);
            }

            foreach (string part in parts)
            {
                int partType = EvaluateType(part);
                if (partType == Type.VARIABLE && Variables[part].Type != Type.STRING)
                {
                    sum -= double.Parse(Variables[part].Value, CultureInfo.InvariantCulture);
                }
                else if (partType == Type.INTEGER || partType == Type.FLOAT || partType == Type.OPERATION_NUMBER)
                {
                    sum -= double.Parse(part, CultureInfo.InvariantCulture);
                }
                else
                {
                    throw new Exception($"Unable to substract {partType} to double !");
                }
            }

            return sum;
        }

        public double EvaluateMultiplication(string line)
        {
            string[] parts = line.Split(" * ");
            double total = 1;

            foreach (string part in parts)
            {
                int partType = EvaluateType(part);

                if (partType == Type.VARIABLE && Variables[part].Type != Type.STRING)
                {
                    total *= double.Parse(Variables[part].Value, CultureInfo.InvariantCulture);
                }
                else if (partType == Type.INTEGER || partType == Type.FLOAT || partType == Type.OPERATION_NUMBER)
                {
                    total *= double.Parse(part, CultureInfo.InvariantCulture);
                }
                else
                {
                    throw new Exception($"Unable to multiply {partType} to double !");
                }
            }

            return total;
        }

        public double EvaluateDivision(string line)
        {
            string[] parts = line.Split(" / ")[1..];
            double total;

            if (parts.Contains("0")) { throw new Exception("Error. Unable to divide by zero."); }

            if (EvaluateType(line.Split(" / ")[0]) == Type.VARIABLE && Variables[line.Split(" / ")[0]].Type != Type.STRING)
            {
                total = double.Parse(Variables[line.Split(" / ")[0]].Value);
            }
            else
            {
                total = double.Parse(line.Split(" / ")[0]);
            }

            foreach (string part in parts)
            {
                int partType = EvaluateType(part);
                if (partType == Type.VARIABLE && Variables[part].Type != Type.STRING)
                {
                    total /= double.Parse(Variables[part].Value, CultureInfo.InvariantCulture);
                }
                else if (partType == Type.INTEGER || partType == Type.FLOAT || partType == Type.OPERATION_NUMBER)
                {
                    total /= double.Parse(part, CultureInfo.InvariantCulture);
                }
                else
                {
                    throw new Exception($"Unable to divide {partType} to double !");
                }
            }

            return total;
        }

        public double EvaluatePower(string line)
        {
            string[] parts = line.Split(" ^ ")[1..];
            double pow;


            if (EvaluateType(line.Split(" ^ ")[0]) == Type.VARIABLE && Variables[line.Split(" ^ ")[0]].Type != Type.STRING)
            {
                pow = double.Parse(Variables[line.Split(" ^ ")[0]].Value);
            }
            else if (EvaluateType(line.Split(" ^ ")[0]) != Type.STRING)
            {
                pow = double.Parse(line.Split(" ^ ")[0]);
                
            } else
            {
                throw new Exception("Unable to calculate a string power.");
            }

            foreach (string part in parts)
            {
                int partType = EvaluateType(part);

                if (partType == Type.VARIABLE && Variables[part].Type != Type.STRING)
                {
                    pow = Math.Pow(pow, double.Parse(Variables[part].Value, CultureInfo.InvariantCulture));
                }
                else if (partType == Type.INTEGER || partType == Type.FLOAT || partType == Type.OPERATION_NUMBER)
                {
                    pow = Math.Pow(pow, double.Parse(part, CultureInfo.InvariantCulture));
                }
                else
                {
                    throw new Exception($"Unable to power {partType} to double !");
                }
            }

            return pow;
        }

        public bool EvaluateBooleanOperations(string line)
        // This function manages the system of boolean operations and redirects each operation to the corresponding operations function.
        // It take a calculation in string type as a parameter and return the result of it as a bool.
        {
            int openBracketIndex = 0;
            int closeBracketIndex = 0;
            int expressionIndex = 0;
            string newExpression;

            StringBuilder expressionBuilder = new StringBuilder(line);

            if (line.StartsWith('(') && line.EndsWith(')') && AreBracketsBalanced(line)) { expressionBuilder = new StringBuilder(line[1..(line.Length - 1)]); } // Delete the bracket at the start and the end of the calculus.

            while (expressionIndex < expressionBuilder.Length)
            {
                if (expressionBuilder.ToString()[expressionIndex] == '(') { openBracketIndex = expressionIndex; }
                else if (expressionBuilder.ToString()[expressionIndex] == ')')
                {
                    closeBracketIndex = expressionIndex;

                    string bracketExpression = expressionBuilder.ToString()[(openBracketIndex)..(closeBracketIndex)];

                    expressionBuilder.Replace(expressionBuilder.ToString()[(openBracketIndex)..(closeBracketIndex + 1)], EvaluateBooleanOperations(expressionBuilder.ToString()[(openBracketIndex + 1)..(closeBracketIndex)]).ToString());

                    expressionIndex = 0;
                }

                expressionIndex++;
            }

            newExpression = expressionBuilder.ToString();

            if (newExpression.Contains("==")) // Case of an egual test
            {
                return EvaluateEgual(newExpression);
            }
            else if (newExpression.Contains("!=")) // Case of an unegual test
            {
                return EvaluateUnegual(newExpression);
            } 
            else if (newExpression.Contains(">=")) // Case of a superior or egual test
            {
                return EvaluateSuperiorEgual(newExpression);
            }
            else if (newExpression.Contains("<=")) // Case of an inferior or egual test
            {
                return EvaluateInferiorEgual(newExpression);
            }
            else if (newExpression.Contains('>')) // Case of a superior test
            {
                return EvaluateSuperior(newExpression);
            }
            else if (newExpression.Contains('<')) // Case of an inferior test
            {
                return EvaluateInferior(newExpression);
            } else if (newExpression.Contains('&'))
            {
                return EvaluateAnd(newExpression);
            } else if (newExpression.Contains('|'))
            {
                return EvaluateOr(newExpression);
            }
            else if (this.EvaluateType(newExpression) == Type.BOOLEAN)
            {
                return bool.Parse(newExpression);
            }
            else if (EvaluateType(newExpression) == Type.VARIABLE && Variables[newExpression].Type == Type.BOOLEAN) {
                return bool.Parse(Variables[newExpression].Value);
            }
            else
            {
                throw new Exception($"Unexcepted operation : ${newExpression}");
            }
        }

        public bool AreBracketsBalanced(string expression)
        {
            int balance = 0;
            foreach (char c in expression)
            {
                if (c == '(') { balance++; }
                else if (c == ')') { balance--; }
                if (balance == 0) { return false; }
            }

            return balance == 0;
        }

        public bool EvaluateAnd(string line)
        {
            string[] parts = line.Split("&");

            if (parts.Length != 2) { throw new Exception("Error. Invalid expression."); }

            string firstPart = parts[0][..(parts[0].Length - 1)]; // Remove space at the end of the string
            string secondPart = parts[1][1..]; // Remove space at the begginning of the string

            int firstPartType = EvaluateType(firstPart);
            int secondPartType = EvaluateType(secondPart);

            if (firstPartType == Type.VARIABLE)
            {
                firstPartType = Variables[firstPart].Type;
                firstPart = Variables[firstPart].Value;
            }

            if (secondPartType == Type.VARIABLE)
            {
                secondPartType = Variables[secondPart].Type;
                secondPart = Variables[secondPart].Value;
            }

            if (firstPartType != Type.BOOLEAN || secondPartType != Type.BOOLEAN) { throw new Exception("Error. Unable to calcul the 'AND' operator between types different that BOOLEAN"); }

            return bool.Parse(firstPart) && bool.Parse(secondPart);
        }

        public bool EvaluateOr(string line)
        {
            string[] parts = line.Split("|");

            if (parts.Length != 2) { throw new Exception("Error. Invalid expression."); }

            string firstPart = parts[0][..(parts[0].Length - 1)]; // Remove space at the end of the string
            string secondPart = parts[1][1..]; // Remove space at the begginning of the string

            int firstPartType = EvaluateType(firstPart);
            int secondPartType = EvaluateType(secondPart);

            if (firstPartType == Type.VARIABLE)
            {
                firstPartType = Variables[firstPart].Type;
                firstPart = Variables[firstPart].Value;
            }

            if (secondPartType == Type.VARIABLE)
            {
                secondPartType = Variables[secondPart].Type;
                secondPart = Variables[secondPart].Value;
            }

            if (firstPartType != Type.BOOLEAN || secondPartType != Type.BOOLEAN) { throw new Exception("Error. Unable to calcul the 'AND' operator between types different that BOOLEAN"); }

            return bool.Parse(firstPart) || bool.Parse(secondPart);
        }

        public bool EvaluateEgual(string line)
        {
            string[] part = line.Trim().Split(" == "); // The trim need to be remove once we implet this method in the EvaluateOperation function

            if (part.Length != 2) { throw new Exception("Invalid expression."); } // Verify that there are exactly 2 values to be compared

            // Extract the 2 values
            string firstPart = part[0];
            string secondPart = part[1];

            // Get the types of the 2 parts
            int firstElementType = this.EvaluateType(firstPart);
            int secondElementType = this.EvaluateType(secondPart);

            if (firstElementType == Type.STRING) {
                firstPart = firstPart[1..(firstPart.Length - 1)];
            }
            if (secondElementType == Type.STRING) {
                secondPart = secondPart[1..(secondPart.Length - 1)];
            }

            if (firstElementType == Type.VARIABLE) // Replace the variable type by the type of his value and the content by the value of the variable
            {
                firstElementType = Variables[firstPart].Type;
                firstPart = Variables[firstPart].Value;
            }
            if (secondElementType == Type.VARIABLE) // Replace the variable type by the type of his value and the content by the value of the variable
            {
                secondElementType = Variables[firstPart].Type;
                secondPart = Variables[firstPart].Value;
            }

            if (firstElementType == Type.OPERATION_NUMBER)
            {
                firstPart = EvaluateOperations(firstPart).ToString();
                firstElementType = EvaluateType(firstPart);
            }
            if (secondElementType == Type.OPERATION_NUMBER)
            {
                secondPart = EvaluateOperations(secondPart).ToString();
                secondElementType = EvaluateType(secondPart);
            }

            return (firstElementType == secondElementType) && (firstPart == secondPart); // Verify that types and value are egual
        }

        public bool EvaluateUnegual(string line)
        {
            return !(EvaluateEgual(line.Replace("!=", "=="))); // Return the opposite of the == operator
        }

        public bool EvaluateSuperior(string line)
        {
            string[] part = line.Trim().Split(" > "); // The trim need to be remove once we implet this method in the EvaluateOperation function

            if (part.Length != 2) { throw new Exception("Invalid expression."); } // Verify that there are exactly 2 values to be compared

            // Extract the 2 values
            string firstPart = part[0];
            string secondPart = part[1];

            // Get the types of the 2 parts
            int firstElementType = this.EvaluateType(firstPart);
            int secondElementType = this.EvaluateType(secondPart);

            if (firstElementType == Type.VARIABLE) // Replace the variable type by the type of his value and the content by the value of the variable
            {
                firstElementType = Variables[firstPart].Type;
                firstPart = Variables[firstPart].Value;
            }
            if (secondElementType == Type.VARIABLE) // Replace the variable type by the type of his value and the content by the value of the variable
            {
                secondElementType = Variables[firstPart].Type;
                secondPart = Variables[firstPart].Value;
            }

            if (firstElementType == Type.OPERATION_NUMBER)
            {
                firstPart = EvaluateOperations(firstPart).ToString();
                firstElementType = EvaluateType(firstPart);
            }
            if (secondElementType == Type.OPERATION_NUMBER)
            {
                secondPart = EvaluateOperations(secondPart).ToString();
                secondElementType = EvaluateType(secondPart);
            }

            if ((firstElementType != Type.INTEGER && firstElementType != Type.FLOAT) && (secondElementType != Type.INTEGER && secondElementType != Type.FLOAT)) { throw new Exception("Comparaison on unsuportable type"); }

            return (firstElementType == secondElementType) && (int.Parse(firstPart) > int.Parse(secondPart)); // Verify that types and value are egual
        }

        public bool EvaluateInferior(string line)
        {
            if (EvaluateEgual(line.Replace("<", "=="))) { return false; }

            return !(EvaluateSuperior(line.Replace("<", ">"))); // Return the opposite of the > operator
        }

        public bool EvaluateSuperiorEgual(string line)
        {
            return EvaluateSuperior(line.Replace(">=", ">")) || EvaluateEgual(line.Replace(">=", "=="));
        }

        public bool EvaluateInferiorEgual(string line)
        {
            return EvaluateInferior(line.Replace("<=", "<")) || EvaluateEgual(line.Replace("<=", "=="));
        }

        public int EvaluateType(string value)
        {
            if (value == "True" || value == "False")
            {
                return Type.BOOLEAN;
            } else if ((value.StartsWith("'") && value.EndsWith("'")) || (value.StartsWith('"') && value.EndsWith('"')))
            {
                return Type.STRING; // String
            }
            else if (int.TryParse(value, out _))
            {
                return Type.INTEGER; // Integer
            }
            else if (double.TryParse(value, out _))
            {
                return Type.FLOAT; // Float
            }
            else if (Variables.ContainsKey(value))
            {
                return Type.VARIABLE; // Variable
            }
            else if (value.Contains('&') || value.Contains('|') || value.Contains("==") || value.Contains('>') || value.Contains('<') || value.Contains(">=") || value.Contains("<="))
            {
                return Type.OPERATION_BOOL;
            }
            else if (value.Contains('+') || value.Contains('*') || value.Contains('/') || value.Contains('-') || value.Contains('^'))
            {
                return Type.OPERATION_NUMBER; // Operation
            } 
            else if (Functions.ContainsKey(value.Split('(')[0]))
            {
                return Type.FUNCTION; // Function
            }
            else
            {
                throw new Exception($"Error. Unable to get the type of {value}");
            }
        }
    }

}
