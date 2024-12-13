using System;
using System.IO;

namespace QuineMethod
{
    internal class Program
    {
        static void Main(string[] args)
        {
            QuineSolver solver = new QuineSolver();
            solver.Run();
        }
    }

    public class QuineSolver
    {
        private int[] _vector = new int[16];
        private int[,] _truthTable = new int[16, 5];
        private string[] _notGlued = Array.Empty<string>();

        public void Run()
        {
            InputHandler inputHandler = new InputHandler();
            FileHandler fileHandler = new FileHandler(inputHandler);
            TruthTableGenerator truthTableGenerator = new TruthTableGenerator();
            SDNFHandler sdnfHandler = new SDNFHandler();

            fileHandler.GetVectorFromFile(ref _vector);
            Console.Write("Полученный вектор из файла: ");
            DisplayVector();

            truthTableGenerator.Generate(ref _truthTable, _vector);
            truthTableGenerator.Display(_truthTable);

            string[] sdnf = sdnfHandler.GenerateSDNF(ref _truthTable);

            if (sdnf.Length == 0)
            {
                Console.WriteLine("Для введённых данных не существует МДНФ!");
                return;
            }

            string[] temp = sdnf;
            string[] gluing;
            do
            {
                gluing = sdnfHandler.QuineSimplify(ref temp, ref _notGlued);
                Console.WriteLine("-----------");
                sdnfHandler.DisplaySDNF(gluing);
                if (gluing.Length != 0) temp = gluing;
            }
            while (gluing.Length != 0);

            if (temp.Length > 0 && !string.IsNullOrEmpty(temp[0]))
            {
                Console.WriteLine("Импликантная таблица:");
                sdnfHandler.ShowImplicantTable(ref sdnf, ref temp, _notGlued);
            }
            else
            {
                Console.WriteLine("Таблица импликантности пуста!");
            }
        }

        private void DisplayVector()
        {
            for (int i = 1; i <= 16; i++)
            {
                if (i / 4 > 0 && i % 4 == 1)
                {
                    Console.Write(" ");
                }
                Console.Write(_vector[i - 1]);
            }
            Console.WriteLine();
        }
    }

    public class InputHandler
    {
        public int GetNumber()
        {
            int number;
            bool isValid;
            do
            {
                Console.WriteLine("Введите название файла: ");
                string input = Console.ReadLine();
                isValid = int.TryParse(input, out number);
                if (!isValid)
                {
                    Console.WriteLine("Чёта ни то ;(");
                }
            }
            while (!isValid);
            return number;
        }
    }

    public class FileHandler
    {
        private readonly InputHandler _inputHandler;

        public FileHandler(InputHandler inputHandler)
        {
            _inputHandler = inputHandler;
        }

        public void GetVectorFromFile(ref int[] vector)
        {
            string fileContent;
            FileInfo fileInfo;

            do
            {
                int fileNumber = _inputHandler.GetNumber();
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), $"{fileNumber}.txt");
                fileInfo = new FileInfo(filePath);

                if (!fileInfo.Exists)
                {
                    Console.WriteLine("Нет такого");
                }
            }
            while (!fileInfo.Exists);

            fileContent = File.ReadAllText(fileInfo.FullName);

            for (int i = 0; i < 16; i++)
            {
                if (int.TryParse(fileContent[i].ToString(), out int value))
                {
                    vector[i] = value;
                }
            }
        }
    }

    public class TruthTableGenerator
    {
        public void Generate(ref int[,] truthTable, int[] vector)
        {
            for (int i = 0; i < 16; i++)
            {
                int x = (i & 8) >> 3;
                int y = (i & 4) >> 2;
                int z = (i & 2) >> 1;
                int w = i & 1;

                truthTable[i, 0] = x;
                truthTable[i, 1] = y;
                truthTable[i, 2] = z;
                truthTable[i, 3] = w;
                truthTable[i, 4] = vector[i];
            }
        }

        public void Display(int[,] truthTable)
        {
            Console.WriteLine("Таблица истинности:");
            Console.WriteLine(",---,---,---,---,---,");
            Console.WriteLine("| x | y | z | w | f |");
            Console.WriteLine("|---|---|---|---|---|");

            for (int i = 0; i < 16; i++)
            {
                Console.WriteLine($"| {truthTable[i, 0]} | {truthTable[i, 1]} | {truthTable[i, 2]} | {truthTable[i, 3]} | {truthTable[i, 4]} |");
            }

            Console.WriteLine("'---'---'---'---'---'");
        }
    }

    public class SDNFHandler
    {
        public string[] GenerateSDNF(ref int[,] truthTable)
        {
            int size = 0;
            for (int i = 0; i < 16; i++)
            {
                if (truthTable[i, 4] == 1)
                {
                    size++;
                }
            }

            string[] sdnf = new string[size];
            int k = 0;

            for (int i = 0; i < 16; i++)
            {
                if (truthTable[i, 4] == 1)
                {
                    sdnf[k] = "";
                    if (truthTable[i, 0] == 0) sdnf[k] += 'X';
                    else sdnf[k] += 'x';
                    if (truthTable[i, 1] == 0) sdnf[k] += 'Y';
                    else sdnf[k] += 'y';
                    if (truthTable[i, 2] == 0) sdnf[k] += 'Z';
                    else sdnf[k] += 'z';
                    if (truthTable[i, 3] == 0) sdnf[k] += 'W';
                    else sdnf[k] += 'w';
                    k++;
                }
            }

            Console.Write("СДНФ: ");
            DisplaySDNF(sdnf);

            return sdnf;
        }

        public string[] QuineSimplify(ref string[] sdnf, ref string[] notGlued)
        {
            string[] gluing = Array.Empty<string>();

            for (int i = 0; i < sdnf.Length; i++)
            {
                bool isSame = false;
                int counter = 0;
                string element = sdnf[i];

                for (int j = 0; j < sdnf.Length; j++)
                {
                    string tempString = "";
                    isSame = false;

                    for (int k = 0; k < element.Length; k++)
                    {
                        if (element[k] == sdnf[j][k])
                        {
                            tempString += element[k];
                        }
                        else if (Math.Abs(element[k] - sdnf[j][k]) == 32) // Проверка на противоположные переменные (например, x и X)
                        {
                            isSame = true;
                        }
                    }

                    if (tempString.Length == element.Length - 1 && isSame && !IsFound(gluing, tempString))
                    {
                        Array.Resize(ref gluing, gluing.Length + 1);
                        gluing[gluing.Length - 1] = tempString;
                        Console.WriteLine($"{i + 1} - {j + 1}: {gluing[gluing.Length - 1]}");
                    }
                    else
                    {
                        counter++;
                        if (counter == sdnf.Length && !IsFound(notGlued, element))
                        {
                            Array.Resize(ref notGlued, notGlued.Length + 1);
                            notGlued[notGlued.Length - 1] = element;
                        }
                    }
                }
            }

            return gluing;
        }

        public void ShowImplicantTable(ref string[] sdnf, ref string[] final, string[] notGlued)
        {
            Console.WriteLine(",-----," + string.Concat(Enumerable.Repeat("------,", sdnf.Length)));
            Console.Write("|     |");
            foreach (var term in sdnf)
            {
                Console.Write($" {term} |");
            }
            Console.WriteLine();

            Console.WriteLine("|-----|" + string.Concat(Enumerable.Repeat("------|", sdnf.Length)));

            foreach (var implicant in final)
            {
                Console.Write("| ");
                Console.Write(implicant.PadRight(5) + "|");

                foreach (var term in sdnf)
                {
                    bool containsAll = implicant.All(c => term.Contains(c));
                    Console.Write(containsAll ? "  +   |" : "      |");
                }

                Console.WriteLine();
                Console.WriteLine("'-----'" + string.Concat(Enumerable.Repeat("------'", sdnf.Length)));
            }
        }

        public static int[] InputVectorFromConsole()
        {
            int[] vector = new int[16];
            Console.WriteLine("Введите 16 значений для вектора (только 0 и 1), разделяя их пробелом:");

            while (true)
            {
                string input = Console.ReadLine();
                string[] inputs = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (inputs.Length == 16 && inputs.All(val => val == "0" || val == "1"))
                {
                    vector = inputs.Select(int.Parse).ToArray();
                    break;
                }
                else
                {
                    Console.WriteLine("Вводи нормально емае");
                }
            }

            return vector;
        }


        public void DisplaySDNF(string[] sdnf)
        {
            for (int i = 0; i < sdnf.Length; i++)
            {
                Console.Write(sdnf[i]);
                if (i != sdnf.Length - 1)
                {
                    Console.Write(" + ");
                }
            }
            Console.WriteLine();
        }

        private bool IsFound(string[] array, string element)
        {
            return array.Any(item => item == element);
        }
    }

}