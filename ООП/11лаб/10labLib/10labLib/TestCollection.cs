using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _10labLib
{
    public class TestCollections
    {
        private Queue<Factory> collection1;
        private Queue<string> collection2;
        private Dictionary<Production, Factory> collection3;
        private Dictionary<string, Factory> collection4;

        public TestCollections(int numberOfElements)
        {
            collection1 = new Queue<Factory>();
            collection2 = new Queue<string>();
            collection3 = new Dictionary<Production, Factory>();
            collection4 = new Dictionary<string, Factory>();

            for (int i = 0; i < numberOfElements; i++)
            {
                var factory = new Factory($"Factory_{i}", i * 10, new Random().Next(1, 10));
                collection1.Enqueue(factory);
                collection2.Enqueue(factory.ToString());
                collection3.Add(new Production($"Production_{i}", i * 10), factory);
                collection4.Add($"Key_{i}", factory);
            }
        }

        public void MeasureSearchTimes()
        {
            var firstElement = collection1.Peek();
            var centralElement = collection1.ToArray()[collection1.Count / 2];
            var lastElement = collection1.ToArray()[collection1.Count - 1];
            var nonExistentElement = new Factory("Non-existent", 0, 0);

            // Измерение времени для collection1 (Queue<Factory>)
            var start = DateTime.Now;
            bool foundFirst = collection1.Contains(firstElement);
            var timeFirst = DateTime.Now - start;

            start = DateTime.Now;
            bool foundCentral = collection1.Contains(centralElement);
            var timeCentral = DateTime.Now - start;

            start = DateTime.Now;
            bool foundLast = collection1.Contains(lastElement);
            var timeLast = DateTime.Now - start;

            start = DateTime.Now;
            bool foundNonExistent = collection1.Contains(nonExistentElement);
            var timeNonExistent = DateTime.Now - start;

            // Измерение времени для collection2 (Queue<string>)
            start = DateTime.Now;
            bool foundFirstString = collection2.Contains(firstElement.ToString());
            var timeFirstString = DateTime.Now - start;

            start = DateTime.Now;
            bool foundCentralString = collection2.Contains(centralElement.ToString());
            var timeCentralString = DateTime.Now - start;

            start = DateTime.Now;
            bool foundLastString = collection2.Contains(lastElement.ToString());
            var timeLastString = DateTime.Now - start;

            start = DateTime.Now;
            bool foundNonExistentString = collection2.Contains("Non-existent");
            var timeNonExistentString = DateTime.Now - start;

            // Измерение времени для collection3 (Dictionary<Production, Factory>)
            start = DateTime.Now;
            bool foundKeyFirst = collection3.ContainsKey(new Production($"Production_0", 0));
            var timeKeyFirst = DateTime.Now - start;

            start = DateTime.Now;
            bool foundKeyCentral = collection3.ContainsKey(new Production($"Production_{collection3.Count / 2}", 0));
            var timeKeyCentral = DateTime.Now - start;

            start = DateTime.Now;
            bool foundKeyLast = collection3.ContainsKey(new Production($"Production_{collection3.Count - 1}", 0));
            var timeKeyLast = DateTime.Now - start;

            start = DateTime.Now;
            bool foundKeyNonExistent = collection3.ContainsKey(new Production("Non-existent", 0));
            var timeKeyNonExistent = DateTime.Now - start;

            // Измерение времени для collection4 (Dictionary<string, Factory>)
            start = DateTime.Now;
            bool foundKeyFirstString = collection4.ContainsKey("Key_0");
            var timeKeyFirstString = DateTime.Now - start;

            start = DateTime.Now;
            bool foundKeyCentralString = collection4.ContainsKey($"Key_{collection4.Count / 2}");
            var timeKeyCentralString = DateTime.Now - start;

            start = DateTime.Now;
            bool foundKeyLastString = collection4.ContainsKey($"Key_{collection4.Count - 1}");
            var timeKeyLastString = DateTime.Now - start;

            start = DateTime.Now;
            bool foundKeyNonExistentString = collection4.ContainsKey("Non-existent");
            var timeKeyNonExistentString = DateTime.Now - start;

            // Вывод результатов
            Console.WriteLine($"Время поиска первого элемента (Queue<Factory>): {timeFirst.TotalMilliseconds} ms");
            Console.WriteLine($"Время поиска центрального элемента (Queue<Factory>): {timeCentral.TotalMilliseconds} ms");
            Console.WriteLine($"Время поиска последнего элемента (Queue<Factory>): {timeLast.TotalMilliseconds} ms");
            Console.WriteLine($"Время поиска несуществующего элемента (Queue<Factory>): {timeNonExistent.TotalMilliseconds} ms");

            Console.WriteLine($"Время поиска первого элемента (Queue<string>): {timeFirstString.TotalMilliseconds} ms");
            Console.WriteLine($"Время поиска центрального элемента (Queue<string>): {timeCentralString.TotalMilliseconds} ms");
            Console.WriteLine($"Время поиска последнего элемента (Queue<string>): {timeLastString.TotalMilliseconds} ms");
            Console.WriteLine($"Время поиска несуществующего элемента (Queue<string>): {timeNonExistentString.TotalMilliseconds} ms");

            Console.WriteLine($"Время поиска первого ключа (Dictionary<Production, Factory>): {timeKeyFirst.TotalMilliseconds} ms");
            Console.WriteLine($"Время поиска центрального ключа(Dictionary<Production, Factory>): {timeKeyCentral.TotalMilliseconds} ms");
            Console.WriteLine($"Время последнего поиска ключа (Dictionary<Production, Factory>): {timeKeyLast.TotalMilliseconds} ms");
            Console.WriteLine($"Несуществующее время поиска ключа (Dictionary<Production, Factory>): {timeKeyNonExistent.TotalMilliseconds} ms");

            Console.WriteLine($"Время поиска первого ключа (Dictionary<string, Factory>): {timeKeyFirstString.TotalMilliseconds} ms");
            Console.WriteLine($"Время поиска центрального ключа (Dictionary<string, Factory>): {timeKeyCentralString.TotalMilliseconds} ms");
            Console.WriteLine($"Время последнего поиска ключа (Dictionary<string, Factory>): {timeKeyLastString.TotalMilliseconds} ms");
            Console.WriteLine($"Несуществующее время поиска ключа (Dictionary<string, Factory>): {timeKeyNonExistentString.TotalMilliseconds} ms");
        }
    }
}
