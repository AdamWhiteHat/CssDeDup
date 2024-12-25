using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using ExCSS;

namespace Linq2Css
{
    public class CssImport
    {
        public string Href { get; set; } = "";
        public string GetFormattedString(int nestedLevel) => $"{StringHelper.GetIndentation(nestedLevel)}@import url(\"{Href}\");";
        public override string ToString() => GetFormattedString(0);
    }
}
