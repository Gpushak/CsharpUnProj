using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _10labLib
{
    public class Factory : Production
    {
        public int GildCount { get; set; }

        public Factory() { }

        public Factory(string name, int employeeCount, int GildCount)
            : base(name, employeeCount)
        {
            GildCount = GildCount;
        }

        public Factory(Factory other) : base(other)
        {
            GildCount = other.GildCount;
        }

        public override void Show()
        {
            base.Show();
            Console.WriteLine($"Gilds: {GildCount}");
        }

        public override void Init()
        {
            base.Init();
            Console.Write("Enter Gild Count: ");
            GildCount = int.Parse(Console.ReadLine());
        }

        public override void RandomInit()
        {
            base.RandomInit();
            GildCount = new Random().Next(1, 10);
        }

        public override bool Equals(object obj)
        {
            if (obj is Factory factory)
                return base.Equals(factory) && GildCount == factory.GildCount;
            return false;
        }
    }
}
