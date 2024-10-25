using System;

namespace lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Task 1 ------------------");
            Task1 task1 = new Task1();
            task1.RunTask1();

            Console.WriteLine("Task 2 ------------------");
            Task2 task2 = new Task2();
            task2.RunTask2();

            Console.WriteLine("Task 3 ------------------");
            Task3 task3 = new Task3();
            task3.RunTask3();
        }
    }

    class Task1
    {
        public void RunTask1()
        {
            Console.Write("m: ");
            int m = int.Parse(Console.ReadLine());
            Console.Write("n: ");
            int n = int.Parse(Console.ReadLine());

            if (n != 1)
            {
                int result1 = m++ / n--;
                Console.WriteLine($"1) m++/n--: {result1}");
            }
            else
            {
                Console.WriteLine("1) Divide by zero");
            }

            bool result2 = ++m < n--;
            Console.WriteLine($"2) ++m<n--: {result2}");

            bool result3 = n-- > m;
            Console.WriteLine($"3) n-->m: {result3}");

            for (int i = 0; i < 3; i++)
            {
                Console.Write("x: ");
                int x = int.Parse(Console.ReadLine());
                double resUr = Math.Sin(Math.Pow(x, 3)) + Math.Pow(x, 4) + Math.Pow((Math.Pow(x, 2) + Math.Pow(x, 3)), 1.0 / 5.0);
                Console.WriteLine($"Result: {resUr}");
            }
        }
    }

    class Task2
    {
        public void RunTask2()
        {
            Console.Write("X1: ");
            int X1 = int.Parse(Console.ReadLine());
            Console.Write("Y1: ");
            int Y1 = int.Parse(Console.ReadLine());
            bool inFig = ((Math.Abs(X1) + Math.Abs(Y1) <= 2) && (Y1 >= 0));
            if (inFig)
            {
                Console.WriteLine($"{X1},{Y1} is in figure");
            }
            else
            {
                Console.WriteLine($"{X1},{Y1} is NOT in figure");
            }
        }
    }

    class Task3
    {
        public void RunTask3()
        {
            float a = 1000;
            double b = 0.0001;

            double chis = Math.Pow(a + b, 3) - (Math.Pow(a, 3) + 3 * a * Math.Pow(b, 2));
            double znam = 3 * b * Math.Pow(a, 2) + Math.Pow(b, 3);
            double rezBigV = chis / znam;
            Console.WriteLine($"Result: {rezBigV}");
        }
    }
}
