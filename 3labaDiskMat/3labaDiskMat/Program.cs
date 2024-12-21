using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Метод_квайна
{
    internal class Program
    {
        static int GetNumberInt()
        {
            int number;
            string inputNumber;
            bool isRightType;
            do
            {
                Console.WriteLine("Введите номер файла: ");
                inputNumber = Console.ReadLine();
                isRightType = Int32.TryParse(inputNumber, out number);
                if (!isRightType)
                    Console.Write("Неверный ввод данных. Пожалуйста, попробуйте ещё раз.\n");
            }
            while (!isRightType);
            return number;
        }
        static void GetVectorFromFile(ref int [] vector)
        {
            string fileVector;
            FileInfo fileinf;
            do
            {
                fileinf = new FileInfo($@"C:\Users\igleb\OneDrive\Рабочий стол\C#_Un_Proj\CsharpUnProj\3labaDiskMat\3labaDiskMat\bin\Debug\net8.0\{ GetNumberInt()}.txt");
                if (!fileinf.Exists)
                {
                    Console.WriteLine("Файла не существует!");
                }
            }
            while (!fileinf.Exists);
            string path = fileinf.FullName;
            fileVector = File.ReadAllText(path);
            int checkNumber = 0;
            for (int i = 0; i < 16; i++)
            {
                if (Int32.TryParse(fileVector[i].ToString(), out checkNumber))
                {
                    vector[i] = Convert.ToInt32(fileVector[i]) - 48;
                }
            }
        }
        static void GetTruthtable(ref int[,] truthTable, ref int[] vector)
        { 
            Console.WriteLine("Таблица истинности:");
            Console.WriteLine(",---,---,---,---,---,");
            Console.WriteLine("| x | y | z | w | f |");
            Console.WriteLine("|---|---|---|---|---|");
            bool checkY = false;
            for (int i = 0; i < 16; i++)
            {
                int x = (i & 8) >> 3;
                int y = (i & 4) >> 2;
                int z = (i & 2) >> 1;
                int w = i & 1;
                int result = vector[i];
                Console.WriteLine($"| {x} | {y} | {z} | {w} | {result} |");
                truthTable[i, 0] = x;
                truthTable[i, 1] = y;
                truthTable[i, 2] = z;
                truthTable[i, 3] = w;
                truthTable[i, 4] = result;
            }
            Console.WriteLine("'---'---'---'---'---'");
        }
        static string [] GetSDNF(ref int[,] truthTable)
        {
            int size = 0;
            for (int i = 0; i < 16; i++)
            {
                if (truthTable[i, 4] == 1)
                {
                    size++;
                }
            }
            string[] SDNF = new string[size];
            int k = 0;
            for (int i = 0; i < 16; i++)
            {
                if (truthTable[i, 4] == 1)
                {
                    if (truthTable[i, 0] == 0) SDNF[k] += 'X';
                    else SDNF[k] += 'x';
                    if (truthTable[i, 1] == 0) SDNF[k] += 'Y';
                    else SDNF[k] += 'y';
                    if (truthTable[i, 2] == 0) SDNF[k] += 'Z';
                    else SDNF[k] += 'z';
                    if (truthTable[i, 3] == 0) SDNF[k] += 'W';
                    else SDNF[k] += 'w';
                    k++;
                }
            }
            Console.Write("СДНФ: ");
            ShowSDNF(ref SDNF);
            return SDNF;
        }
        static void ShowSDNF(ref string [] SDNF)
        {
            for (int i = 0; i < SDNF.Length; i++)
            {
                Console.Write(SDNF[i]);
                if (i != SDNF.Length - 1)
                {
                    Console.Write(" + ");
                }
            }
            Console.WriteLine();
            Console.WriteLine();
        }
        static string[] Quine(ref string[] SDNF, ref string[] notGlued)
        {
            string[] gluing = new string[0];
            for (int i = 0; i < SDNF.Length; i++)
            {
                bool same, f = false;
                int counter = 0;
                string element = SDNF[i];
                for (int j = 0; j < SDNF.Length; j++)
                {
                    string tempString = "";
                    same = false;
                    for (int k = 0; k < element.Length; k++)
                    {
                        if (element[k] == SDNF[j][k])
                        {
                            tempString += element[k];
                        }
                        else if (Math.Abs(Convert.ToInt32(element[k]) - Convert.ToInt32(SDNF[j][k])) == 32)
                        {
                            same = true;
                        }
                    }
                    if (tempString.Length == element.Length - 1 && same && !IsFound(ref gluing, tempString))
                    {
                        Array.Resize(ref gluing, gluing.Length + 1);
                        gluing[gluing.Length - 1] = tempString;
                        Console.WriteLine($"{i + 1} - {j + 1}: {gluing[gluing.Length - 1]}");
                    }
                    if (tempString.Length != element.Length - 1 || !same)
                    {
                        counter++;
                        if (counter == SDNF.Length)
                        {
                            Array.Resize(ref notGlued, notGlued.Length + 1);
                            notGlued[notGlued.Length - 1] = SDNF[i];
                        }
                    }
                }
            }
            return gluing;
        }
        static void ShowImplicantTable(ref string[] SDNF, ref string[] final)
        {
            for (int i = 0; i < SDNF.Length + 1; i++)
            {
                if (i == 0)
                {
                    Console.Write(",-----,");
                }
                else
                {
                    Console.Write("------,");
                }
            }
            Console.WriteLine();
            Console.Write($"|     |");
            for (int i = 0; i < SDNF.Length; i++)
            {
                Console.Write($" {SDNF[i]} |");
            }
            Console.WriteLine();
            for (int i = 0; i < SDNF.Length + 1; i++)
            {
                if (i == 0)
                {
                    Console.Write("|-----|");
                }
                else
                {
                    Console.Write("------|");
                }
            }
            Console.WriteLine();
            for (int i = 0; i < final.Length; i++)
            {
                Console.Write("| ");
                if (final[i].Length == 1) Console.Write($" {final[i]}");
                else if (final[i].Length == 2) Console.Write(final[i]);
                else if (final[i].Length >= 3) Console.Write(final[i]);
                if (final[i].Length < 3) Console.Write("  |");
                else if (final[i].Length == 3) Console.Write(" |");
                else Console.Write("|");
                for (int j = 0; j < SDNF.Length; j++)
                {
                    bool check = true;
                    for (int k = 0; k < final[i].Length; k++)
                    {
                        if (!SDNF[j].Contains(final[i][k])) check = false;
                    }
                    if (check) Console.Write("  +   |");
                    else Console.Write("      |");
                }
                Console.WriteLine();
                for (int j = 0; j < SDNF.Length + 1; j++)
                {
                    if (j == 0)
                    {
                        Console.Write("'-----'");
                    }
                    else
                    {
                        Console.Write("------'");
                    }
                }
                Console.WriteLine();
            }
        }
        static bool IsFound(ref string[] SDNF, string str)
        {
            for (int i = 0; i < SDNF.Length; i++)
            {
                if (SDNF[i] == str) return true;
            }
            return false;
        }
        static void Main(string[] args)
        {
            int[] vector = new int[16];
            int[,] truthTable = new int[16, 5];
            string[] notGlued = new string[0];
            GetVectorFromFile(ref  vector);
            Console.Write("Полученный вектор из файла: ");
            for (int i = 1; i <= 16; i++)
            {
                if (i / 4 > 0 && i % 4 == 1)
                {
                    Console.Write(" ");
                }
                Console.Write(vector[i - 1]);
            }
            Console.WriteLine();
            GetTruthtable(ref truthTable, ref vector);
            Console.ResetColor();
            string[] SDNF = GetSDNF(ref truthTable);
            if (SDNF.Length == 0) Console.WriteLine("Для введённых данных не существует МДНФ!");
            else
            {
                string[] temp = SDNF;
                string[] gluing;
                do
                {
                    gluing = Quine(ref temp, ref notGlued);
                    Console.WriteLine("-----------");
                    ShowSDNF(ref gluing);
                    if (gluing.Length != 0) temp = gluing;
                }
                while (gluing.Length != 0);
                if (temp[0] != "")
                {
                    Console.WriteLine("Импликантная таблица: ");
                    int k = 0, amount = 0;
                    for (int i = 0; i < notGlued.Length; i++)
                    {
                        if (IsFound(ref temp, notGlued[i]))
                        {
                            amount++;
                        }
                    }
                    int oldLength = temp.Length;
                    Array.Resize(ref temp, temp.Length + notGlued.Length - amount);
                    for (int i = oldLength; i < temp.Length; i++) // если не повторяются
                    {
                        if (!IsFound(ref temp, notGlued[k]))
                        {
                            temp[i] = notGlued[k];
                        }
                        k++;
                    }
                    ShowImplicantTable(ref SDNF, ref temp);
                }
                else
                {
                    Console.WriteLine("Таблица импликантности пуста!");
                }
            }
        }
    }
}
