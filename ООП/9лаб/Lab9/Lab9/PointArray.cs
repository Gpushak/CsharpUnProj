using System;

public class PointArray
{
    Point[] arr; 
    static int instanceCount = 0; 

    // Конструктор без параметров
    public PointArray()
    {
        arr = new Point[0];
        instanceCount++;
    }

    // Конструктор с параметром для создания массива случайных объектов Point
    public PointArray(int size)
    {
        arr = new Point[size];
        Random rand = new Random();
        for (int i = 0; i < size; i++)
        {
            arr[i] = new Point(rand.NextDouble() * 100, rand.NextDouble() * 100);
        }
        instanceCount++;
    }

    // Метод для просмотра элементов массива
    public void PrintArray()
    {
        for (int i = 0; i < arr.Length; i++)
        {
            Console.WriteLine($"Точка {i + 1}: X = {arr[i].X}, Y = {arr[i].Y}");
        }
    }

    // Индексатор для доступа к элементам массива
    public Point this[int index]
    {
        get
        {
            if (index < 0 || index >= arr.Length)
                throw new ArgumentException("Индекс находится за пределами массива.");
            return arr[index];
        }
        set
        {
            if (index < 0 || index >= arr.Length)
                throw new ArgumentException("Индекс находится за пределами массива.");
            arr[index] = value;
        }
    }

    // Свойство Length для получения длины массива
    public int Length
    {
        get { return arr.Length; }
    }

    // Статическое свойство для получения количества созданных объектов PointArray
    public static int InstanceCount
    {
        get { return instanceCount; }
    }
}