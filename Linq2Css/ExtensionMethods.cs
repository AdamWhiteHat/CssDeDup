using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq2Css
{
    public static class ListExtensionMethods
    {
        public static bool RemoveMany<T>(this List<T> source, IEnumerable<T> toRemove)
        {
            bool result = true;
            foreach (T item in toRemove)
            {
                result &= source.Remove(item);
            }
            return result;
        }
    }
}
