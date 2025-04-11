using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using _10labLib;

namespace ProductionApp
{
    using _10labLib;
    using MyCollections;

    class Program
    {
        // Обобщённая коллекция: ключ – название корпорации, значение – список филиалов (каждый филиал – коллекция Production)
        static SortedDictionary<string, List<Production>> corporationCollection = new SortedDictionary<string, List<Production>>();

        static void Main(string[] args)
        {
            // Заполнение коллекции данными
            FillCollections();

            // Демонстрация запросов – каждый запрос выполняется двумя способами
            Console.WriteLine("------ Запрос 1: Выборка данных ------");
            QuerySelectionUsingLINQ("Corporation_1");
            QuerySelectionUsingMethods("Corporation_1");

            Console.WriteLine("------ Запрос 2: Получение счетчика ------");
            QueryCountUsingLINQ(50);
            QueryCountUsingMethods(50);

            Console.WriteLine("------ Запрос 3: Операции над множествами ------");
            QuerySetOperationsUsingLINQ();
            QuerySetOperationsUsingMethods();

            Console.WriteLine("------ Запрос 4: Агрегирование данных ------");
            QueryAggregationUsingLINQ();
            QueryAggregationUsingMethods();

            Console.WriteLine("------ Запрос 5: Группировка данных ------");
            QueryGroupingUsingLINQ();
            QueryGroupingUsingMethods();

            // ===================================
            // Часть 2: Работа с MyNewCollection (двунаправленный список)
            // ===================================
            Console.WriteLine("\n------ Часть 2: MyNewCollection и методы расширения ------");
            // Пример заполнения коллекции
            MyNewCollection<int> intCollection = new MyNewCollection<int>();
            intCollection.Add(10);
            intCollection.Add(5);
            intCollection.Add(15);
            intCollection.Add(20);
            intCollection.Add(3);

            // a) Выборка данных по условию (например, выбрать четные числа)
            var filtered = intCollection.WhereEx(x => x % 2 == 0);
            Console.WriteLine("Элементы, удовлетворяющие условию (четные): " + string.Join(", ", filtered));

            // b) Агрегирование данных (сумма, минимум, максимум, среднее)
            Console.WriteLine("Сумма: " + intCollection.AggregateEx((x, y) => x + y));
            Console.WriteLine("Минимум: " + intCollection.Min());
            Console.WriteLine("Максимум: " + intCollection.Max());
            Console.WriteLine("Среднее: " + intCollection.Average());

            // c) Сортировка коллекции (по возрастанию)
            var sortedAsc = intCollection.SortEx(true);
            Console.WriteLine("Отсортировано по возрастанию: " + string.Join(", ", sortedAsc));
            // сортировка по убыванию
            var sortedDesc = intCollection.SortEx(false);
            Console.WriteLine("Отсортировано по убыванию: " + string.Join(", ", sortedDesc));

            Console.ReadLine();
        }

        // Метод заполнения коллекции корпораций и филиалов данными
        static void FillCollections()
        {
            // Для примера создаём две корпорации, каждая с несколькими филиалами,
            // где в филиалах хранятся объекты разных типов (Factory, Gild, Workshop)

            for (int corp = 1; corp <= 3; corp++)
            {
                List<Production> branch = new List<Production>();
                // Добавляем фабрику
                branch.Add(new Factory($"Factory_{corp}", new Random().Next(50, 150), new Random().Next(1, 5)));
                // Добавляем цех (Workshop)
                branch.Add(new Workshop($"Workshop_{corp}_A", new Random().Next(10, 100), new List<string> { "Drill", "Hammer" }));
                branch.Add(new Workshop($"Workshop_{corp}_B", new Random().Next(10, 100), new List<string> { "Saw", "Wrench" }));
                // Добавляем цех (Gild) – в данном примере для разнообразия
                branch.Add(new Gild($"Gild_{corp}", new Random().Next(5, 50), $"Supervisor_{corp}"));

                corporationCollection.Add($"Corporation_{corp}", branch);
            }
        }

        // =========================
        // Запрос 1: Выборка данных
        // Пример: Получить наименования всех цехов (Workshop) заданной корпорации
        // Реализация с использованием LINQ-запроса
        static void QuerySelectionUsingLINQ(string corpKey)
        {
            if (corporationCollection.TryGetValue(corpKey, out List<Production> branch))
            {
                var workshopNames = from p in branch
                                    where p is Workshop
                                    select p.Name;
                Console.WriteLine($"[LINQ] Цеха в {corpKey}: {string.Join(", ", workshopNames)}");
            }
        }

        // Реализация с использованием методов расширения
        static void QuerySelectionUsingMethods(string corpKey)
        {
            if (corporationCollection.TryGetValue(corpKey, out List<Production> branch))
            {
                var workshopNames = branch.Where(p => p is Workshop)
                                          .Select(p => p.Name);
                Console.WriteLine($"[Методы расширения] Цеха в {corpKey}: {string.Join(", ", workshopNames)}");
            }
        }

        // =========================
        // Запрос 2: Получение счетчика (например, количество объектов с EmployeeCount больше заданного значения)
        static void QueryCountUsingLINQ(int threshold)
        {
            int count = (from corp in corporationCollection.Values
                         from p in corp
                         where p.EmployeeCount > threshold
                         select p).Count();
            Console.WriteLine($"[LINQ] Количество объектов с числом сотрудников > {threshold}: {count}");
        }

        static void QueryCountUsingMethods(int threshold)
        {
            int count = corporationCollection.Values.SelectMany(p => p)
                                                      .Count(p => p.EmployeeCount > threshold);
            Console.WriteLine($"[Методы расширения] Количество объектов с числом сотрудников > {threshold}: {count}");
        }

        // =========================
        // Запрос 3: Операции над множествами (пересечение, объединение, разность)
        // Пример: Пусть у нас есть две выборки из разных корпораций – выполним пересечение по наименованиям объектов.
        static void QuerySetOperationsUsingLINQ()
        {
            // Для примера выбираем корпорации 1 и 2
            if (corporationCollection.TryGetValue("Corporation_1", out List<Production> branch1) &&
                corporationCollection.TryGetValue("Corporation_2", out List<Production> branch2))
            {
                var names1 = (from p in branch1 select p.Name).ToList();
                var names2 = (from p in branch2 select p.Name).ToList();
                var intersection = names1.Intersect(names2);
                Console.WriteLine("[LINQ] Пересечение наименований из Corporation_1 и Corporation_2: " + string.Join(", ", intersection));
            }
        }

        static void QuerySetOperationsUsingMethods()
        {
            if (corporationCollection.TryGetValue("Corporation_1", out List<Production> branch1) &&
                corporationCollection.TryGetValue("Corporation_2", out List<Production> branch2))
            {
                var names1 = branch1.Select(p => p.Name);
                var names2 = branch2.Select(p => p.Name);
                var union = names1.Union(names2);
                var difference = names1.Except(names2);
                Console.WriteLine("[Методы расширения] Объединение наименований: " + string.Join(", ", union));
                Console.WriteLine("[Методы расширения] Разность наименований (Corporation_1 - Corporation_2): " + string.Join(", ", difference));
            }
        }

        // =========================
        // Запрос 4: Агрегирование данных (например, суммарное количество сотрудников во всех филиалах)
        static void QueryAggregationUsingLINQ()
        {
            var totalEmployees = (from corp in corporationCollection.Values
                                  from p in corp
                                  select p.EmployeeCount).Sum();
            Console.WriteLine("[LINQ] Общее количество сотрудников: " + totalEmployees);
        }

        static void QueryAggregationUsingMethods()
        {
            var totalEmployees = corporationCollection.Values.SelectMany(p => p)
                                                               .Select(p => p.EmployeeCount)
                                                               .Sum();
            Console.WriteLine("[Методы расширения] Общее количество сотрудников: " + totalEmployees);
        }

        // =========================
        // Запрос 5: Группировка данных
        // Пример: Группировать объекты по типу
        static void QueryGroupingUsingLINQ()
        {
            var groups = from corp in corporationCollection.Values
                         from p in corp
                         group p by p.GetType().Name into g
                         select new { Type = g.Key, Count = g.Count() };

            Console.WriteLine("[LINQ] Группировка по типу объектов:");
            foreach (var group in groups)
            {
                Console.WriteLine($"  {group.Type}: {group.Count}");
            }
        }

        static void QueryGroupingUsingMethods()
        {
            var groups = corporationCollection.Values.SelectMany(p => p)
                        .GroupBy(p => p.GetType().Name)
                        .Select(g => new { Type = g.Key, Count = g.Count() });

            Console.WriteLine("[Методы расширения] Группировка по типу объектов:");
            foreach (var group in groups)
            {
                Console.WriteLine($"  {group.Type}: {group.Count}");
            }
        }
    }
}

// ===================================
// Часть 2: MyNewCollection и методы расширения
// ===================================

namespace MyCollections
{
    using System.Collections;
    using System.Collections.Generic;

    // Простой класс двунаправленного списка
    public class MyNewCollection<T> : IEnumerable<T>
    {
        // Вложенный класс узла
        public class Node
        {
            public T Data { get; set; }
            public Node Next { get; set; }
            public Node Prev { get; set; }
            public Node(T data)
            {
                Data = data;
            }
        }

        private Node head;
        private Node tail;
        public int Count { get; private set; }

        // Добавление элемента в конец списка
        public void Add(T item)
        {
            Node newNode = new Node(item);
            if (head == null)
            {
                head = newNode;
                tail = newNode;
            }
            else
            {
                tail.Next = newNode;
                newNode.Prev = tail;
                tail = newNode;
            }
            Count++;
        }

        // Реализация перечислителя для поддержки IEnumerable
        public IEnumerator<T> GetEnumerator()
        {
            Node current = head;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // Метод для получения элементов в виде списка (для удобства сортировки, преобразования и т.д.)
        public List<T> ToList()
        {
            List<T> list = new List<T>();
            foreach (var item in this)
                list.Add(item);
            return list;
        }

        // Метод очистки коллекции (если потребуется)
        public void Clear()
        {
            head = tail = null;
            Count = 0;
        }
    }

    // Методы расширения для MyNewCollection
    public static class MyNewCollectionExtensions
    {
        // a) Выборка данных по условию – аналог стандартного Where
        public static IEnumerable<T> WhereEx<T>(this MyNewCollection<T> collection, Func<T, bool> predicate)
        {
            foreach (T item in collection)
            {
                if (predicate(item))
                    yield return item;
            }
        }

        // b) Агрегирование данных: реализуем метод AggregateEx, похожий на метод Aggregate LINQ
        public static T AggregateEx<T>(this MyNewCollection<T> collection, Func<T, T, T> func)
        {
            IEnumerator<T> enumerator = collection.GetEnumerator();
            if (!enumerator.MoveNext())
                throw new InvalidOperationException("Коллекция пуста");
            T result = enumerator.Current;
            while (enumerator.MoveNext())
            {
                result = func(result, enumerator.Current);
            }
            return result;
        }

        // c) Сортировка коллекции – возвращаем отсортированный список, можно задать направление сортировки
        public static List<T> SortEx<T>(this MyNewCollection<T> collection, bool ascending = true)
        {
            List<T> list = collection.ToList();
            if (ascending)
                list.Sort();
            else
                list.Sort((x, y) => Comparer<T>.Default.Compare(y, x));
            return list;
        }
    }
}