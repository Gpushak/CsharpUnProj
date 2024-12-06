using ProductionHierarchy;
using System;

//Запросы:
//Наименование всех цехов на заданной фабрике.
//Имена рабочих заданного цеха.
//Количество рабочих на фабрике.

class Program
{
    static void Main()
    {
        Production[] productions = new Production[3];

        productions[0] = new Factory();
        productions[1] = new Workshop();
        productions[2] = new ShopFloor();


        foreach (var production in productions)
        {
            production.RandomInit();
        }

        Console.WriteLine("Обычный вызов:");
        foreach (var production in productions)
        {
            production.Show();
        }

        Console.WriteLine("\nВиртуальный вызов:");
        foreach (var production in productions)
        {
            production.Show();
        }
    }
}
