using System;

class PrimMST
{
    static void Main()
    {
        const int n = 10;
        int[,] graph = new int[n, n];
        Console.WriteLine($"Enter the {n}x{n} distance matrix (rows separated by newline, values by space):");
        for (int i = 0; i < n; i++)
        {
            string line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line)) { i--; continue; }
            string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != n)
            {
                Console.WriteLine($"Invalid input on row {i}. Expected {n} values.");
                i--;
                continue;
            }
            for (int j = 0; j < n; j++)
            {
                if (!int.TryParse(parts[j], out graph[i, j]))
                {
                    Console.WriteLine($"Invalid number '{parts[j]}' at row {i}, column {j}. Try again.");
                    j--;
                }
            }
        }

        bool[] inMST = new bool[n];            // MST inclusion flag
        int[] parent = new int[n];             // Array to store constructed MST
        int[] key = new int[n];                // Key values used to pick minimum weight edge

        // Initialize keys as infinite
        for (int i = 0; i < n; i++)
        {
            key[i] = int.MaxValue;
            inMST[i] = false;
            parent[i] = -1;
        }

        // Start from the first vertex
        key[0] = 0;

        // Prim's algorithm
        for (int count = 0; count < n - 1; count++)
        {
            // Pick the minimum key vertex not yet included
            int u = MinKey(key, inMST);
            inMST[u] = true;

            // Update key and parent for adjacent vertices
            for (int v = 0; v < n; v++)
            {
                if (graph[u, v] != 0 && !inMST[v] && graph[u, v] < key[v])
                {
                    parent[v] = u;
                    key[v] = graph[u, v];
                }
            }
        }

        // Output the MST edges and total weight
        Console.WriteLine("Edge \tWeight");
        int totalWeight = 0;
        for (int i = 1; i < n; i++)
        {
            Console.WriteLine($"{parent[i]} - {i} \t{graph[i, parent[i]]}");
            totalWeight += graph[i, parent[i]];
        }
        Console.WriteLine($"Total weight of MST: {totalWeight}");
    }

    // Utility to find vertex with minimum key value
    static int MinKey(int[] key, bool[] inMST)
    {
        int min = int.MaxValue;
        int minIndex = -1;

        for (int v = 0; v < key.Length; v++)
        {
            if (!inMST[v] && key[v] < min)
            {
                min = key[v];
                minIndex = v;
            }
        }
        return minIndex;
    }
}
