using System;

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\nГлавное меню:");
            Console.WriteLine("1. Работа с одномерными массивами");
            Console.WriteLine("2. Работа с двумерными массивами");
            Console.WriteLine("3. Работа с рваными массивами");
            Console.WriteLine("4. Выход");
            Console.Write("Выберите действие: ");

            switch (Console.ReadLine())
            {
                case "1":
                    ArrayHelper.ArrayMenu();
                    break;
                case "2":
                    TwoDimArrayHelper.TwoDimArrayMenu();
                    break;
                case "3":
                    JaggedArrayHelper.JaggedArrayMenu();
                    break;
                case "4":
                    Console.WriteLine("Выход из программы.");
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }
        }
    }
}
