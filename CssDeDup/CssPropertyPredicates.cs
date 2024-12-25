using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linq2Css;

namespace CssDeDup
{
    public delegate bool PropertyPredicate(CssProperty left, CssProperty right);

    public static class CssPropertyPredicates
    {
        public static bool ExactMatch(CssProperty left, CssProperty right)
        {
            if (!string.Equals(left.Name, right.Name, StringCompareSingletons.IgnoreCaseComparison))
            {
                return false;
            }
            if (!string.Equals(left.Value, right.Value, StringCompareSingletons.IgnoreCaseComparison))
            {
                return false;
            }
            if (left.IsImportant != right.IsImportant)
            {
                return false;
            }
            return true;
        }
    }
}
