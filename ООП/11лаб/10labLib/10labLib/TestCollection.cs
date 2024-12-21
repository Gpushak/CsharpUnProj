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
        public Queue<Gild> Collection1 { get; private set; }
        public Queue<string> Collection2 { get; private set; }
        public Dictionary<Production, Gild> Collection3 { get; private set; }
        public Dictionary<string, Gild> Collection4 { get; private set; }

        public TestCollections(int count)
        {
            Collection1 = new Queue<Gild>();
            Collection2 = new Queue<string>();
            Collection3 = new Dictionary<Production, Gild>();
            Collection4 = new Dictionary<string, Gild>();

            for (int i = 0; i < count; i++)
            {
                var gild = new Gild($"Gild {i}", 100 + i, 5, $"Supervisor {i}");
                Collection1.Enqueue(gild);
                Collection2.Enqueue(gild.ToString());
                Collection3.Add(gild.BaseProduction, gild);
                Collection4.Add(gild.Name, gild);
            }
        }

        public void AddToCollection1(Gild gild)
        {
            Collection1.Enqueue(gild);
        }

        public void RemoveFromCollection1()
        {
            if (Collection1.Count > 0)
                Collection1.Dequeue();
        }

        public void MeasureSearchTime()
        {
            var firstElement = Collection1.Peek();
            var centralElement = Collection1.ToArray()[Collection1.Count / 2];
            var lastElement = Collection1.ToArray()[Collection1.Count - 1];
            var nonExistentElement = new Gild("NonExistent", 0, 0, "None");

            MeasureContains(Collection1, firstElement, centralElement, lastElement, nonExistentElement);
            MeasureContains(Collection2, firstElement.ToString(), centralElement.ToString(), lastElement.ToString(), "NonExistent");
            MeasureContainsKey(Collection3, firstElement.BaseProduction, centralElement.BaseProduction, lastElement.BaseProduction, new Production("NonExistent", 0));
            MeasureContainsKey(Collection4, firstElement.Name, centralElement.Name, lastElement.Name, "NonExistent");
        }

        private void MeasureContains(Queue<Gild> collection, Gild first, Gild central, Gild last, Gild nonExistent)
        {
            Stopwatch stopwatch = new Stopwatch();

            // Measure for first element
            stopwatch.Start();
            bool containsFirst = collection.Contains(first);
            stopwatch.Stop();
            Console.WriteLine($"Contains first element: {containsFirst}, Time: {stopwatch.ElapsedTicks} ticks");

            // Measure for central element
            stopwatch.Restart();
            bool containsCentral = collection.Contains(central);
            stopwatch.Stop();
            Console.WriteLine($"Contains central element: {containsCentral}, Time: {stopwatch.ElapsedTicks} ticks");

            // Measure for last element
            stopwatch.Restart();
            bool containsLast = collection.Contains(last);
            stopwatch.Stop();
            Console.WriteLine($"Contains last element: {containsLast}, Time: {stopwatch.ElapsedTicks} ticks");

            // Measure for non-existent element
            stopwatch.Restart();
            bool containsNonExistent = collection.Contains(nonExistent);
            stopwatch.Stop();
            Console.WriteLine($"Contains non-existent element: {containsNonExistent}, Time: {stopwatch.ElapsedTicks} ticks");
        }

        private void MeasureContains(Dictionary<Production, Gild> collection, Production first, Production central, Production last, Production nonExistent)
        {
            Stopwatch stopwatch = new Stopwatch();

            // Measure for first element
            stopwatch.Start();
            bool containsKeyFirst = collection.ContainsKey(first);
            stopwatch.Stop();
            Console.WriteLine($"Contains key for first element: {containsKeyFirst}, Time: {stopwatch.ElapsedTicks} ticks");

            // Measure for central element
            stopwatch.Restart();
            bool containsKeyCentral = collection.ContainsKey(central);
            stopwatch.Stop();
            Console.WriteLine($"Contains key for central element: {containsKeyCentral}, Time: {stopwatch.ElapsedTicks} ticks");

            // Measure for last element
            stopwatch.Restart();
            bool containsKeyLast = collection.ContainsKey(last);
            stopwatch.Stop();
            Console.WriteLine($"Contains key for last element: {containsKeyLast}, Time: {stopwatch.ElapsedTicks} ticks");

            // Measure for non-existent element
            stopwatch.Restart();
            bool containsKeyNonExistent = collection.ContainsKey(nonExistent);
            stopwatch.Stop();
            Console.WriteLine($"Contains key for non-existent element: {containsKeyNonExistent}, Time: {stopwatch.ElapsedTicks} ticks");
        }
    }
}
