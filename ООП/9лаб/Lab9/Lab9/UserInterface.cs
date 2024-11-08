using System;

public class UserInterface
{
    // Метод для создания массива точек
    public static PointArray CreatePointArray()
    {
        Console.WriteLine("Выберите способ создания массива:");
        Console.WriteLine("1. Без параметров");
        Console.WriteLine("2. Случайные значения");
        Console.WriteLine("3. Ввести значения вручную");

        int choice = int.Parse(Console.ReadLine());
        switch (choice)
        {
            case 1:
                return new PointArray();
            case 2:
                Console.Write("Введите размер массива: ");
                int size = int.Parse(Console.ReadLine());
                return new PointArray(size);
            case 3:
                Console.Write("Введите размер массива: ");
                size = int.Parse(Console.ReadLine());
                return new PointArray(size, true);
            default:
                Console.WriteLine("Неверный выбор.");
                return null;
        }
    }

    // Метод для вывода точек массива
    public static void DisplayPoints(PointArray pointArray)
    {
        if (pointArray != null)
        {
            Console.WriteLine("\nЭлементы массива:");
            pointArray.PrintArray();
        }
        else
        {
            Console.WriteLine("Массив не был создан.");
        }
    }
}