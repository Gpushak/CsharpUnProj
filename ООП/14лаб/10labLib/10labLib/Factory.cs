using System;
using System.Collections.Generic;

namespace _10labLib
{
    public class Factory : Production
    {
        public List<Gild> Gilds { get; set; } = new List<Gild>();
        public List<Employee> Employees { get; set; } = new List<Employee>();

        public int GildCount => Gilds.Count;
        public new int EmployeeCount => Employees.Count;

        public Factory() { }

        public Factory(string name, List<Gild> gilds, List<Employee> employees)
            : base(name, employees.Count)
        {
            Gilds = gilds ?? new List<Gild>();
            Employees = employees ?? new List<Employee>();
        }

        public Factory(Factory other) : base(other)
        {
            Gilds = new List<Gild>(other.Gilds);
            Employees = new List<Employee>(other.Employees);
        }

        public override void Show()
        {
            base.Show();
            Console.WriteLine($"Gilds: {GildCount}");
            Console.WriteLine($"Employees: {EmployeeCount}");
        }

        public override void Init()
        {
            base.Init();

            Console.Write("Enter number of Gilds: ");
            int gildNum = int.Parse(Console.ReadLine());
            for (int i = 0; i < gildNum; i++)
            {
                var gild = new Gild();
                gild.Init();
                Gilds.Add(gild);
            }

            Console.Write("Enter number of Employees: ");
            int empNum = int.Parse(Console.ReadLine());
            for (int i = 0; i < empNum; i++)
            {
                var emp = new Employee();
                emp.Init();
                Employees.Add(emp);
            }
        }

        public override void RandomInit()
        {
            base.RandomInit();

            var rnd = new Random();
            int gildNum = rnd.Next(1, 5);
            for (int i = 0; i < gildNum; i++)
            {
                var gild = new Gild();
                gild.RandomInit();
                Gilds.Add(gild);
            }

            int empNum = rnd.Next(5, 100);
            for (int i = 0; i < empNum; i++)
            {
                var emp = new Employee();
                emp.RandomInit();
                Employees.Add(emp);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Factory factory)
            {
                return base.Equals(factory)
                    && GildCount == factory.GildCount
                    && EmployeeCount == factory.EmployeeCount;
            }
            return false;
        }
    }
}
