using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _10labLib
{
    public static class Queries
    {
        public static void GetGildNamesOnFactory(Production[] productions, string factoryName)
        {
            foreach (var prod in productions)
            {
                if (prod is Factory factory && factory.Name == factoryName)
                {
                    Console.WriteLine($"Factory: {factory.Name}, Gilds: {factory.GildCount}");
                }
            }
        }

        public static void GetSupervisorsOnGild(Production[] productions, string GildName)
        {
            foreach (var prod in productions)
            {
                if (prod is Gild Gild && Gild.Name == GildName)
                {
                    Console.WriteLine($"Gild: {Gild.Name}, Supervisor: {Gild.Supervisor}");
                }
            }
        }

        public static int GetEmployeeCountOnFactory(Production[] productions)
        {
            int totalEmployees = 0;
            foreach (var prod in productions)
            {
                if (prod is Factory factory)
                    totalEmployees += factory.EmployeeCount;
            }
            return totalEmployees;
        }
    }
}
