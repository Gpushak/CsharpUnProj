using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _10labLib
{
    public class ProductionNameComparer : IComparer<Production>
    {
        public int Compare(Production x, Production y)
        {
            return string.Compare(x?.Name, y?.Name, StringComparison.Ordinal);
        }
    }
}
