using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using ExCSS;

namespace Linq2Css
{
    public class CssNamespace
    {
        public string Prefix { get; set; } = "";
        public string NamespaceUri { get; set; } = "";
        public string GetFormattedString(int nestedLevel) => $"{StringHelper.GetIndentation(nestedLevel)}@namespace{(!string.IsNullOrWhiteSpace(Prefix) ? " " : "")}{Prefix} url({NamespaceUri});";
        public override string ToString() => GetFormattedString(0);
    }
}
