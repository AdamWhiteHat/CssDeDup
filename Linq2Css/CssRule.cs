using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using ExCSS;

namespace Linq2Css
{
    public class CssRule
    {
        public string Selector { get; set; } = "";
        public List<CssProperty> Properties { get; set; } = new List<CssProperty>();
        public virtual string GetFormattedString(int nestedLevel) => $"{StringHelper.GetIndentation(nestedLevel)}{Selector} {{\r\n{string.Join(Environment.NewLine, Properties.Select(p => p.GetFormattedString(nestedLevel + 1)))}\r\n{StringHelper.GetIndentation(nestedLevel)}}}";
        public override string ToString() => GetFormattedString(0);
    }
}
