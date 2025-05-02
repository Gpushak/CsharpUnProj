using System;
using System.Collections.Generic;
using System.Linq;
using _10labLib;

class Program
{
    static void Main()
    {
        // 1. Создание обобщенной коллекции
        SortedDictionary<string, List<Production>> corporations = new SortedDictionary<string, List<Production>>();

        // Заполнение данными
        Random rand = new Random();
        for (int i = 1; i <= 3; i++)
        {
            List<Production> branches = new List<Production>();
            for (int j = 0; j < 3; j++)
            {
                Production branch;
                switch (rand.Next(3))
                {
                    case 0:
                        branch = new Factory();
                        break;
                    case 1:
                        branch = new Gild();
                        break;
                    default:
                        branch = new Workshop();
                        break;
                }
                branch.RandomInit();
                branches.Add(branch);
            }
            corporations.Add($"Corp{i}", branches);
        }

        // Выполнение запросов
        Console.WriteLine("Запрос 1 (LINQ):");
        ExecuteSelectQueryWithLinq(corporations);
        Console.WriteLine("\nЗапрос 1 (Методы):");
        ExecuteSelectQueryWithMethods(corporations);

        // Остальные запросы аналогично...
    }

    // 1a. Выборка данных: Фабрики с числом цехов > 3
    static void ExecuteSelectQueryWithLinq(SortedDictionary<string, List<Production>> corps)
    {
        var query = from corp in corps
                    from branch in corp.Value
                    where branch is Factory f && f.GildCount > 3
                    select branch;
        foreach (var item in query)
            item.Show();
    }

    static void ExecuteSelectQueryWithMethods(SortedDictionary<string, List<Production>> corps)
    {
        var query = corps.SelectMany(c => c.Value)
                        .OfType<Factory>()
                        .Where(f => f.GildCount > 3);
        foreach (var item in query)
            item.Show();
    }

    // 2. Счетчик цехов с руководителем
    static void ExecuteCountQueryWithLinq(SortedDictionary<string, List<Production>> corps, string supervisor)
    {
        int count = (from corp in corps
                     from branch in corp.Value
                     where branch is Gild g && g.Supervisor == supervisor
                     select branch).Count();
        Console.WriteLine($"Count: {count}");
    }

    static void ExecuteCountQueryWithMethods(SortedDictionary<string, List<Production>> corps, string supervisor)
    {
        int count = corps.SelectMany(c => c.Value)
                        .OfType<Gild>()
                        .Count(g => g.Supervisor == supervisor);
        Console.WriteLine($"Count: {count}");
    }

    // 3. Операции над множествами: Объединение двух корпораций
    static void ExecuteUnionQuery(SortedDictionary<string, List<Production>> corps, string corp1, string corp2)
    {
        var list1 = corps[corp1];
        var list2 = corps[corp2];
        var union = list1.Union(list2);
        foreach (var item in union)
            item.Show();
    }

    // 4. Агрегирование: Сумма сотрудников
    static void ExecuteSumQueryWithLinq(SortedDictionary<string, List<Production>> corps)
    {
        int sum = (from corp in corps
                   from branch in corp.Value
                   select branch.EmployeeCount).Sum();
        Console.WriteLine($"Total employees: {sum}");
    }

    static void ExecuteSumQueryWithMethods(SortedDictionary<string, List<Production>> corps)
    {
        int sum = corps.SelectMany(c => c.Value)
                      .Sum(b => b.EmployeeCount);
        Console.WriteLine($"Total employees: {sum}");
    }

    // 5. Группировка по типу
    static void ExecuteGroupQueryWithLinq(SortedDictionary<string, List<Production>> corps)
    {
        var groups = from branch in corps.SelectMany(c => c.Value)
                     group branch by branch.GetType().Name into g
                     select new { Type = g.Key, Count = g.Count() };
        foreach (var g in groups)
            Console.WriteLine($"{g.Type}: {g.Count}");
    }

    static void ExecuteGroupQueryWithMethods(SortedDictionary<string, List<Production>> corps)
    {
        var groups = corps.SelectMany(c => c.Value)
                          .GroupBy(b => b.GetType().Name)
                          .Select(g => new { Type = g.Key, Count = g.Count() });
        foreach (var g in groups)
            Console.WriteLine($"{g.Type}: {g.Count}");
    }
}
public class DoublyLinkedList<T>
{
    public class Node
    {
        public T Value;
        public Node Next;
        public Node Prev;

        public Node(T value)
        {
            Value = value;
        }
    }

    public Node Head { get; private set; }
    public Node Tail { get; private set; }

    public Node First { get { return Head; } }
    public Node Last { get { return Tail; } }

    public void AddLast(T value)
    {
        Node newNode = new Node(value);
        if (Head == null)
        {
            Head = newNode;
            Tail = newNode;
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
        {
            node.Next.Prev = newNode;
        }
        else
        {
            Tail = newNode;
        }
        node.Next = newNode;
    }

    public void Clear()
    {
        Head = null;
        Tail = null;
    }
}

public static class DoublyLinkedListExtensions
{
    // 1. Выборка данных по условию
    public static IEnumerable<Production> Where(this DoublyLinkedList<Production> list, Func<Production, bool> predicate)
    {
        var current = list.First;
        while (current != null)
        {
            if (predicate(current.Value))
                yield return current.Value;
            current = current.Next;
        }
    }

    // 2. Агрегирование: Среднее количество сотрудников
    public static double AverageEmployeeCount(this DoublyLinkedList<Production> list)
    {
        if (list.First == null) return 0;
        int sum = 0, count = 0;
        var current = list.First;
        while (current != null)
        {
            sum += current.Value.EmployeeCount;
            count++;
            current = current.Next;
        }
        return (double)sum / count;
    }

    // 3. Сортировка по возрастанию/убыванию
    public static IEnumerable<Production> OrderByEmployeeCount(this DoublyLinkedList<Production> list, bool ascending = true)
    {
        var comparer = Comparer<Production>.Create((x, y) => x.EmployeeCount.CompareTo(y.EmployeeCount));
        var sortedList = new List<Production>();
        var current = list.First;
        while (current != null)
        {
            sortedList.Add(current.Value);
            current = current.Next;
        }
        sortedList.Sort(comparer);
        if (!ascending)
            sortedList.Reverse();
        return sortedList;
    }
}