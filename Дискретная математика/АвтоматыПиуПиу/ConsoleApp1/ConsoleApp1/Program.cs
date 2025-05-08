using System;
using System.Collections.Generic;

class DFA
{
    private HashSet<char> alphabet = new HashSet<char> { 'a', 'b', 'c', 'd' };

    private Dictionary<int, Dictionary<char, int>> transitionTable = new Dictionary<int, Dictionary<char, int>>
    {
        { 0, new Dictionary<char, int> { { 'a', 1 }, { 'b', 0 }, { 'c', 0 }, { 'd', 0 } } },
        { 1, new Dictionary<char, int> { { 'a', 1 }, { 'b', 2 }, { 'c', 0 }, { 'd', 0 } } },
        { 2, new Dictionary<char, int> { { 'a', 1 }, { 'b', 0 }, { 'c', 3 }, { 'd', 0 } } },
        { 3, new Dictionary<char, int> { { 'a', 3 }, { 'b', 3 }, { 'c', 3 }, { 'd', 3 } } }
    };

    private HashSet<int> acceptingStates = new HashSet<int> { 0, 1, 2 };

    public bool IsValid(string word)
    {
        int currentState = 0;

        foreach (char ch in word)
        {
            if (!alphabet.Contains(ch))
                return false;

            currentState = transitionTable[currentState][ch];

            if (currentState == 3)
                break;
        }

        return acceptingStates.Contains(currentState);
    }
}

class Program
{
    static void Main()
    {
        DFA automaton = new DFA();
        Console.WriteLine("Enter word");

        while (true)
        {
            Console.Write("> ");
            string word = Console.ReadLine().Trim().ToLower();

            if (automaton.IsValid(word))
                Console.WriteLine("True");
            else
                Console.WriteLine("False");
        }
    }
}