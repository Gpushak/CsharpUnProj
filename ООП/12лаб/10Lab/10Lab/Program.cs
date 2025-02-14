using System;
using _10labLib;

class Program
{
    static void Main(string[] args)
    {

        // Шаг 1: Формируем двунаправленный список и заполняем его объектами из иерархии классов.
        LinkedList<Production> list = new LinkedList<Production>();

        // Создадим несколько объектов, используя RandomInit() для демонстрации
        Production prod = new Production();
        prod.RandomInit();
        list.AddLast(prod);

        Factory factory = new Factory();
        factory.RandomInit();
        list.AddLast(factory);

        Gild gild = new Gild();
        gild.RandomInit();
        list.AddLast(gild);

        Workshop workshop = new Workshop();
        workshop.RandomInit();
        list.AddLast(workshop);

        // Можно добавить и больше объектов по необходимости

        Console.WriteLine("Исходный список:");
        // Шаг 2: Распечатываем полученный список.
        PrintList(list);

        // Шаг 3: Обработка списка – добавить (вставить) в список клоны объектов, находящихся на позициях 1, 3, 5 и т.д.
        InsertOddPositionClones(list);

        Console.WriteLine("\nИзменённый список:");
        // Шаг 4: Распечатываем список после обработки.
        PrintList(list);

        // Шаг 5: Удаляем список из памяти.
        list.Clear();
        list = null;

        Console.WriteLine("\nСписок удалён из памяти.");
        // Для того чтобы окно консоли не закрывалось сразу (при запуске вне IDE)
        Console.WriteLine("Нажмите любую клавишу для выхода...");
        Console.ReadKey();
    }

    /// <summary>
    /// Метод для вывода списка. Для каждого элемента вызывается его метод Show().
    /// </summary>
    static void PrintList(LinkedList<Production> list)
    {
        int i = 1;
        foreach (var item in list)
        {
            Console.WriteLine($"Элемент {i}:");
            item.Show();
            Console.WriteLine();
            i++;
        }
    }

    /// <summary>
    /// Метод обрабатывает список: для каждого оригинального узла с нечётным номером (1, 3, 5…)
    /// создаётся клон, который вставляется сразу после него.
    /// </summary>
    static void InsertOddPositionClones(LinkedList<Production> list)
    {
        // Для избежания влияния вставленных узлов на нумерацию,
        // сначала собираем ссылки на оригинальные узлы с позициями 1,3,5,...
        List<LinkedListNode<Production>> nodesToClone = new List<LinkedListNode<Production>>();
        int pos = 1;
        for (var node = list.First; node != null; node = node.Next)
        {
            if (pos % 2 == 1)
            {
                nodesToClone.Add(node);
            }
            pos++;
        }

        // Для каждого найденного узла создаём клон и вставляем его после оригинала.
        foreach (var node in nodesToClone)
        {
            Production cloned = (Production)node.Value.Clone();
            list.AddAfter(node, cloned);
        }
    }
}

