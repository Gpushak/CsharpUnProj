using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("1. Работа с одномерными массивами");
            Console.WriteLine("2. Работа с двумерными массивами");
            Console.WriteLine("3. Работа с рваными массивами");
            Console.WriteLine("4. Выход");
            Console.Write("Введите номер действия: ");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    OneDimensionalMenu();
                    break;
                case 2:
                    TwoDimensionalMenu();
                    break;
                case 3:
                    JaggedArrayMenu();
                    break;
                case 4:
                    return;
                default:
                    Console.WriteLine("Некорректный выбор. Попробуйте снова.");
                    break;
            }
        }
    }

    // Меню работы с одномерными массивами
    static void OneDimensionalMenu()
    {
        // Реализация работы с одномерными массивами
    }

    // Меню работы с двумерными массивами
    static void TwoDimensionalMenu()
    {
        int[,] array = null;
        while (true)
        {
            Console.WriteLine("\n1. Создать двумерный массив");
            Console.WriteLine("2. Напечатать двумерный массив");
            Console.WriteLine("3. Добавить K строк в начало двумерного массива");
            Console.WriteLine("4. Назад");
            Console.Write("Введите номер действия: ");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    array = CreateTwoDimensionalArray();
                    break;
                case 2:
                    PrintTwoDimensionalArray(array);
                    break;
                case 3:
                    AddRowsToBeginning(ref array);
                    break;
                case 4:
                    return;
                default:
                    Console.WriteLine("Некорректный выбор. Попробуйте снова.");
                    break;
            }
        }
    }

    // Меню работы с рваными массивами
    static void JaggedArrayMenu()
    {
        int[][] jaggedArray = null;
        while (true)
        {
            Console.WriteLine("\n1. Создать рваный массив");
            Console.WriteLine("2. Напечатать рваный массив");
            Console.WriteLine("3. Удалить строку с заданным номером");
            Console.WriteLine("4. Назад");
            Console.Write("Введите номер действия: ");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    jaggedArray = CreateJaggedArray();
                    break;
                case 2:
                    PrintJaggedArray(jaggedArray);
                    break;
                case 3:
                    DeleteRowFromJaggedArray(ref jaggedArray);
                    break;
                case 4:
                    return;
                default:
                    Console.WriteLine("Некорректный выбор. Попробуйте снова.");
                    break;
            }
        }
    }

    // Функция создания рваного массива
    static int[][] CreateJaggedArray()
    {
        Console.Write("Введите количество строк: ");
        int rows = int.Parse(Console.ReadLine());

        int[][] jaggedArray = new int[rows][];
        Random rand = new Random();

        for (int i = 0; i < rows; i++)
        {
            Console.Write($"Введите количество элементов в строке {i + 1}: ");
            int cols = int.Parse(Console.ReadLine());
            jaggedArray[i] = new int[cols];

            for (int j = 0; j < cols; j++)
            {
                jaggedArray[i][j] = rand.Next(1, 100); // Заполнение случайными числами
            }
        }

        return jaggedArray;
    }

    // Функция печати рваного массива
    static void PrintJaggedArray(int[][] jaggedArray)
    {
        if (jaggedArray == null || jaggedArray.Length == 0)
        {
            Console.WriteLine("Массив пустой.");
            return;
        }

        Console.WriteLine("Рваный массив:");
        for (int i = 0; i < jaggedArray.Length; i++)
        {
            Console.Write($"Строка {i + 1}: ");
            foreach (int num in jaggedArray[i])
            {
                Console.Write(num + " ");
            }
            Console.WriteLine();
        }
    }

    // Функция удаления строки с заданным номером в рваном массиве
    static void DeleteRowFromJaggedArray(ref int[][] jaggedArray)
    {
        if (jaggedArray == null || jaggedArray.Length == 0)
        {
            Console.WriteLine("Массив пустой. Удаление невозможно.");
            return;
        }

        Console.Write("Введите номер строки для удаления (начиная с 1): ");
        int rowToDelete = int.Parse(Console.ReadLine()) - 1;

        if (rowToDelete < 0 || rowToDelete >= jaggedArray.Length)
        {
            Console.WriteLine("Некорректный номер строки.");
            return;
        }

        List<int[]> tempList = new List<int[]>(jaggedArray);
        tempList.RemoveAt(rowToDelete);
        jaggedArray = tempList.ToArray();

        Console.WriteLine("Строка успешно удалена.");
    }

    // Функция создания двумерного массива
    static int[,] CreateTwoDimensionalArray()
    {
        Console.Write("Введите количество строк: ");
        int rows = int.Parse(Console.ReadLine());
        Console.Write("Введите количество столбцов: ");
        int cols = int.Parse(Console.ReadLine());

        int[,] array = new int[rows, cols];
        Random rand = new Random();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                array[i, j] = rand.Next(1, 100); // Заполнение случайными числами
            }
        }

        return array;
    }

    // Функция печати двумерного массива
    static void PrintTwoDimensionalArray(int[,] array)
    {
        if (array == null || array.Length == 0)
        {
            Console.WriteLine("Массив пустой.");
            return;
        }

        Console.WriteLine("Двумерный массив:");
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                Console.Write(array[i, j] + " ");
            }
            Console.WriteLine();
        }
    }

    // Функция добавления K строк в начало двумерного массива
    static void AddRowsToBeginning(ref int[,] array)
    {
        if (array == null || array.Length == 0)
        {
            Console.WriteLine("Массив пустой. Операция невозможна.");
            return;
        }

        Console.Write("Введите количество строк для добавления: ");
        int k = int.Parse(Console.ReadLine());
        int rows = array.GetLength(0) + k;
        int cols = array.GetLength(1);

        int[,] newArray = new int[rows, cols];
        Random rand = new Random();

        // Заполнение новых строк случайными числами
        for (int i = 0; i < k; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                newArray[i, j] = rand.Next(1, 100);
            }
        }

        // Копирование старых строк
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                newArray[i + k, j] = array[i, j];
            }
        }

        array = newArray;
        Console.WriteLine($"Добавлено {k} строк(и) в начало массива.");
    }
}
