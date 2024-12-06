using System;
using _10labLib;

class Program
{
    static void Main(string[] args)
    {
        var rand = new Random();

        Production[] productions1 = new Production[6];
        for (int i = 0; i < 2; i++)
        {
            productions1[i] = new Factory();
            productions1[i].RandomInit();
        }
        for (int i = 2; i < 4; i++)
        {
            productions1[i] = new Gild();
            productions1[i].RandomInit();
        }
        for (int i = 4; i < 6; i++)
        {
            productions1[i] = new Workshop();
            productions1[i].RandomInit();
        }

        foreach (var item in productions1)
        {
            item.Show();
            Console.WriteLine();
        }

        //----------------------------2 Часть---------------------------

        Production[] productions2 = new Production[6];
        productions2[0] = new Factory { Name = "Factory_A", EmployeeCount = 50, GildCount = 3 };
        productions2[1] = new Gild { Name = "Gild_2", EmployeeCount = 20, Supervisor = "Jane Smith" };
        productions2[2] = new Factory { Name = "Factory_B", EmployeeCount = 60, GildCount = 5 };
        productions2[3] = new Gild { Name = "Gild_4", EmployeeCount = 25, Supervisor = "Michael Brown" };

        // Запрос 1: Наименование всех цехов на заданной фабрике
        Console.WriteLine("Enter factory name to list its Gilds:");
        string factoryName = Console.ReadLine();
        Queries.GetGildNamesOnFactory(productions2, factoryName);

        // Запрос 2: Имена рабочих заданного цеха
        Console.WriteLine("\nEnter Gild name to get its supervisor:");
        string GildName = Console.ReadLine();
        Queries.GetSupervisorsOnGild(productions2, GildName);

        // Запрос 3: Количество рабочих на фабриках
        Console.WriteLine("\nCalculating total employees in all factories...");
        int totalEmployees = Queries.GetEmployeeCountOnFactory(productions2);
        Console.WriteLine($"Total employees in all factories: {totalEmployees}");

        //---------------------------3 Часть-------------------------------

        // Создание массива из объектов различных классов
        IInit[] items = new IInit[4];
        items[0] = new Factory();
        items[1] = new Gild();
        items[3] = new Workshop();
        items[2] = new StandaloneUnit();


        // Инициализация объектов
        foreach (var item in items)
        {
            item.RandomInit();
        }

        // Просмотр объектов
        foreach (var item in items)
        {
            if (item is Production production)
                production.Show();
            else if (item is StandaloneUnit unit)
                unit.Show();
        }

        Console.WriteLine("\nSorting by Employee Count...");
        Array.Sort(items, (x, y) =>
        {
            if (x is Production px && y is Production py)
                return px.EmployeeCount.CompareTo(py.EmployeeCount);
            return 0;
        });

        foreach (var item in items)
        {
            if (item is Production production)
                production.Show();
        }
    }
}
