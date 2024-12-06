using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _10labLib
{
    public class Gild : Production
    {
        public string Supervisor { get; set; }

        public Gild() { }

        public Gild(string name, int employeeCount, string supervisor)
            : base(name, employeeCount)
        {
            Supervisor = supervisor;
        }

        public Gild(Gild other) : base(other)
        {
            Supervisor = other.Supervisor;
        }

        public override void Show()
        {
            base.Show();
            Console.WriteLine($"Supervisor: {Supervisor}");
        }

        public override void Init()
        {
            base.Init();
            Console.Write("Enter Supervisor Name: ");
            Supervisor = Console.ReadLine();
        }

        public override void RandomInit()
        {
            base.RandomInit();
            Supervisor = $"Supervisor_{new Random().Next(1, 100)}";
        }

        public override bool Equals(object obj)
        {
            if (obj is Gild Gild)
                return base.Equals(Gild) && Supervisor == Gild.Supervisor;
            return false;
        }
    }
}
