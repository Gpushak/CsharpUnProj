using System;
using System.Collections.Generic;

public interface IInit
{
    void Init();
    void RandomInit();
}

public class Production : IInit, IComparable<Production>, ICloneable
{
    public string Name { get; set; }
    public int EmployeeCount { get; set; }

    public Production() { }

    public Production(string name, int employeeCount)
    {
        Name = name;
        EmployeeCount = employeeCount;
    }

    public Production(Production other)
    {
        Name = other.Name;
        EmployeeCount = other.EmployeeCount;
    }

    public virtual void Show()
    {
        Console.WriteLine($"Production: {Name}, Employees: {EmployeeCount}");
    }

    public virtual void Init()
    {
        Console.Write("Enter Name: ");
        Name = Console.ReadLine();
        Console.Write("Enter Employee Count: ");
        EmployeeCount = int.Parse(Console.ReadLine());
    }

    public virtual void RandomInit()
    {
        var rand = new Random();
        Name = $"Production_{rand.Next(1, 100)}";
        EmployeeCount = rand.Next(5, 100);
    }

    public override bool Equals(object obj)
    {
        if (obj is Production production)
            return Name == production.Name && EmployeeCount == production.EmployeeCount;
        return false;
    }

    public int CompareTo(Production other)
    {
        if (other == null) return 1;
        return EmployeeCount.CompareTo(other.EmployeeCount);
    }

    public object Clone()
    {
        return new Production(Name, EmployeeCount);
    }

    public Production ShallowCopy()
    {
        return (Production)MemberwiseClone();
    }
}

public class Factory : Production
{
    public int GildCount { get; set; }

    public Factory() { }

    public Factory(string name, int employeeCount, int gildCount)
        : base(name, employeeCount)
    {
        GildCount = gildCount;
    }

    public Factory(Factory other) : base(other)
    {
        GildCount = other.GildCount;
    }

    public override void Show()
    {
        base.Show();
        Console.WriteLine($"Gilds: {GildCount}");
    }

    public override void Init()
    {
        base.Init();
        Console.Write("Enter Gild Count: ");
        GildCount = int.Parse(Console.ReadLine());
    }

    public override void RandomInit()
    {
        base.RandomInit();
        GildCount = new Random().Next(1, 10);
    }

    public override bool Equals(object obj)
    {
        if (obj is Factory factory)
            return base.Equals(factory) && GildCount == factory.GildCount;
        return false;
    }

    public virtual Production BaseProduction
    {
        get
        {
            return new Production(Name, EmployeeCount);
        }
    }
}

public class TestCollections
{
    private Queue<Factory> collection1;
    private Queue<string> collection2;
    private Dictionary<Production, Factory> collection3;
    private Dictionary<string, Factory> collection4;

    public TestCollections(int numberOfElements)
    {
        collection1 = new Queue<Factory>();
        collection2 = new Queue<string>();
        collection3 = new Dictionary<Production, Factory>();
        collection4 = new Dictionary<string, Factory>();

        for (int i = 0; i < numberOfElements; i++)
        {
            var factory = new Factory($"Factory_{i}", i * 10, new Random().Next(1, 10));
            collection1.Enqueue(factory);
            collection2.Enqueue(factory.ToString());
            collection3.Add(new Production($"Production_{i}", i * 10), factory);
            collection4.Add($"Key_{i}", factory);
        }
    }

    public void MeasureSearchTimes()
    {
        var firstElement = collection1.Peek();
        var centralElement = collection1.ToArray()[collection1.Count / 2];
        var lastElement = collection1.ToArray()[collection1.Count - 1];
        var nonExistentElement = new Factory("Non-existent", 0, 0);

        // Измерение времени для collection1 (Queue<Factory>)
        var start = DateTime.Now;
        bool foundFirst = collection1.Contains(firstElement);
        var timeFirst = DateTime.Now - start;

        start = DateTime.Now;
        bool foundCentral = collection1.Contains(centralElement);
        var timeCentral = DateTime.Now - start;

        start = DateTime.Now;
        bool foundLast = collection1.Contains(lastElement);
        var timeLast = DateTime.Now - start;

        start = DateTime.Now;
        bool foundNonExistent = collection1.Contains(nonExistentElement);
        var timeNonExistent = DateTime.Now - start;

        // Измерение времени для collection2 (Queue<string>)
        start = DateTime.Now;
        bool foundFirstString = collection2.Contains(firstElement.ToString());
        var timeFirstString = DateTime.Now - start;

        start = DateTime.Now;
        bool foundCentralString = collection2.Contains(centralElement.ToString());
        var timeCentralString = DateTime.Now - start;

        start = DateTime.Now;
        bool foundLastString = collection2.Contains(lastElement.ToString());
        var timeLastString = DateTime.Now - start;

        start = DateTime.Now;
        bool foundNonExistentString = collection2.Contains("Non-existent");
        var timeNonExistentString = DateTime.Now - start;

        // Измерение времени для collection3 (Dictionary<Production, Factory>)
        start = DateTime.Now;
        bool foundKeyFirst = collection3.ContainsKey(new Production($"Production_0", 0));
        var timeKeyFirst = DateTime.Now - start;

        start = DateTime.Now;
        bool foundKeyCentral = collection3.ContainsKey(new Production($"Production_{collection3.Count / 2}", 0));
        var timeKeyCentral = DateTime.Now - start;

        start = DateTime.Now;
        bool foundKeyLast = collection3.ContainsKey(new Production($"Production_{collection3.Count - 1}", 0));
        var timeKeyLast = DateTime.Now - start;

        start = DateTime.Now;
        bool foundKeyNonExistent = collection3.ContainsKey(new Production("Non-existent", 0));
        var timeKeyNonExistent = DateTime.Now - start;

        // Измерение времени для collection4 (Dictionary<string, Factory>)
        start = DateTime.Now;
        bool foundKeyFirstString = collection4.ContainsKey("Key_0");
        var timeKeyFirstString = DateTime.Now - start;

        start = DateTime.Now;
        bool foundKeyCentralString = collection4.ContainsKey($"Key_{collection4.Count / 2}");
        var timeKeyCentralString = DateTime.Now - start;

        start = DateTime.Now;
        bool foundKeyLastString = collection4.ContainsKey($"Key_{collection4.Count - 1}");
        var timeKeyLastString = DateTime.Now - start;

        start = DateTime.Now;
        bool foundKeyNonExistentString = collection4.ContainsKey("Non-existent");
        var timeKeyNonExistentString = DateTime.Now - start;

        // Вывод результатов
        Console.WriteLine($"First Element Search Time (Queue<Factory>): {timeFirst.TotalMilliseconds} ms");
        Console.WriteLine($"Central Element Search Time (Queue<Factory>): {timeCentral.TotalMilliseconds} ms");
        Console.WriteLine($"Last Element Search Time (Queue<Factory>): {timeLast.TotalMilliseconds} ms");
        Console.WriteLine($"Non-existent Element Search Time (Queue<Factory>): {timeNonExistent.TotalMilliseconds} ms");

        Console.WriteLine($"First Element Search Time (Queue<string>): {timeFirstString.TotalMilliseconds} ms");
        Console.WriteLine($"Central Element Search Time (Queue<string>): {timeCentralString.TotalMilliseconds} ms");
        Console.WriteLine($"Last Element Search Time (Queue<string>): {timeLastString.TotalMilliseconds} ms");
        Console.WriteLine($"Non-existent Element Search Time (Queue<string>): {timeNonExistentString.TotalMilliseconds} ms");

        Console.WriteLine($"First Key Search Time (Dictionary<Production, Factory>): {timeKeyFirst.TotalMilliseconds} ms");
        Console.WriteLine($"Central Key Search Time (Dictionary<Production, Factory>): {timeKeyCentral.TotalMilliseconds} ms");
        Console.WriteLine($"Last Key Search Time (Dictionary<Production, Factory>): {timeKeyLast.TotalMilliseconds} ms");
        Console.WriteLine($"Non-existent Key Search Time (Dictionary<Production, Factory>): {timeKeyNonExistent.TotalMilliseconds} ms");

        Console.WriteLine($"First Key Search Time (Dictionary<string, Factory>): {timeKeyFirstString.TotalMilliseconds} ms");
        Console.WriteLine($"Central Key Search Time (Dictionary<string, Factory>): {timeKeyCentralString.TotalMilliseconds} ms");
        Console.WriteLine($"Last Key Search Time (Dictionary<string, Factory>): {timeKeyLastString.TotalMilliseconds} ms");
        Console.WriteLine($"Non-existent Key Search Time (Dictionary<string, Factory>): {timeKeyNonExistentString.TotalMilliseconds} ms");
    }
}

class Program
{
    static void Main(string[] args)
    {
        TestCollections testCollections = new TestCollections(1000);
        testCollections.MeasureSearchTimes();
    }
}