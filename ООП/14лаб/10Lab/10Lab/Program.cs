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

        public Node(T value) => Value = value;
    }

    public Node Head { get; private set; }
    public Node Tail { get; private set; }

    public Node First => Head;
    public Node Last => Tail;

    public void AddLast(T value)
    {
        Node newNode = new Node(value);
        if (Head == null)
        {
            Head = Tail = newNode;
        }
        else
        {
            Tail.Next = newNode;
            newNode.Prev = Tail;
            Tail = newNode;
        }
    }

    public void InsertAfter(Node node, T value)
    {
        if (node == null) return;

        Node newNode = new Node(value)
        {
            Next = node.Next,
            Prev = node
        };

        if (node.Next != null)
            node.Next.Prev = newNode;
        else
            Tail = newNode;

        node.Next = newNode;
    }

    public void Clear()
    {
        Head = Tail = null;
    }
}

class Program
{

    static void Main()
    {
        // Часть 1: Создание и заполнение коллекции
        SortedDictionary<string, List<Production>> corporations = new SortedDictionary<string, List<Production>>();

        Random rand = new Random();
        string[] corpNames = { "AlphaCorp", "BetaGroup", "GammaInc" };
        foreach (var corpName in corpNames)
        {
            List<Production> branches = new List<Production>();
            for (int j = 0; j < 4; j++) // Добавляем по 4 элемента в каждую корпорацию
            {
                Production branch;
                switch (rand.Next(3))
                {
                    case 0:
                        branch = new Factory();
                        ((Factory)branch).GildCount = rand.Next(1, 10); // Явное задание параметров
                        break;
                    case 1:
                        branch = new Gild();
                        ((Gild)branch).Supervisor = $"Supervisor_{rand.Next(1, 50)}";
                        break;
                    default:
                        branch = new Workshop();
                        ((Workshop)branch).Equipment = Enumerable.Range(1, 3)
                            .Select(n => $"Equipment_{rand.Next(100)}").ToList();
                        break;
                }
                branch.Name = $"{corpName}_Branch{j + 1}";
                branch.EmployeeCount = rand.Next(10, 1000);
                branches.Add(branch);
            }
            corporations.Add(corpName, branches);
        }

        // Часть 1: Выполнение всех запросов
        Console.WriteLine("=== ЗАПРОС 1: Фабрики с более чем 5 цехами (LINQ) ===");
        ExecuteSelectQueryWithLinq(corporations);

        Console.WriteLine("\n=== ЗАПРОС 2: Количество цехов с руководителем 'Supervisor_5' (LINQ) ===");
        ExecuteCountQueryWithLinq(corporations, "Supervisor_5");

        Console.WriteLine("\n=== ЗАПРОС 3: Объединение AlphaCorp и BetaGroup (Методы) ===");
        ExecuteUnionQuery(corporations, "AlphaCorp", "BetaGroup");

        Console.WriteLine("\n=== ЗАПРОС 4: Сумма сотрудников (LINQ) ===");
        ExecuteSumQueryWithLinq(corporations);

        Console.WriteLine("\n=== ЗАПРОС 5: Группировка по типу (Методы) ===");
        ExecuteGroupQueryWithMethods(corporations);

        // Часть 2: Работа с DoublyLinkedList
        DoublyLinkedList<Production> myList = new DoublyLinkedList<Production>();
        corporations["AlphaCorp"].ForEach(p => myList.AddLast(p));

        Console.WriteLine("\n=== Методы расширения для DoublyLinkedList ===");
        Console.WriteLine("Фильтрация: цеха с >500 сотрудниками");
        var filtered = myList.Where(p => p.EmployeeCount > 500);
        foreach (var item in filtered) item.Show();

        Console.WriteLine($"\nСреднее сотрудников: {myList.AverageEmployeeCount():F2}");
        Console.WriteLine("\nСортировка по возрастанию:");
        foreach (var item in myList.OrderByEmployeeCount())
            Console.WriteLine($"{item.Name}: {item.EmployeeCount}");
    }

    #region Часть 1: Реализация всех запросов
    // 1a. Выборка фабрик с GildCount > 5
    static void ExecuteSelectQueryWithLinq(SortedDictionary<string, List<Production>> corps)
    {
        var query = from corp in corps
                    from branch in corp.Value
                    where branch is Factory f && f.GildCount > 5
                    select branch;
        foreach (var item in query) item.Show();
    }

    // 1b. То же через методы расширения
    static void ExecuteSelectQueryWithMethods(SortedDictionary<string, List<Production>> corps)
    {
        var query = corps.SelectMany(c => c.Value)
                        .OfType<Factory>()
                        .Where(f => f.GildCount > 5);
        foreach (var item in query) item.Show();
    }

    // 2a. Количество цехов с заданным руководителем (LINQ)
    static void ExecuteCountQueryWithLinq(SortedDictionary<string, List<Production>> corps, string supervisor)
    {
        int count = (from corp in corps
                     from branch in corp.Value
                     where branch is Gild g && g.Supervisor == supervisor
                     select branch).Count();
        Console.WriteLine($"Найдено цехов: {count}");
    }

    // 2b. Счетчик через методы
    static void ExecuteCountQueryWithMethods(SortedDictionary<string, List<Production>> corps, string supervisor)
    {
        int count = corps.SelectMany(c => c.Value)
                        .OfType<Gild>()
                        .Count(g => g.Supervisor == supervisor);
        Console.WriteLine($"Найдено цехов: {count}");
    }

    // 3a. Объединение двух корпораций
    static void ExecuteUnionQuery(SortedDictionary<string, List<Production>> corps, string corp1, string corp2)
    {
        var union = corps[corp1].Union(corps[corp2]).Distinct();
        foreach (var item in union) item.Show();
    }

    // 4a. Агрегирование: сумма сотрудников (LINQ)
    static void ExecuteSumQueryWithLinq(SortedDictionary<string, List<Production>> corps)
    {
        int sum = (from corp in corps
                   from branch in corp.Value
                   select branch.EmployeeCount).Sum();
        Console.WriteLine($"Всего сотрудников: {sum}");
    }

    // 4b. Сумма через методы
    static void ExecuteSumQueryWithMethods(SortedDictionary<string, List<Production>> corps)
    {
        int sum = corps.SelectMany(c => c.Value).Sum(b => b.EmployeeCount);
        Console.WriteLine($"Всего сотрудников: {sum}");
    }

    // 5a. Группировка по типу (LINQ)
    static void ExecuteGroupQueryWithLinq(SortedDictionary<string, List<Production>> corps)
    {
        var groups = from branch in corps.SelectMany(c => c.Value)
                     group branch by branch.GetType().Name into g
                     select new { Type = g.Key, Count = g.Count(), Avg = g.Average(b => b.EmployeeCount) };

        foreach (var g in groups)
            Console.WriteLine($"{g.Type}: {g.Count} шт., Среднее сотрудников: {g.Avg:F0}");
    }

    // 5b. Группировка через методы
    static void ExecuteGroupQueryWithMethods(SortedDictionary<string, List<Production>> corps)
    {
        var groups = corps.SelectMany(c => c.Value)
                        .GroupBy(b => b.GetType().Name)
                        .Select(g => new {
                            Type = g.Key,
                            Count = g.Count(),
                            Avg = g.Average(b => b.EmployeeCount)
                        });

        foreach (var g in groups)
            Console.WriteLine($"{g.Type}: {g.Count} шт., Среднее сотрудников: {g.Avg:F0}");
    }
    #endregion
}

public static class DoublyLinkedListExtensions
{
    // 1. Выборка по условию
    public static IEnumerable<Production> Where(this DoublyLinkedList<Production> list, Func<Production, bool> predicate)
    {
        var current = list.First;
        while (current != null)
        {
            if (predicate(current.Value)) yield return current.Value;
            current = current.Next;
        }
    }

    // 2. Агрегирование
    public static int SumEmployeeCount(this DoublyLinkedList<Production> list) =>
        list.Where(_ => true).Sum(p => p.EmployeeCount);

    public static double AverageEmployeeCount(this DoublyLinkedList<Production> list) =>
        list.Where(_ => true).Average(p => p.EmployeeCount);

    public static int MaxEmployeeCount(this DoublyLinkedList<Production> list) =>
        list.Where(_ => true).Max(p => p.EmployeeCount);

    // 3. Сортировка
    public static IEnumerable<Production> OrderByEmployeeCount(this DoublyLinkedList<Production> list, bool ascending = true)
    {
        return ascending ?
            list.Where(_ => true).OrderBy(p => p.EmployeeCount) :
            list.Where(_ => true).OrderByDescending(p => p.EmployeeCount);
    }
}