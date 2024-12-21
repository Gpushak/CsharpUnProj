using System;
using System.IO;

class Program
{
    const int size = 6;
    static int[,] matrix = new int[size, size];

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("Меню:");
            Console.WriteLine("1. Ввести матрицу с консоли");
            Console.WriteLine("2. Загрузить матрицу из файла");
            Console.WriteLine("3. Показать матрицу");
            Console.WriteLine("4. Проверить свойства матрицы");
            Console.WriteLine("5. Выход");
            Console.Write("Выберите опцию: ");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    InputMatrixFromConsole();
                    break;
                case 2:
                    LoadMatrixFromFile();
                    break;
                case 3:
                    DisplayMatrix();
                    break;
                case 4:
                    CheckMatrixProperties();
                    break;
                case 5:
                    return;
                default:
                    Console.WriteLine("Неверный выбор, попробуйте снова.");
                    break;
            }
        }
    }

    static void InputMatrixFromConsole()
    {
        Console.WriteLine("Введите элементы матрицы 6x6 (только 0 или 1):");
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Console.Write($"Элемент [{i},{j}]: ");
                matrix[i, j] = int.Parse(Console.ReadLine());
            }
        }
    }

    static void LoadMatrixFromFile()
    {
        Console.Write("Введите имя файла: ");
        string fileName = Console.ReadLine();

        try
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                for (int i = 0; i < size; i++)
                {
                    string[] row = sr.ReadLine().Split();
                    for (int j = 0; j < size; j++)
                    {
                        matrix[i, j] = int.Parse(row[j]);
                    }
                }
            }
            Console.WriteLine("Матрица успешно загружена из файла.");
        }
        catch (Exception e)
        {
            Console.WriteLine("Ошибка чтения файла: " + e.Message);
        }
    }

    static void DisplayMatrix()
    {
        Console.WriteLine("Матрица:");
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Console.Write(matrix[i, j] + " ");
            }
            Console.WriteLine();
        }
    }

    static void CheckEquivalence(bool reflexive, bool symmetric, bool transitive)
    {
        if (reflexive && symmetric && transitive)
        {
            Console.WriteLine("Отношение является отношением эквивалентности.");
        }
        else
        {
            Console.WriteLine("Отношение не является отношением эквивалентности.");
        }
    }

    static void CheckOrder(bool reflexive, bool antiSymmetric, bool transitive, bool connected)
    {
        if (reflexive && antiSymmetric && transitive && connected)
        {
            Console.WriteLine("Отношение является нестрогим полным порядком.");
        }
        else if (!reflexive && antiSymmetric && transitive && connected)
        {
            Console.WriteLine("Отношение является строгим полным порядком.");
        }
        else if (reflexive && antiSymmetric && transitive && !connected)
        {
            Console.WriteLine("Отношение является нестрогим частичным порядком.");
        }
        else if (!reflexive && antiSymmetric && transitive && !connected)
        {
            Console.WriteLine("Отношение является строгим частичным порядком.");
        }
        else
        {
            Console.WriteLine("Отношение не соответствует ни одному из известных типов порядка.");
        }
    }
    static void CheckMatrixProperties()
    {
        bool reflexive = true, antiReflexive = true;
        bool symmetric = true, antiSymmetric = true, asymmetric = true;
        bool transitive = true, connected = true;

        for (int i = 0; i < size; i++)
        {
            if (matrix[i, i] != 1)
                reflexive = false;
            if (matrix[i, i] != 0)
                antiReflexive = false;

            for (int j = 0; j < size; j++)
            {
                if (i != j)
                {
                    if (matrix[i, j] != matrix[j, i])
                        symmetric = false;
                    if (matrix[i, j] == 1 && matrix[j, i] == 1)
                        asymmetric = false;
                    if (matrix[i, j] == 0 && matrix[j, i] == 0)
                        connected = false;
                }
                for (int k = 0; k < size; k++)
                {
                    if (matrix[i, j] == 1 && matrix[j, k] == 1 && matrix[i, k] != 1)
                        transitive = false;
                }
            }
        }

        Console.WriteLine("Свойства матрицы:");
        Console.WriteLine($"Рефлексивна: {reflexive}");
        Console.WriteLine($"Антирефлексивна: {antiReflexive}");
        Console.WriteLine($"Симметрична: {symmetric}");
        Console.WriteLine($"Асимметрична: {asymmetric}");
        Console.WriteLine($"Транзитивна: {transitive}");
        Console.WriteLine($"Связана (полная): {connected}");

        CheckEquivalence(reflexive, symmetric, transitive);
        CheckOrder(reflexive, antiSymmetric, transitive, connected);
    }


    
}
