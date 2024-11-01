using System;

public static class TwoDimArrayHelper
{
    public static void TwoDimArrayMenu()
    {
        int[,] array = null;

        while (true)
        {
            Console.WriteLine("\nМеню двумерного массива:");
            Console.WriteLine("1. Создать массив");
            Console.WriteLine("2. Напечатать массив");
            Console.WriteLine("3. Добавить K строк в начало");
            Console.WriteLine("4. Назад");

            switch (Console.ReadLine())
            {
                case "1":
                    array = CreateArray();
                    break;
                case "2":
                    PrintArray(array);
                    break;
                case "3":
                    array = AddRowsAtBeginning(array);
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }
        }
    }

    public static int[,] CreateArray()
    {
        Console.Write("Введите количество строк: ");
        int rows = int.Parse(Console.ReadLine());
        Console.Write("Введите количество столбцов: ");
        int cols = int.Parse(Console.ReadLine());

        Console.WriteLine("Выберите способ заполнения массива:");
        Console.WriteLine("1. Вручную");
        Console.WriteLine("2. С помощью ДСЧ");

        int choice = int.Parse(Console.ReadLine());
        int[,] array = new int[rows, cols];

        if (choice == 1)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write($"Введите элемент [{i + 1},{j + 1}]: ");
                    array[i, j] = int.Parse(Console.ReadLine());
                }
            }
        }
        else
        {
            Random rand = new Random();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    array[i, j] = rand.Next(1, 100);
                }
            }
        }

        return array;
    }

    public static void PrintArray(int[,] array)
    {
        if (array == null || array.Length == 0)
        {
            Console.WriteLine("Массив пустой.");
            return;
        }
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                Console.Write(array[i, j] + "\t");
            }
            Console.WriteLine();
        }
    }

    public static int[,] AddRowsAtBeginning(int[,] array)
    {
        Console.Write("Введите количество строк для добавления в начало: ");
        int k = int.Parse(Console.ReadLine());

        int originalRows = array.GetLength(0);
        int cols = array.GetLength(1);
        int[,] newArray = new int[originalRows + k, cols];
        Random rand = new Random();

        for (int i = 0; i < k; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                newArray[i, j] = rand.Next(1, 100);
            }
        }

        for (int i = 0; i < originalRows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                newArray[i + k, j] = array[i, j];
            }
        }

        return newArray;
    }
}
