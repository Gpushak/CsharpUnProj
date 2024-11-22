using System;

class Program
{
    static void Main()
    {
        // ------------ ЧАСТЬ 1: Демонстрация класса Point -----------------
        Console.WriteLine("Демонстрация класса Point:");

        Point point1 = new Point(3.0, 4.0);
        Point point2 = new Point(6.0, 8.0);

        Console.WriteLine("\nКоординаты точек:");
        point1.PrintCoordinates();
        point2.PrintCoordinates();

        Console.WriteLine($"\nРасстояние от (3.0, 4.0) до начала координат: {Point.CalculateDistanceToOrigin(3.0, 4.0):F2}");
        Console.WriteLine($"Расстояние от (6.0, 8.0) до начала координат: {Point.CalculateDistanceToOrigin(6.0, 8.0):F2}");

        Console.WriteLine($"\nКоличество созданных объектов Point: {Point.InstanceCount}");

        // ------------ ЧАСТЬ 2: Демонстрация перегрузки операторов --------
        Console.WriteLine("\nДемонстрация перегрузки операторов:");

        point1 = new Point(10.0, 15.0);
        point2 = new Point(5.0, 8.0);

        Console.WriteLine("\nКоординаты точек:");
        point1.PrintCoordinates();
        point2.PrintCoordinates();

        Console.WriteLine("\nПрименение унарного оператора -- к point1:");
        point1--;
        point1.PrintCoordinates();

        Console.WriteLine("\nПрименение унарного оператора - к point2:");
        Point swappedPoint = -point2;
        swappedPoint.PrintCoordinates();

        int intX = point1;
        double doubleY = (double)point1;
        Console.WriteLine($"\nНеявное приведение point1 к int: X = {intX}");
        Console.WriteLine($"Явное приведение point1 к double: Y = {doubleY}");

        Console.WriteLine("\nПрименение оператора point1 - 3:");
        Point newPointX = point1 - 3;
        newPointX.PrintCoordinates();

        Console.WriteLine("\nПрименение оператора 4 - point2:");
        Point newPointY = 4 - point2;
        newPointY.PrintCoordinates();

        double distance = point1 - point2;
        Console.WriteLine($"\nРасстояние между point1 и point2: {distance:F2}");

        // ------------ ЧАСТЬ 3: Демонстрация работы с массивом ------------
        Console.WriteLine("\nДемонстрация работы с массивом PointArray:");

        PointArray pointArray = new PointArray(5); // массив из 5 случайных точек
        Console.WriteLine("\nСодержимое массива точек:");
        pointArray.PrintArray();

        Point nearestPoint = FindNearestToOrigin(pointArray);
        if (nearestPoint != null)
        {
            Console.WriteLine($"\nТочка, ближайшая к началу координат: X = {nearestPoint.X}, Y = {nearestPoint.Y}");
        }

        Console.WriteLine($"\nКоличество созданных объектов PointArray: {PointArray.InstanceCount}");
    }

    static Point FindNearestToOrigin(PointArray pointArray)
    {
        if (pointArray.Length == 0)
        {
            Console.WriteLine("Массив пустой или не инициализирован.");
            return null;
        }

        Point nearestPoint = pointArray[0];
        double minDistance = nearestPoint.DistanceToOrigin();

        for (int i = 1; i < pointArray.Length; i++)
        {
            double distance = pointArray[i].DistanceToOrigin();
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestPoint = pointArray[i];
            }
        }
        return nearestPoint;
    }
}
