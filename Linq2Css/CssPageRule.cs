using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using ExCSS;

namespace Linq2Css
{
    public class CssPageRule : CssRule
    {
        public override string GetFormattedString(int nestedLevel) => $"{StringHelper.GetIndentation(nestedLevel)}@page {Selector} {{\r\n{string.Join(Environment.NewLine, Properties.Select(p => p.GetFormattedString(nestedLevel + 1)))}\r\n{StringHelper.GetIndentation(nestedLevel)}}}";
        public override string ToString() => GetFormattedString(0);
    }
}
