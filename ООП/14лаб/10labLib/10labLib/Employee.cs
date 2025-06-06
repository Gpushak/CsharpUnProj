using System;

namespace _10labLib
{
    public class Employee
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }

        private static readonly string[] SampleNames = {
            "Alice", "Bob", "Charlie", "Diana", "Ethan",
            "Fiona", "George", "Hannah", "Ian", "Julia"
        };

        private static readonly string[] SamplePositions = {
            "Operator", "Technician", "Supervisor", "Administrator", "Clerk"
        };

        public Employee() { }

        public Employee(string name, string position, decimal salary)
        {
            Name = name;
            Position = position;
            Salary = salary;
        }

        public Employee(Employee other)
        {
            Name = other.Name;
            Position = other.Position;
            Salary = other.Salary;
        }

        
        public virtual void Show()
        {
            Console.WriteLine($"Name:     {Name}");
            Console.WriteLine($"Position: {Position}");
            Console.WriteLine($"Salary:   {Salary:C}");
        }

        
        public virtual void Init()
        {
            Console.Write("Enter Name: ");
            Name = Console.ReadLine();

            Console.Write("Enter Position: ");
            Position = Console.ReadLine();

            Console.Write("Enter Salary: ");
            Salary = decimal.Parse(Console.ReadLine());
        }

        
        public virtual void RandomInit()
        {
            var rnd = new Random();
            Name = SampleNames[rnd.Next(SampleNames.Length)];
            Position = SamplePositions[rnd.Next(SamplePositions.Length)];
            Salary = Math.Round((decimal)(rnd.NextDouble() * 70_000 + 30_000), 2);
        }

        public override bool Equals(object obj)
        {
            if (obj is Employee emp)
            {
                return Name == emp.Name
                    && Position == emp.Position
                    && Salary == emp.Salary;
            }
            return false;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + (Name?.GetHashCode() ?? 0);
            hash = hash * 23 + (Position?.GetHashCode() ?? 0);
            hash = hash * 23 + Salary.GetHashCode();
            return hash;
        }

    }
}
