using System;

namespace ProductionHierarchy
{
    public class Production
    {
        public string Name { get; set; }
        public int Employees { get; set; }
        public double Area { get; set; }

        public Production() { }

        public Production(string name, int employees, double area)
        {
            Name = name;
            Employees = employees > 0 ? employees : throw new ArgumentException("Количество сотрудников должно быть больше 0.");
            Area = area > 0 ? area : throw new ArgumentException("Площадь должна быть больше 0.");
        }

        public Production(Production other)
        {
            Name = other.Name;
            Employees = other.Employees;
            Area = other.Area;
        }

        public virtual void Show()
        {
            Console.WriteLine($"Production: {Name}, Employees: {Employees}, Area: {Area} sq.m.");
        }

        public virtual void Init()
        {
            Console.Write("Введите название: ");
            Name = Console.ReadLine();
            Console.Write("Введите количество сотрудников: ");
            Employees = int.Parse(Console.ReadLine());
            Console.Write("Введите площадь: ");
            Area = double.Parse(Console.ReadLine());
        }

        public virtual void RandomInit()
        {
            Random rnd = new Random();
            Name = "Production" + rnd.Next(1, 100);
            Employees = rnd.Next(10, 500);
            Area = rnd.Next(100, 1000);
        }

        public override bool Equals(object obj)
        {
            if (obj is Production other)
            {
                return Name == other.Name && Employees == other.Employees && Math.Abs(Area - other.Area) < 0.01;
            }
            return false;
        }
    }

    public class Factory : Production
    {
        public int Machines { get; set; }

        public Factory() { }

        public Factory(string name, int employees, double area, int machines) : base(name, employees, area)
        {
            Machines = machines > 0 ? machines : throw new ArgumentException("Количество станков должно быть больше 0.");
        }

        public override void Show()
        {
            base.Show();
            Console.WriteLine($"Machines: {Machines}");
        }

        public override void Init()
        {
            base.Init();
            Console.Write("Введите количество станков: ");
            Machines = int.Parse(Console.ReadLine());
        }

        public override void RandomInit()
        {
            base.RandomInit();
            Random rnd = new Random();
            Machines = rnd.Next(1, 100);
        }
    }

    public class Workshop : Production
    {
        public int Projects { get; set; }

        public Workshop() { }

        public Workshop(string name, int employees, double area, int projects) : base(name, employees, area)
        {
            Projects = projects > 0 ? projects : throw new ArgumentException("Количество проектов должно быть больше 0.");
        }

        public override void Show()
        {
            base.Show();
            Console.WriteLine($"Projects: {Projects}");
        }

        public override void Init()
        {
            base.Init();
            Console.Write("Введите количество проектов: ");
            Projects = int.Parse(Console.ReadLine());
        }

        public override void RandomInit()
        {
            base.RandomInit();
            Random rnd = new Random();
            Projects = rnd.Next(1, 50);
        }
    }

    public class ShopFloor : Production
    {
        public int Stations { get; set; }

        public ShopFloor() { }

        public ShopFloor(string name, int employees, double area, int stations) : base(name, employees, area)
        {
            Stations = stations > 0 ? stations : throw new ArgumentException("Количество станций должно быть больше 0.");
        }

        public override void Show()
        {
            base.Show();
            Console.WriteLine($"Stations: {Stations}");
        }

        public override void Init()
        {
            base.Init();
            Console.Write("Введите количество станций: ");
            Stations = int.Parse(Console.ReadLine());
        }

        public override void RandomInit()
        {
            base.RandomInit();
            Random rnd = new Random();
            Stations = rnd.Next(1, 20);
        }
    }
}
