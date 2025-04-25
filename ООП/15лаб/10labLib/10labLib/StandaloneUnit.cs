using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _10labLib
{
    public class StandaloneUnit : IInit
    {
        public string UnitName { get; set; }
        public int Capacity { get; set; }

        public void Init()
        {
            Console.Write("Enter Unit Name: ");
            UnitName = Console.ReadLine();
            Console.Write("Enter Capacity: ");
            Capacity = int.Parse(Console.ReadLine());
        }

        public void RandomInit()
        {
            var rand = new Random();
            UnitName = $"Unit_{rand.Next(1, 50)}";
            Capacity = rand.Next(10, 200);
        }

        public void Show()
        {
            Console.WriteLine($"Standalone Unit: {UnitName}, Capacity: {Capacity}");
        }
    }
}
