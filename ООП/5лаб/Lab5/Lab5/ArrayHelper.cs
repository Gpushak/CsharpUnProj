using System;

public static class ArrayHelper
{
    public static void ArrayMenu()
    {
        int[] array = null;

        while (true)
        {
            Console.WriteLine("\nМеню одномерного массива:");
            Console.WriteLine("1. Создать массив");
            Console.WriteLine("2. Напечатать массив");
            Console.WriteLine("3. Удалить все нечетные элементы");
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
                    array = RemoveOddElements(array);
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }
        }
    }

    public static int[] CreateArray()
    {
        Console.Write("Введите количество элементов массива: ");
        int n = int.Parse(Console.ReadLine());

        Console.WriteLine("Выберите способ заполнения массива:");
        Console.WriteLine("1. Вручную");
        Console.WriteLine("2. С помощью ДСЧ");

        int choice = int.Parse(Console.ReadLine());
        int[] array = new int[n];

        if (choice == 1)
        {
            for (int i = 0; i < n; i++)
            {
                Console.Write($"Введите элемент {i + 1}: ");
                array[i] = int.Parse(Console.ReadLine());
            }
        }
        else
        {
            Random rand = new Random();
            for (int i = 0; i < n; i++)
            {
                array[i] = rand.Next(1, 100);
            }
        }

        return array;
    }

    public static void PrintArray(int[] array)
    {
        if (array == null || array.Length == 0)
        {
            Console.WriteLine("Массив пустой.");
            return;
        }
        Console.WriteLine("Массив: " + string.Join(", ", array));
    }

    public static int[] RemoveOddElements(int[] array)
    {
        if (array == null || array.Length == 0)
        {
            Console.WriteLine("Массив пустой.");
            return array;
        }

        List<int> evenNumbers = new List<int>();

        foreach (int x in array)
        {
            if (x % 2 == 0)
            {
                evenNumbers.Add(x);
            }
        }

        return evenNumbers.ToArray();
    }
}
