using System;

public class PointArray
{
    Point[] arr; 
    static int instanceCount = 0; 

    public PointArray()
    {
        arr = new Point[0];
        instanceCount++;
    }

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

    public void PrintArray()
    {
        for (int i = 0; i < arr.Length; i++)
        {
            Console.WriteLine($"Точка {i + 1}: X = {arr[i].X}, Y = {arr[i].Y}");
        }
    }

    // Индексатор
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

    public int Length
    {
        get { return arr.Length; }
    }

    public static int InstanceCount
    {
        get { return instanceCount; }
    }
}
