using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linq2Css;

namespace CssDeDup
{
    public delegate bool RulePredicate(CssRule left, CssRule right);

    public static class CssRulePredicates
    {
        public static bool ExactMatch(CssRule left, CssRule right)
        {
            if (!string.Equals(left.Selector, right.Selector, StringCompareSingletons.IgnoreCaseComparison))
            {
                return false;
            }

            if (left.Properties.Count != right.Properties.Count)
            {
                return false;
            }

            foreach (CssProperty leftProp in left.Properties)
            {
                if (!right.Properties.Any(rightProp => CssPropertyPredicates.ExactMatch(leftProp, rightProp)))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool PropertiesOnlyMatch(CssRule left, CssRule right)
        {
            if (left.Properties.Count != right.Properties.Count)
            {
                return false;
            }

            foreach (CssProperty leftProp in left.Properties)
            {
                if (!right.Properties.Any(rightProp => CssPropertyPredicates.ExactMatch(leftProp, rightProp)))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
