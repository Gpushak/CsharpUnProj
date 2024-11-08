using System;

class Program
{
    static void Main()
    {
        // Создание объектов класса Point
        Point point1 = new Point(3.0, 4.0);
        Point point2 = new Point(6.0, 8.0);

        // Вывод координат объектов
        Console.WriteLine("Координаты точек:");
        point1.PrintCoordinates();
        point2.PrintCoordinates();

        // Вызов метода экземпляра для расчета расстояния до начала координат
        Console.WriteLine($"Расстояние от точки point1 до начала координат: {point1.DistanceToOrigin():F2}");
        Console.WriteLine($"Расстояние от точки point2 до начала координат: {point2.DistanceToOrigin():F2}");

        // Вызов статического метода для расчета расстояния до начала координат
        Console.WriteLine($"Расстояние от (3.0, 4.0) до начала координат: {Point.CalculateDistanceToOrigin(3.0, 4.0):F2}");
        Console.WriteLine($"Расстояние от (6.0, 8.0) до начала координат: {Point.CalculateDistanceToOrigin(6.0, 8.0):F2}");

        // Вывод количества созданных объектов
        Console.WriteLine($"Количество созданных объектов Point: {Point.InstanceCount}");

        //--------------ЧАСТЬ 2----------------------------------------------------

        point1 = new Point(10.0, 15.0);
        point2 = new Point(5.0, 8.0);

        Console.WriteLine("Координаты точек:");
        point1.PrintCoordinates();
        point2.PrintCoordinates();

        // Демонстрация унарной операции --
        Console.WriteLine("\nПрименение унарного оператора -- к point1:");
        point1--;
        point1.PrintCoordinates();

        // Демонстрация унарного оператора - (смена координат местами)
        Console.WriteLine("\nПрименение унарного оператора - к point2:");
        Point swappedPoint = -point2;
        swappedPoint.PrintCoordinates();

        // Демонстрация приведения типов
        int intX = point1;
        double doubleY = (double)point1;
        Console.WriteLine($"\nНеявное приведение point1 к int: X = {intX}");
        Console.WriteLine($"Явное приведение point1 к double: Y = {doubleY}");

        // Бинарная операция Point - int
        Console.WriteLine("\nПрименение оператора point1 - 3:");
        Point newPointX = point1 - 3;
        newPointX.PrintCoordinates();

        // Бинарная операция int - Point
        Console.WriteLine("\nПрименение оператора 4 - point2:");
        Point newPointY = 4 - point2;
        newPointY.PrintCoordinates();

        // Бинарная операция Point - Point (расстояние между точками)
        double distance = point1 - point2;
        Console.WriteLine($"\nРасстояние между point1 и point2: {distance:F2}");

        //--------------ЧАСТЬ 3----------------------------------------------------
        // Создание массива точек через пользовательский интерфейс
        PointArray pointArray = UserInterface.CreatePointArray();

        // Вывод массива точек
        UserInterface.DisplayPoints(pointArray);

        // Нахождение точки, ближайшей к началу координат
        Point nearestPoint = FindNearestToOrigin(pointArray);
        if (nearestPoint != null)
        {
            Console.WriteLine($"\nТочка, ближайшая к началу координат: X = {nearestPoint.X}, Y = {nearestPoint.Y}");
        }

        // Вывод количества созданных объектов PointArray
        Console.WriteLine($"Количество созданных объектов PointArray: {PointArray.InstanceCount}");

    }

    public static Point FindNearestToOrigin(PointArray pointArray)
    {
        if (pointArray == null || pointArray[0] == null)
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
