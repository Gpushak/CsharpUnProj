using System;

class SeriesFunction
{
    const double Eps = 0.0001;

    // Метод для вычисления точного значения функции
    public static double ExactValue(double x)
    {
        return (0.25 * Math.Log((1 + x) / (1 - x))) + 0.5 * Math.Atan(x);
    }

    // Метод для вычисления суммы ряда при заданном n
    public static double SumForN(double x, int n)
    {
        double sum = 0.0;
        for (int i = 0; i <= n; i++)
        {
            sum += Math.Pow(x, 4 * i + 1) / (4 * i + 1);
        }
        return sum;
    }

    // Метод для вычисления суммы ряда до достижения точности Eps
    public static double SumForEps(double x)
    {
        double sum = 0.0;
        double term;
        int i = 0;
        do
        {
            term = Math.Pow(x, 4 * i + 1) / (4 * i + 1);
            sum += term;
            i++;
        } while (Math.Abs(term) > Eps);

        return sum;
    }

    // Метод для расчета и вывода результатов
    public static void Calculate(double a, double b, int k, int n)
    {
        double step = (b - a) / k;
        for (double x = a; x <= b+step; x += step)
        {
            double SN = SumForN(x, n);
            double SE = SumForEps(x);
            double Y = ExactValue(x);

            Console.WriteLine($"X = {x:F5}, SN = {SN:F5}, SE = {SE:F5}, Y = {Y:F5}");
        }
    }

    // Точка входа в программу
    static void Main(string[] args)
    {
        double a = 0.1; // Начальное значение x
        double b = 0.8; // Конечное значение x
        int k = 10;     // Количество шагов
        int n = 3;      // Заданное количество слагаемых

        Calculate(a, b, k, n);
    }
}
