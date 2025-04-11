using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _10labLib
{
    public class Workshop : Production
    {
        public List<string> Equipment { get; set; }

        public Workshop()
        {
            Equipment = new List<string>();
        }

        public Workshop(string name, int employeeCount, List<string> equipment)
            : base(name, employeeCount)
        {
            Equipment = equipment ?? new List<string>();
        }

        public Workshop(Workshop other) : base(other)
        {
            Equipment = new List<string>(other.Equipment);
        }

        public override void Show()
        {
            base.Show();
            Console.WriteLine("Equipment:");
            foreach (var item in Equipment)
            {
                Console.WriteLine($"- {item}");
            }
        }

        public override void Init()
        {
            base.Init();
            Console.Write("Enter number of equipment items: ");
            int count = int.Parse(Console.ReadLine());
            Equipment.Clear();
            for (int i = 0; i < count; i++)
            {
                Console.Write($"Enter equipment {i + 1}: ");
                Equipment.Add(Console.ReadLine());
            }
        }

        public override void RandomInit()
        {
            base.RandomInit();
            var rand = new Random();
            Equipment.Clear();
            int count = rand.Next(1, 5);
            for (int i = 0; i < count; i++)
            {
                Equipment.Add($"Equipment_{rand.Next(1, 100)}");
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Workshop workshop)
            {
                return base.Equals(workshop) && Equipment.SequenceEqual(workshop.Equipment);
            }
            return false;
        }
    }
}
