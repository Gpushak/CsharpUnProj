using System;

public static class JaggedArrayHelper
{
    public static void JaggedArrayMenu()
    {
        int[][] array = null;

        while (true)
        {
            Console.WriteLine("\nМеню рваного массива:");
            Console.WriteLine("1. Создать массив");
            Console.WriteLine("2. Напечатать массив");
            Console.WriteLine("3. Удалить строку с заданным номером");
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
                    array = RemoveRow(array);
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }
        }
    }

    public static int[][] CreateArray()
    {
        Console.Write("Введите количество строк: ");
        int rows = int.Parse(Console.ReadLine());
        int[][] array = new int[rows][];

        Console.WriteLine("Выберите способ заполнения массива:");
        Console.WriteLine("1. Вручную");
        Console.WriteLine("2. С помощью ДСЧ");

        int choice = int.Parse(Console.ReadLine());
        Random rand = new Random();

        for (int i = 0; i < rows; i++)
        {
            Console.Write($"Введите количество элементов в строке {i + 1}: ");
            int cols = int.Parse(Console.ReadLine());
            array[i] = new int[cols];

            if (choice == 1)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write($"Введите элемент [{i + 1},{j + 1}]: ");
                    array[i][j] = int.Parse(Console.ReadLine());
                }
            }
            else
            {
                for (int j = 0; j < cols; j++)
                {
                    array[i][j] = rand.Next(1, 100);
                }
            }
        }
        return array;
    }

    public static void PrintArray(int[][] array)
    {
        if (array == null || array.Length == 0)
        {
            Console.WriteLine("Массив пустой.");
            return;
        }

        foreach (var row in array)
        {
            Console.WriteLine(string.Join("\t", row));
        }
    }

    public static int[][] RemoveRow(int[][] array)
    {
        Console.Write("Введите номер строки для удаления: ");
        int rowToRemove = int.Parse(Console.ReadLine());

        if (rowToRemove < 0 || rowToRemove >= array.Length)
        {
            Console.WriteLine("Некорректный номер строки.");
            return array;
        }

        int[][] newArray = new int[array.Length - 1][];
        int index = 0;

        for (int i = 0; i < array.Length; i++)
        {
            if (i != rowToRemove)
            {
                newArray[index++] = array[i];
            }
        }

        return newArray;
    }
}
