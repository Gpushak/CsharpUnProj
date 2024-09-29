using System;
using System.Collections.Generic;

class SetCalculator
{
    static void Main(string[] args)
    {
        Dictionary<string, HashSet<int>> sets = new Dictionary<string, HashSet<int>>();
        string command;

        Console.WriteLine("Set Calculator. Type 'help' to see available commands.");

        do
        {
            Console.Write("\nEnter command: ");
            command = Console.ReadLine();
            string[] parts = command.Split(' ');

            switch (parts[0].ToLower())
            {
                case "create":
                    CreateSet(sets, parts);
                    break;
                case "union":
                    PerformOperation(sets, parts, Union);
                    break;
                case "intersection":
                    PerformOperation(sets, parts, Intersection);
                    break;
                case "difference":
                    PerformOperation(sets, parts, Difference);
                    break;
                case "symmetricdifference":
                    PerformOperation(sets, parts, SymmetricDifference);
                    break;
                case "print":
                    PrintSet(sets, parts);
                    break;
                case "help":
                    PrintHelp();
                    break;
                case "exit":
                    Console.WriteLine("Exiting...");
                    break;
                default:
                    Console.WriteLine("Unknown command. Type 'help' for the list of commands.");
                    break;
            }
        } while (command.ToLower() != "exit");
    }

    static void CreateSet(Dictionary<string, HashSet<int>> sets, string[] parts)
    {
        if (parts.Length < 3)
        {
            Console.WriteLine("Usage: create <setName> <elements>");
            return;
        }

        string setName = parts[1];
        if (sets.ContainsKey(setName))
        {
            Console.WriteLine($"Set {setName} already exists.");
            return;
        }

        string[] elements = parts[2].Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        HashSet<int> set = new HashSet<int>();

        foreach (var element in elements)
        {
            if (int.TryParse(element, out int number))
            {
                set.Add(number);
            }
            else
            {
                Console.WriteLine($"Invalid element: {element}");
            }
        }

        sets[setName] = set;
        Console.WriteLine($"Set {setName} created with elements: {{ {string.Join(", ", set)} }}");
    }

    static void PerformOperation(Dictionary<string, HashSet<int>> sets, string[] parts, Func<HashSet<int>, HashSet<int>, HashSet<int>> operation)
    {
        if (parts.Length != 4)
        {
            Console.WriteLine("Usage: <operation> <set1> <set2> <resultSet>");
            return;
        }

        string set1 = parts[1];
        string set2 = parts[2];
        string resultSet = parts[3];

        if (!sets.ContainsKey(set1) || !sets.ContainsKey(set2))
        {
            Console.WriteLine("One or both sets do not exist.");
            return;
        }

        HashSet<int> result = operation(sets[set1], sets[set2]);
        sets[resultSet] = result;
        Console.WriteLine($"Result set {resultSet} created.");
    }

    static HashSet<int> Union(HashSet<int> set1, HashSet<int> set2)
    {
        HashSet<int> result = new HashSet<int>(set1);

        
        foreach (int element in set2)
        {
            result.Add(element);
        }

        return result;
    }

    static HashSet<int> Intersection(HashSet<int> set1, HashSet<int> set2)
    {
        HashSet<int> result = new HashSet<int>();
       
        foreach (int element in set1)
        {
            if (set2.Contains(element))
            {
                result.Add(element);
            }
        }

        return result;
    }

    static HashSet<int> Difference(HashSet<int> set1, HashSet<int> set2)
    {
        HashSet<int> result = new HashSet<int>(set1);
       
        foreach (int element in set2)
        {
            result.Remove(element);
        }

        return result;
    }

    static HashSet<int> SymmetricDifference(HashSet<int> set1, HashSet<int> set2)
    {
        HashSet<int> result = new HashSet<int>();

        foreach (int element in set1)
        {
            if (!set2.Contains(element))
            {
                result.Add(element);
            }
        }

        foreach (int element in set2)
        {
            if (!set1.Contains(element))
            {
                result.Add(element);
            }
        }

        return result;
    }

    static void PrintSet(Dictionary<string, HashSet<int>> sets, string[] parts)
    {
        if (parts.Length != 2)
        {
            Console.WriteLine("Usage: print <setName>");
            return;
        }

        string setName = parts[1];
        if (!sets.ContainsKey(setName))
        {
            Console.WriteLine($"Set {setName} does not exist.");
            return;
        }

        Console.WriteLine($"{setName}: {{ {string.Join(", ", sets[setName])} }}");
    }

    static void PrintHelp()
    {
        Console.WriteLine("Available commands:");
        Console.WriteLine("create <setName> <elements> - Create a new set with the specified name and elements.");
        Console.WriteLine("union <set1> <set2> <resultSet> - Perform union of two sets and store in resultSet.");
        Console.WriteLine("intersection <set1> <set2> <resultSet> - Perform intersection of two sets and store in resultSet.");
        Console.WriteLine("difference <set1> <set2> <resultSet> - Perform difference of two sets and store in resultSet.");
        Console.WriteLine("symmetricdifference <set1> <set2> <resultSet> - Perform symmetric difference of two sets and store in resultSet.");
        Console.WriteLine("print <setName> - Print the contents of a set.");
        Console.WriteLine("exit - Exit the program.");
    }
}