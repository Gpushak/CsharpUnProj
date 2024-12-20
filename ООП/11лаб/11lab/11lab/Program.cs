using System;
using System.Collections.Generic;
using _10labLib;

namespace LabWork
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Production> productions = new List<Production>();
            // Добавление объектов
            productions.Add(new Factory("Factory1", 10, 5));
            productions.Add(new Gild("Gild1", 20, "Supervisor1"));
            productions.Add(new Workshop("Workshop1", 15, new List<string> { "Drill", "Lathe" }));

            // Меню для добавления и удаления объектов
            void ShowMenu()
            {
                Console.WriteLine("1. Добавить объект");
                Console.WriteLine("2. Удалить объект");
                Console.WriteLine("3. Вывести объекты");
                Console.WriteLine("4. Выход");
            }

            void AddProduction(List<Production> productions)
            {
                // Логика добавления объекта
            }

            void RemoveProduction(List<Production> productions)
            {
                // Логика удаления объекта
            }

            void PrintFactories(List<Production> productions)
            {
                foreach (var prod in productions)
                {
                    if (prod is Factory)
                    {
                        prod.Show();
                    }
                }
            }

            int CountGilds(List<Production> productions)
            {
                return productions.Count(prod => prod is Gild);
            }

            foreach (var production in productions)
            {
                production.Show();
            }

            List<Production> clonedProductions = new List<Production>(productions);

            productions.Sort();
            var foundProduction = productions.Find(p => p.Name == "Factory1");

            //Задание 2

            //Задание 3


        }
    }
}