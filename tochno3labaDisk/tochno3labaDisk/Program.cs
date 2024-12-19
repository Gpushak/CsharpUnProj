using System;

namespace Completeness
{
    public class BooleanFunction
    {
        public int[] Values { get; private set; }

        public BooleanFunction(int size)
        {
            Values = new int[size];
        }

        public void SetValue(int index, int value)
        {
            Values[index] = value;
        }

        public bool IsT0() => Values == null || Values[0] == 0;
        public bool IsT1() => Values == null || Values[Values.Length - 1] == 1;
        public bool IsS()
        {
            if (Values == null) return true;
            if (Values.Length == 1) return false;
            for (int i = 0; i < Values.Length / 2; i++)
            {
                if (Values[i] == Values[Values.Length - 1 - i])
                {
                    return false;
                }
            }
            return true;
        }
        public bool IsM()
        {
            if (Values == null || Values.Length == 1) return true;
            for (int i = 0; i < Values.Length / 2; i++)
            {
                if (Values[i] == 1 && Values[i] > Values[i + Values.Length / 2]) return false;
            }
            int[] func1 = new int[Values.Length / 2];
            int[] func2 = new int[Values.Length / 2];
            for (int i = 0; i < Values.Length / 2; i++)
            {
                func1[i] = Values[i];
                func2[i] = Values[func2.Length + i];
            }
            return new BooleanFunction(func1.Length) { Values = func1 }.IsM() && new BooleanFunction(func2.Length) { Values = func2 }.IsM();
        }
        public bool IsL()
        {
            if (Values.Length == 1 || Values.Length == 2) return true;
            int[] c = new int[Values.Length];
            c[0] = Values[0];
            c[1] = Operation(c[0], Values[1]);
            c[2] = Operation(c[0], Values[2]);
            if (Values.Length == 4)
            {
                c[3] = Operation(Operation(Operation(c[0], c[1]), c[2]), Values[3]);
                return c[3] == 0;
            }
            else
            {
                c[3] = Operation(c[0], Values[4]);
                c[4] = Operation(Operation(Operation(c[0], c[2]), c[1]), Values[3]);
                c[5] = Operation(Operation(Operation(c[0], c[3]), c[1]), Values[5]);
                c[6] = Operation(Operation(Operation(c[0], c[3]), c[2]), Values[6]);
                c[7] = Operation(Operation(Operation(Operation(Operation(Operation(Operation(c[0], c[3]), c[2]), c[1]), c[6]), c[5]), c[4]), Values[7]);
                return c[4] == 0 && c[5] == 0 && c[6] == 0 && c[7] == 0;
            }
        }
        private int Operation(int i, int j) => i == j ? 0 : 1;
    }

    internal class Program
    {
        static int GetNumberInt(string str)
        {
            int number;
            string inputNumber;
            bool isRightType;
            do
            {
                Console.Write(str);
                inputNumber = Console.ReadLine();
                isRightType = Int32.TryParse(inputNumber, out number);
                if (!isRightType)
                    Console.Write("Неверный ввод данных. Пожалуйста, попробуйте ещё раз.\n");
            }
            while (!isRightType);
            return number;
        }

        static int GetNumberIntVector(string str)
        {
            int number;
            string inputNumber;
            bool isRightType;
            do
            {
                Console.Write(str);
                inputNumber = Console.ReadLine();
                isRightType = Int32.TryParse(inputNumber, out number);
                if (!isRightType || number < 0 || number > 1)
                    Console.Write("Неверный ввод данных. Пожалуйста, попробуйте ещё раз.\n");
            }
            while (!isRightType || number < 0 || number > 1);
            return number;
        }

        static void ShowTable(BooleanFunction[] functions)
        {
            Console.WriteLine("Принадлежность классам");
            Console.WriteLine(",----,----,----,---,---,---,");
            Console.WriteLine("|    | T0 | T1 | S | M | L |");
            Console.WriteLine("|----|----|----|---|---|---|");

            foreach (var func in functions)
            {
                Console.WriteLine($"| f{Array.IndexOf(functions, func) + 1} | {ConvertSymbol(func.IsT0())}  | {ConvertSymbol(func.IsT1())}  | " +
                    $"{ConvertSymbol(func.IsS())} | {ConvertSymbol(func.IsM())} | {ConvertSymbol(func.IsL())} |");
                Console.WriteLine("|----|----|----|---|---|---|");
            }

            Console.WriteLine("'----'----'----'---'---'---'");
            GetAnswer(functions);
        }

        static void GetAnswer(BooleanFunction[] functions)
        {
            bool t0 = functions.All(f => f.IsT0());
            bool t1 = functions.All(f => f.IsT1());
            bool s = functions.All(f => f.IsS());
            bool m = functions.All(f => f.IsM());
            bool l = functions.All(f => f.IsL());

            if (t0 || t1 || s || m || l) Console.WriteLine("Система неполная");
            else Console.WriteLine("Система полная");
        }

        static char ConvertSymbol(bool f) => f ? '+' : ' ';

        static void Main(string[] args)
        {
            int amountFunctions;
            do
            {
                amountFunctions = GetNumberInt("Введите количество функций: ");
                if (amountFunctions < 1 || amountFunctions > 3)
                {
                    Console.WriteLine("Функций может быть не больше трёх и не меньше 1!");
                }
            }
            while (amountFunctions < 1 || amountFunctions > 3);

            BooleanFunction[] functions = new BooleanFunction[amountFunctions];

            for (int i = 0; i < amountFunctions; i++)
            {
                int size;
                do
                {
                    size = GetNumberInt("Введите размер функции: ");
                    if (size != 1 && size != 2 && size != 4 && size != 8)
                    {
                        Console.WriteLine("Размер функции должен быть равен 2^i!");
                    }
                }
                while (size != 1 && size != 2 && size != 4 && size != 8);

                functions[i] = new BooleanFunction(size);
                for (int j = 0; j < size; j++)
                {
                    functions[i].SetValue(j, GetNumberIntVector($"{j + 1} элемент: "));
                }
            }

            ShowTable(functions);
        }
    }
}