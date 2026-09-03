using System;
using System.Collections.Generic;

public class OptimizedNFA
{
    // check if the string matches the 5-state nfa
    public static bool IsValidCStyleComment(string input)
    {
        if (string.IsNullOrEmpty(input)) 
            return false;

        // keep track of where we are. start at state 0
        var currentStates = new HashSet<int> { 0 };

        foreach (char c in input)
        {
            var nextStates = new HashSet<int>();

            foreach (int state in currentStates)
            {
                switch (state)
                {
                    case 0:
                        if (c == '/') nextStates.Add(1);
                        break;
                    
                    case 1:
                        if (c == '*') nextStates.Add(2);
                        break;
                    
                    case 2:
                        if (c == '*') nextStates.Add(3);
                        else nextStates.Add(2); // loop back to state 2 for 'a' or '/'
                        break;
                    
                    case 3:
                        if (c == '*') nextStates.Add(3);
                        else if (c == '/') nextStates.Add(4);
                        else nextStates.Add(2); // false alarm, back to the main comment body
                        break;
                    
                    case 4:
                        // accept state. no way out of here
                        // if we get more chars after closing, the path just dies
                        break;
                }
            }

            currentStates = nextStates;

            // kill it early if we run out of valid moves
            if (currentStates.Count == 0) 
                return false;
        }

        // valid only if we ended up in the accept state (4)
        return currentStates.Contains(4);
    }

    public static void Main()
    {
        // should pass these
        Console.WriteLine("--- Should Accept ---");
        Console.WriteLine(IsValidCStyleComment("/*a*/"));         // true
        Console.WriteLine(IsValidCStyleComment("/**/"));          // true
        Console.WriteLine(IsValidCStyleComment("/***/"));         // true
        Console.WriteLine(IsValidCStyleComment("/*aaa*aaa*/"));   // true
        Console.WriteLine(IsValidCStyleComment("/*a/a*/"));       // true

        // should fail these
        Console.WriteLine("\n--- Should Reject ---");
        Console.WriteLine(IsValidCStyleComment("/**"));           // false 
        Console.WriteLine(IsValidCStyleComment("*/a/*aa*/"));     // false 
        Console.WriteLine(IsValidCStyleComment("aaa/**/aa"));     // false 
        Console.WriteLine(IsValidCStyleComment("/*/"));           // false 
        Console.WriteLine(IsValidCStyleComment("/**a/"));         // false 
        Console.WriteLine(IsValidCStyleComment("//aaaa"));        // false 
        Console.WriteLine(IsValidCStyleComment("/*a*/aa*/"));     // false
    }
}