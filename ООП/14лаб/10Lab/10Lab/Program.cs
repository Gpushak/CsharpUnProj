using System;
using System.Collections.Generic;
using System.Linq;
using _10labLib;

public class DoublyLinkedList<T>
{
    public class Node
    {
        public T Value;
        public Node Next;
        public Node Prev;
        public Node(T value) { Value = value; }
    }
    public Node Head { get; private set; }
    public Node Tail { get; private set; }
    public Node First => Head;
    public Node Last => Tail;
    public void AddLast(T value)
    {
        var newNode = new Node(value);
        if (Head == null) Head = Tail = newNode;
        else { Tail.Next = newNode; newNode.Prev = Tail; Tail = newNode; }
    }
    public void InsertAfter(Node node, T value)
    {
        if (node == null) return;
        var newNode = new Node(value) { Next = node.Next, Prev = node };
        if (node.Next != null) node.Next.Prev = newNode;
        else Tail = newNode;
        node.Next = newNode;
    }
    public void Clear() { Head = Tail = null; }
}

// Методы расширения для DoublyLinkedList
public static class DoublyLinkedListExtensions
{
    public static IEnumerable<T> Where<T>(this DoublyLinkedList<T> list, Func<T, bool> predicate)
    {
        for (var node = list.First; node != null; node = node.Next)
            if (predicate(node.Value)) yield return node.Value;
    }
    public static int Sum<T>(this DoublyLinkedList<T> list, Func<T, int> selector)
    {
        int sum = 0;
        foreach (var val in list.Where(x => true)) sum += selector(val);
        return sum;
    }
    public static double Average<T>(this DoublyLinkedList<T> list, Func<T, int> selector)
    {
        var vals = list.Where(x => true).Select(selector).ToArray();
        return vals.Any() ? vals.Average() : 0;
    }
    public static int Min<T>(this DoublyLinkedList<T> list, Func<T, int> selector)
    {
        return list.Where(x => true).Select(selector).Min();
    }
    public static int Max<T>(this DoublyLinkedList<T> list, Func<T, int> selector)
    {
        return list.Where(x => true).Select(selector).Max();
    }
    public static IEnumerable<T> OrderBy<T, TKey>(this DoublyLinkedList<T> list, Func<T, TKey> keySelector)
        where TKey : IComparable
    {
        return list.Where(x => true).OrderBy(keySelector);
    }
    public static IEnumerable<T> OrderByDescending<T, TKey>(this DoublyLinkedList<T> list, Func<T, TKey> keySelector)
        where TKey : IComparable
    {
        return list.Where(x => true).OrderByDescending(keySelector);
    }
}

class Program
{
    static void Main()
    {
        var corp = new Dictionary<string, List<Production>>();
        corp["Factories"] = new List<Production>();
        corp["Gilds"] = new List<Production>();
        corp["Workshops"] = new List<Production>();

        for (int i = 0; i < 3; i++) { var f = new Factory(); f.RandomInit(); corp["Factories"].Add(f); }
        for (int i = 0; i < 3; i++) { var g = new Gild(); g.RandomInit(); corp["Gilds"].Add(g); }
        for (int i = 0; i < 3; i++) { var w = new Workshop(); w.RandomInit(); corp["Workshops"].Add(w); }

        QuerySelectionLINQ(corp);
        QuerySelectionExt(corp);
        QueryCountLINQ(corp);
        QueryCountExt(corp);
        QuerySetOpsLINQ(corp);
        QuerySetOpsExt(corp);
        QueryAggregateLINQ(corp);
        QueryAggregateExt(corp);
        QueryGroupLINQ(corp);
        QueryGroupExt(corp);

        var list = new DoublyLinkedList<Production>();
        for (int i = 0; i < 5; i++) { var p = new Production(); p.RandomInit(); list.AddLast(p); }
        Console.WriteLine("\nВыборка EmployeeCount > 50:");
        foreach (var p in list.Where(p => p.EmployeeCount > 50)) p.Show();
        Console.WriteLine($"Sum Employees: {list.Sum(p => p.EmployeeCount)}");
        Console.WriteLine($"Avg Employees: {list.Average(p => p.EmployeeCount):F2}");
        Console.WriteLine("Sorted by Employees asc:");
        foreach (var p in list.OrderBy(p => p.EmployeeCount)) p.Show();
    }

    static void QuerySelectionLINQ(Dictionary<string, List<Production>> corp)
    {
        Console.WriteLine("\nLINQ Selection: Factories with >50 employees");
        var result = from list in corp["Factories"]
                     where list.EmployeeCount > 50
                     select list;
        foreach (var p in result) p.Show();
    }
    static void QuerySelectionExt(Dictionary<string, List<Production>> corp)
    {
        Console.WriteLine("\nExt Selection: Gilds supervised 'Supervisor_50'");
        var result = corp["Gilds"].Where(g => ((Gild)g).Supervisor == "Supervisor_50");
        foreach (var p in result) p.Show();
    }
    static void QueryCountLINQ(Dictionary<string, List<Production>> corp)
    {
        Console.WriteLine("\nLINQ Count: Workshops with equipment >2");
        var count = (from w in corp["Workshops"]
                     where ((Workshop)w).Equipment.Count > 2
                     select w).Count();
        Console.WriteLine(count);
    }
    static void QueryCountExt(Dictionary<string, List<Production>> corp)
    {
        Console.WriteLine("\nExt Count: total employees in all Factories");
        var count = corp["Factories"].Sum(f => f.EmployeeCount);
        Console.WriteLine(count);
    }
    static void QuerySetOpsLINQ(Dictionary<string, List<Production>> corp)
    {
        Console.WriteLine("\nLINQ SetOps: intersection Factories and Gilds by employeeCount");
        var a = corp["Factories"].Select(f => f.EmployeeCount);
        var b = corp["Gilds"].Select(g => g.EmployeeCount);
        var inter = a.Intersect(b);
        Console.WriteLine(string.Join(",", inter));
    }
    static void QuerySetOpsExt(Dictionary<string, List<Production>> corp)
    {
        Console.WriteLine("\nExt SetOps: union Workshops and Gilds by employeeCount");
        var a = corp["Workshops"].Select(w => w.EmployeeCount);
        var b = corp["Gilds"].Select(g => g.EmployeeCount);
        var uni = a.Union(b);
        Console.WriteLine(string.Join(",", uni));
    }
    static void QueryAggregateLINQ(Dictionary<string, List<Production>> corp)
    {
        Console.WriteLine("\nLINQ Aggregate: avg employees across all collections");
        var all = corp.Values.SelectMany(x => x).Select(p => p.EmployeeCount);
        Console.WriteLine(all.Average());
    }
    static void QueryAggregateExt(Dictionary<string, List<Production>> corp)
    {
        Console.WriteLine("\nExt Aggregate: max employees in any collection");
        var all = corp.Values.SelectMany(x => x).Select(p => p.EmployeeCount);
        Console.WriteLine(all.Max());
    }
    static void QueryGroupLINQ(Dictionary<string, List<Production>> corp)
    {
        Console.WriteLine("\nLINQ Group: group by type");
        var all = corp.Values.SelectMany(x => x);
        var grouped = from p in all
                      group p by p.GetType().Name into g
                      select g;
        foreach (var g in grouped)
        {
            Console.WriteLine($"{g.Key}: {g.Count()}");
        }
    }
    static void QueryGroupExt(Dictionary<string, List<Production>> corp)
    {
        Console.WriteLine("\nExt Group: group by employee count range");
        var all = corp.Values.SelectMany(x => x);
        var grouped = all.GroupBy(p => p.EmployeeCount / 10 * 10);
        foreach (var g in grouped)
            Console.WriteLine($"Range {g.Key}-{g.Key + 9}: {g.Count()}");
    }
}