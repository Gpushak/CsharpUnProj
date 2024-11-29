using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        // Ввод вектора длиной 16
        int[] vector = InputVector();
        if (vector.Length != 16)
        {
            Console.WriteLine("Ошибка: вектор должен содержать ровно 16 значений.");
            return;
        }

        // Формирование таблицы истинности
        Console.WriteLine("\nТаблица истинности:");
        PrintTruthTable(vector);

        // Формирование СДНФ
        string sdnf = GetSDNF(vector);
        Console.WriteLine($"\nСДНФ: {sdnf}");

        // Формирование СКНФ
        string sknf = GetSKNF(vector);
        Console.WriteLine($"\nСКНФ: {sknf}");

        // Минимизация методом Квайна
        Console.WriteLine("\nМинимизация методом Квайна:");
        MinimizeQuine(vector);
    }

    static int[] InputVector()
    {
        Console.WriteLine("Выберите способ ввода данных:");
        Console.WriteLine("1. Ввод с консоли");
        Console.WriteLine("2. Чтение из файла");

        int choice = int.Parse(Console.ReadLine() ?? "1");
        if (choice == 1)
        {
            Console.WriteLine("Введите 16 значений вектора (0 или 1), разделенных пробелом:");
            return Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
        }
        else if (choice == 2)
        {
            Console.WriteLine("Введите путь к файлу:");
            string filePath = Console.ReadLine();
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath).Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                                                .Select(int.Parse).ToArray();
            }
            else
            {
                Console.WriteLine("Файл не найден.");
                return new int[0];
            }
        }
        else
        {
            Console.WriteLine("Неверный выбор.");
            return new int[0];
        }
    }

    static void PrintTruthTable(int[] vector)
    {
        Console.WriteLine("| №  | a | b | c | d | f |");
        Console.WriteLine("|----|---|---|---|---|---|");
        for (int i = 0; i < 16; i++)
        {
            int a = (i & 8) >> 3;
            int b = (i & 4) >> 2;
            int c = (i & 2) >> 1;
            int d = i & 1;
            Console.WriteLine($"| {i + 1,2} | {a} | {b} | {c} | {d} | {vector[i]} |");
        }
    }

    static string GetSDNF(int[] vector)
    {
        List<string> terms = new List<string>();
        for (int i = 0; i < 16; i++)
        {
            if (vector[i] == 1)
            {
                terms.Add(GenerateTerm(i, true));
            }
        }
        return string.Join(" v ", terms);
    }

    static string GetSKNF(int[] vector)
    {
        List<string> terms = new List<string>();
        for (int i = 0; i < 16; i++)
        {
            if (vector[i] == 0)
            {
                terms.Add("(" + GenerateTerm(i, false) + ")");
            }
        }
        return string.Join(" ^ ", terms);
    }

    static string GenerateTerm(int index, bool isSDNF)
    {
        string[] vars = { "a", "b", "c", "d" };
        List<string> term = new List<string>();
        for (int i = 0; i < 4; i++)
        {
            bool bit = (index & (1 << (3 - i))) != 0;
            term.Add((isSDNF == bit ? "" : "!") + vars[i]);
        }
        return string.Join("", term);
    }

    static void MinimizeQuine(int[] vector)
    {
        // Шаг 1: Первичные импликанты
        List<string> implicants = GetPrimaryImplicants(vector);
        Console.WriteLine("\nШаг 1: Первичные импликанты:");
        Console.WriteLine(string.Join(", ", implicants));

        // Шаг 2: Построение импликантной матрицы
        Console.WriteLine("\nШаг 2: Импликантная матрица:");
        BuildImplicantMatrix(vector, implicants);

        // Шаг 3: Минимизация
        Console.WriteLine("\nШаг 3: Минимизация:");
        List<string> minimized = MinimizeImplicants(vector, implicants);
        Console.WriteLine("Минимизированная функция: " + string.Join(" v ", minimized));
    }

    static List<string> GetPrimaryImplicants(int[] vector)
    {
        // Преобразование индексов строк таблицы истинности в бинары
        List<string> implicants = new List<string>();
        for (int i = 0; i < 16; i++)
        {
            if (vector[i] == 1)
            {
                implicants.Add(Convert.ToString(i, 2).PadLeft(4, '0'));
            }
        }

        // Попарное склеивание
        bool[] used = new bool[implicants.Count];
        List<string> newImplicants = new List<string>();
        for (int i = 0; i < implicants.Count; i++)
        {
            for (int j = i + 1; j < implicants.Count; j++)
            {
                string combined = CombineImplicants(implicants[i], implicants[j]);
                if (combined != null)
                {
                    newImplicants.Add(combined);
                    used[i] = used[j] = true;
                }
            }
        }

        // Добавление несократившихся импликант
        for (int i = 0; i < implicants.Count; i++)
        {
            if (!used[i])
            {
                newImplicants.Add(implicants[i]);
            }
        }
        return newImplicants;
    }

    static string CombineImplicants(string imp1, string imp2)
    {
        int diffCount = 0;
        int diffIndex = -1;

        for (int i = 0; i < imp1.Length; i++)
        {
            if (imp1[i] != imp2[i])
            {
                diffCount++;
                diffIndex = i;
            }
        }

        if (diffCount == 1)
        {
            char[] combined = imp1.ToCharArray();
            combined[diffIndex] = '-';
            return new string(combined);
        }
        return null;
    }

    static void BuildImplicantMatrix(int[] vector, List<string> implicants)
    {
        Console.Write("|       |");
        for (int i = 0; i < 16; i++) Console.Write($" {i + 1,3} |");
        Console.WriteLine();

        foreach (var implicant in implicants)
        {
            Console.Write($"| {implicant,-5} |");
            for (int i = 0; i < 16; i++)
            {
                Console.Write($"  {(Covers(implicant, i) ? "+" : " ")} |");
            }
            Console.WriteLine();
        }
    }

    static bool Covers(string implicant, int index)
    {
        string binaryIndex = Convert.ToString(index, 2).PadLeft(4, '0');
        for (int i = 0; i < implicant.Length; i++)
        {
            if (implicant[i] != '-' && implicant[i] != binaryIndex[i])
            {
                return false;
            }
        }
        return true;
    }

    static List<string> MinimizeImplicants(int[] vector, List<string> implicants)
    {
        // Выбор покрытия
        return implicants; // Пока возвращаем все импликанты
    }
}
