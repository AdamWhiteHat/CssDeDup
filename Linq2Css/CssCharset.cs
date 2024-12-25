using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using ExCSS;

namespace Linq2Css
{
    public class CssCharset
    {
        public string CharacterSet { get; set; } = "";
        public string GetFormattedString(int nestedLevel) => $"{StringHelper.GetIndentation(nestedLevel)}@charset \"{CharacterSet}\";";
        public override string ToString() => GetFormattedString(0);
    }
}
