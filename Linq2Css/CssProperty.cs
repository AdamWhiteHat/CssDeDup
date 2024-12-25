using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using ExCSS;

namespace Linq2Css
{
    public class CssProperty
    {
        public string Name { get; set; } = "";
        public string Value { get; set; } = "";
        public bool IsImportant { get; set; } = false;
        public string GetFormattedString(int nestedLevel) => $"{StringHelper.GetIndentation(nestedLevel)}{Name}:{Value}{(IsImportant ? "!important" : "")};";
        public override string ToString() => GetFormattedString(0);
    }
}
