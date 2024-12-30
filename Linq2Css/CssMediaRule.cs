using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using ExCSS;

namespace Linq2Css
{
    public class CssMediaRule
    {
        public string Conditions { get; set; } = "";
        public List<CssRule> Rules { get; set; } = new List<CssRule>();
        public string GetFormattedString(int nestedLevel) => $"{StringHelper.GetIndentation(nestedLevel)}@media {Conditions} {{\r\n{string.Join(Environment.NewLine + Environment.NewLine, Rules.Select(r => r.GetFormattedString(nestedLevel + 1)))}\r\n{StringHelper.GetIndentation(nestedLevel)}}}";
        public override string ToString() => GetFormattedString(0);
    }
}
