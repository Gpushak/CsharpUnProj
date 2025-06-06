using System;
using System.Collections.Generic;
using System.Threading;

namespace AerialReconConsole
{
    class Program
    {
        static int totalFound = 0;
        static readonly object consoleLock = new object();

        static void Main(string[] args)
        {
            Console.WriteLine("=== Авиаразведка (консольное приложение) ===");

            // Ввод параметров
            int width = ReadInt("Введите ширину карты:", 10, 1000);
            int height = ReadInt("Введите высоту карты:", 10, 1000);
            int scoutCount = ReadInt("Введите количество разведчиков:", 1, 20);

            // Настройка приоритетов
            ThreadPriority[] priorities = (ThreadPriority[])Enum.GetValues(typeof(ThreadPriority));
            ThreadPriority[] scoutPriorities = new ThreadPriority[scoutCount];
            Console.WriteLine("Доступные приоритеты потоков:");
            for (int i = 0; i < priorities.Length; i++)
                Console.WriteLine($"  {i}: {priorities[i]}");
            for (int i = 0; i < scoutCount; i++)
            {
                scoutPriorities[i] = priorities[
                    ReadInt($"Приоритет для разведчика #{i + 1} (0-{priorities.Length - 1}):", 0, priorities.Length - 1)
                ];
            }

            // Генерация карты целей
            bool[,] map = GenerateMap(width, height, out int targetsCount, out double percentage);
            Console.WriteLine($"Карта сгенерирована: {width}x{height}, целей {targetsCount} ({percentage:P0})");

            // Визуализация карты
            Console.WriteLine("Визуализация карты (0 - пусто, 1 - цель):");
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                    Console.Write(map[x, y] ? '1' : '0');
                Console.WriteLine();
            }

            // точка старта
            Random rand = new Random();
            int startX = rand.Next(width);
            int startY = rand.Next(height);
            Console.WriteLine($"Стартовая точка разведчиков: ({startX}, {startY})");

            // Подготовка и запуск потоков-разведчиков
            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < scoutCount; i++)
            {
                int idx = i; 
                Thread t = new Thread(() => ScoutTask(idx, width, height, map, startX, startY, scoutCount));
                t.Priority = scoutPriorities[i];
                threads.Add(t);
            }

            Console.WriteLine("Запуск разведчиков...");
            threads.ForEach(t => t.Start());
            threads.ForEach(t => t.Join());
            Console.WriteLine($"\nОбщее количество найденных целей: {totalFound}");

            Console.WriteLine("Разведка завершена. Нажмите любую клавишу для выхода.");
            Console.ReadKey();
        }

        static int ReadInt(string prompt, int min, int max)
        {
            int value;
            while (true)
            {
                Console.Write($"{prompt} ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out value) && value >= min && value <= max)
                    break;
                Console.WriteLine($"Пожалуйста, введите целое число от {min} до {max}.");
            }
            return value;
        }

        static bool[,] GenerateMap(int width, int height, out int targetsCount, out double percentage)
        {
            var rand = new Random();
            bool[,] map = new bool[width, height];
            percentage = rand.Next(5, 21) / 100.0;
            targetsCount = (int)(width * height * percentage);
            for (int i = 0; i < targetsCount; i++)
            {
                int x = rand.Next(width);
                int y = rand.Next(height);
                map[x, y] = true;
            }
            return map;
        }

        static void ScoutTask(int index, int width, int height, bool[,] map, int startX, int startY, int totalScouts)
        {
            int found = 0;
            double sectorSize = 2 * Math.PI / totalScouts;
            double minAngle = index * sectorSize;
            double maxAngle = minAngle + sectorSize;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int dx = x - startX;
                    int dy = y - startY;
                    if (dx == 0 && dy == 0)
                    {
                        if (index == 0 && map[x, y]) found++;
                    }
                    else
                    {
                        double angle = Math.Atan2(dy, dx);
                        if (angle < 0) angle += 2 * Math.PI;
                        if (angle >= minAngle && angle < maxAngle && map[x, y])

                            found++;
                    }
                }
            }

            lock (consoleLock)
            {
                totalFound += found;
                Console.WriteLine(
                    $"Разведчик #{index + 1}: сектор {index * 360.0 / totalScouts:F1}°–{(index + 1) * 360.0 / totalScouts:F1}°, нашёл {found} целей " +
                    $"(приоритет: {Thread.CurrentThread.Priority})"
                    $"\nОбщее количество найденных целей: {totalFound}"
                );
            }

        }
    }
}
