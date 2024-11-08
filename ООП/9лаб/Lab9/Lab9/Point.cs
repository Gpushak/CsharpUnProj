using System;

public class Point
{
    // Закрытые поля для координат точки
    private double x;
    private double y;

    // Статическая переменная для подсчета количества созданных объектов
    private static int instanceCount = 0;

    // Свойства для доступа к координатам
    public double X
    {
        get { return x; }
        set { x = value; }
    }

    public double Y
    {
        get { return y; }
        set { y = value; }
    }

    // Конструктор для инициализации координат и увеличения счетчика
    public Point(double x, double y)
    {
        this.x = x;
        this.y = y;
        instanceCount++;  // Увеличение статической переменной при создании объекта
    }

    // Метод для вычисления расстояния от точки до начала координат
    public double DistanceToOrigin()
    {
        return Math.Sqrt(x * x + y * y);
    }

    // Статический метод для вычисления расстояния от точки до начала координат
    public static double CalculateDistanceToOrigin(double x, double y)
    {
        return Math.Sqrt(x * x + y * y);
    }

    // Статическое свойство для получения количества созданных объектов
    public static int InstanceCount
    {
        get { return instanceCount; }
    }

    // Метод для вывода координат точки
    public void PrintCoordinates()
    {
        Console.WriteLine($"Точка с координатами: X = {x}, Y = {y}");
    }

    //--------------ЧАСТЬ 2----------------------------------------------------

    // Перегрузка унарного оператора -- для уменьшения координат x и y на 1
    public static Point operator --(Point p)
    {
        p.x--;
        p.y--;
        return p;
    }

    // Перегрузка унарного оператора - для смены координат местами
    public static Point operator -(Point p)
    {
        return new Point(p.y, p.x);
    }

    // Перегрузка оператора приведения типа к int (неявная)
    public static implicit operator int(Point p)
    {
        return (int)p.x;
    }

    // Перегрузка оператора приведения типа к double (явная)
    public static explicit operator double(Point p)
    {
        return p.y;
    }

    // Бинарная операция: Point - int (уменьшается координата x)
    public static Point operator -(Point p, int value)
    {
        return new Point(p.x - value, p.y);
    }

    // Бинарная операция: int - Point (уменьшается координата y)
    public static Point operator -(int value, Point p)
    {
        return new Point(p.x, p.y - value);
    }

    // Бинарная операция: Point - Point (вычисляется расстояние между точками)
    public static double operator -(Point p1, Point p2)
    {
        return Math.Sqrt(Math.Pow(p1.x - p2.x, 2) + Math.Pow(p1.y - p2.y, 2));
    }
}
