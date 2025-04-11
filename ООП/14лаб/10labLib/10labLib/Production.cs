using System;
using System.Collections.Generic;
using System.Linq;

namespace _10labLib
{
    public class Production : IInit, IComparable<Production>, ICloneable
    {
        public string Name { get; set; }
        public int EmployeeCount { get; set; }

        public Production() { }

        public Production(string name, int employeeCount)
        {
            Name = name;
            EmployeeCount = employeeCount;
        }

        public Production(Production other)
        {
            Name = other.Name;
            EmployeeCount = other.EmployeeCount;
        }

        public virtual void Show()
        {
            Console.WriteLine($"Production: {Name}, Employees: {EmployeeCount}");
        }

        public virtual void Init()
        {
            Console.Write("Enter Name: ");
            Name = Console.ReadLine();
            Console.Write("Enter Employee Count: ");
            EmployeeCount = int.Parse(Console.ReadLine());
        }

        public virtual void RandomInit()
        {
            var rand = new Random();
            Name = $"Production_{rand.Next(1, 100)}";
            EmployeeCount = rand.Next(5, 100);
        }

        public override bool Equals(object obj)
        {
            if (obj is Production production)
                return Name == production.Name && EmployeeCount == production.EmployeeCount;
            return false;
        }
        public int CompareTo(Production other)
        {
            if (other == null) return 1;
            return EmployeeCount.CompareTo(other.EmployeeCount);
        }

        public static Production BinarySearchByName(Production[] productions, string name)
        {
            var comparer = new ProductionNameComparer();
            Array.Sort(productions, comparer);
            int index = Array.BinarySearch(productions, new Production { Name = name }, comparer);
            return index >= 0 ? productions[index] : null;
        }

        public object Clone()
        {
            return new Production(Name, EmployeeCount);
        }

        public Production ShallowCopy()
        {
            return (Production)MemberwiseClone();
        }

    }
}
