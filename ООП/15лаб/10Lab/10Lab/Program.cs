using System;
using System.Threading;

namespace AirReconConsoleCoverage
{
    class Program
    {
        // Размеры карты
        const int MapRows = 20;
        const int MapCols = 20;
        static int[,] map = new int[MapRows, MapCols];

        // Объект для синхронизации доступа к карте
        static readonly object lockObj = new object();
        static readonly Random rand = new Random();

        // Количество разведчиков – их курсы будут равномерно распределены по кругу
        const int NumberOfScouts = 8;

        static void Main(string[] args)
        {
            Console.WriteLine("Инициализация карты...");
            InitializeMap();
            Console.WriteLine("Начальное состояние карты:");
            PrintMap();

            // Выбираем случайную стартовую точку на карте
            int startRow = rand.Next(MapRows);
            int startCol = rand.Next(MapCols);
            Console.WriteLine($"\nСтартовая точка: ({startRow}, {startCol})");

            // Вычисляем направления для разведчиков равномерно по кругу.
            // Принимаем, что 0° соответствует движению вверх.
            (int dr, int dc)[] directions = new (int dr, int dc)[NumberOfScouts];
            for (int i = 0; i < NumberOfScouts; i++)
            {
                double angle = 2 * Math.PI * i / NumberOfScouts;
                int dr = -(int)Math.Round(Math.Cos(angle));
                int dc = (int)Math.Round(Math.Sin(angle));

                if (dr == 0 && dc == 0)
                    dc = 1;

                directions[i] = (dr, dc);
                Console.WriteLine($"Разведчик {i + 1}: направление (dr = {dr}, dc = {dc}), угол ≈ {Math.Round(angle * 180 / Math.PI)}°");
            }

            // Создаем и запускаем потоки для каждого направления
            Thread[] scouts = new Thread[NumberOfScouts];
            for (int i = 0; i < NumberOfScouts; i++)
            {
                int scoutId = i + 1;
                int currentRow = startRow;
                int currentCol = startCol;
                var direction = directions[i];

                Thread scoutThread = new Thread(() => ScoutRoutine(scoutId, currentRow, currentCol, direction));

                // Устанавливаем приоритет до вызова Start()
                scoutThread.Priority = ThreadPriority.Normal;

                scouts[i] = scoutThread;
                scoutThread.Start();
            }

            // Ожидаем завершения работы всех потоков
            foreach (Thread t in scouts)
            {
                t.Join();
            }

            Console.WriteLine("\nВсе разведчики завершили обход карты.");
            Console.WriteLine("\nИтоговое состояние карты:");
            PrintMap();
            Console.WriteLine("\nДля выхода нажмите любую клавишу...");
            Console.ReadKey();
        }

        // Метод инициализации карты: заполняем нулями и случайно располагаем цели (значением 1)
        static void InitializeMap()
        {
            for (int i = 0; i < MapRows; i++)
                for (int j = 0; j < MapCols; j++)
                    map[i, j] = 0;

            int numberOfTargets = 30;
            for (int k = 0; k < numberOfTargets; k++)
            {
                int i = rand.Next(MapRows);
                int j = rand.Next(MapCols);
                map[i, j] = 1;
            }
        }

        // Метод, выводящий карту в консоль
        static void PrintMap()
        {
            // Для каждого элемента выводим его значение с разделителем.
            for (int i = 0; i < MapRows; i++)
            {
                for (int j = 0; j < MapCols; j++)
                {
                    // Если в ячейке осталась цель, выводим 1, иначе 0.
                    Console.Write(map[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        // Метод, реализующий работу разведчика в отдельном потоке.
        static void ScoutRoutine(int scoutId, int row, int col, (int dr, int dc) direction)
        {
            int targetsFound = 0;
            while (row >= 0 && row < MapRows && col >= 0 && col < MapCols)
            {
                if (map[row, col] == 1)
                {
                    lock (lockObj)
                    {
                        if (map[row, col] == 1)
                        {
                            targetsFound++;
                            map[row, col] = 0;
                        }
                    }
                    Console.WriteLine($"Разведчик {scoutId} обнаружил цель на ({row}, {col}). Всего найдено: {targetsFound}");
                }
                Thread.Sleep(200);
                row += direction.dr;
                col += direction.dc;
            }
            Console.WriteLine($"Разведчик {scoutId} завершил обход. Обнаружено целей: {targetsFound}");
        }
    }
}
