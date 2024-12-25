using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linq2Css;

namespace CssDeDup
{
    public static class CssRuleExtensionMethods
    {
        public static IEnumerable<Tuple<CssRule, CssRule>> PairRulesBy(this List<CssRule> source, List<CssRule> others, RulePredicate predicate)
        {
            foreach (CssRule thisRule in source)
            {
                foreach (CssRule otherRule in others)
                {
                    if (predicate.Invoke(thisRule, otherRule))
                    {
                        yield return new Tuple<CssRule, CssRule>(thisRule, otherRule);
                    }
                }
            }
        }
    }

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
