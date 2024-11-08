using System;

public static class UserInterface
{
    public static void ShowMainMenu()
    {
        Console.WriteLine("\nМеню:");
        Console.WriteLine("1. Демонстрация класса Point и его методов");
        Console.WriteLine("2. Демонстрация перегрузки операторов для класса Point");
        Console.WriteLine("3. Демонстрация работы с массивом PointArray");
        Console.WriteLine("4. Выход");
    }

    public static int GetUserChoice()
    {
        Console.Write("Выберите действие: ");
        if (int.TryParse(Console.ReadLine(), out int choice))
        {
            return choice;
        }
        return -1;
    }

    public static void DemonstratePointClass() //Демонстрация 1 части
    {
        Console.WriteLine("\nВведите координаты для первой точки:");
        Point point1 = CreatePoint();

        Console.WriteLine("\nВведите координаты для второй точки:");
        Point point2 = CreatePoint();

        Console.WriteLine("\nКоординаты точек:");
        point1.PrintCoordinates();
        point2.PrintCoordinates();

        Console.WriteLine($"\nРасстояние от точки point1 до начала координат: {point1.DistanceToOrigin():F2}");
        Console.WriteLine($"Расстояние от точки point2 до начала координат: {point2.DistanceToOrigin():F2}");

        Console.WriteLine($"\nРасстояние от ({point1.X}, {point1.Y}) до начала координат: {Point.CalculateDistanceToOrigin(point1.X, point1.Y):F2}");
        Console.WriteLine($"Расстояние от ({point2.X}, {point2.Y}) до начала координат: {Point.CalculateDistanceToOrigin(point2.X, point2.Y):F2}");

        Console.WriteLine($"\nКоличество созданных объектов Point: {Point.InstanceCount}");
    }

    public static void DemonstrateOverloadedOperators() //Демонстрация 2 части
    {
        Console.WriteLine("\nВведите координаты для первой точки:");
        Point point1 = CreatePoint();

        Console.WriteLine("\nВведите координаты для второй точки:");
        Point point2 = CreatePoint();

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
    }

    public static void DemonstratePointArray() //Демонстрация 3 части
    {
        PointArray pointArray = CreatePointArray();
        DisplayPoints(pointArray);

        Point nearestPoint = FindNearestToOrigin(pointArray);
        if (nearestPoint != null)
        {
            Console.WriteLine($"\nТочка, ближайшая к началу координат: X = {nearestPoint.X}, Y = {nearestPoint.Y}");
        }

        Console.WriteLine($"\nКоличество созданных объектов PointArray: {PointArray.InstanceCount}");
    }

    private static Point CreatePoint()
    {
        Console.Write("Введите координату X: ");
        double x = double.Parse(Console.ReadLine());

        Console.Write("Введите координату Y: ");
        double y = double.Parse(Console.ReadLine());

        return new Point(x, y);
    }

    public static PointArray CreatePointArray()
    {
        Console.Write("Введите размер массива точек: ");
        int size = int.Parse(Console.ReadLine());

        Console.Write("Заполнить массив случайными значениями? (y/n): ");
        bool isRandom = Console.ReadLine().ToLower() == "y";

        if (isRandom)
        {
            return new PointArray(size);
        }
        else
        {
            return new PointArray(size, true);
        }
    }

    public static void DisplayPoints(PointArray pointArray)
    {
        Console.WriteLine("\nСодержимое массива точек:");
        pointArray.PrintArray();
    }

    public static Point FindNearestToOrigin(PointArray pointArray)
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
