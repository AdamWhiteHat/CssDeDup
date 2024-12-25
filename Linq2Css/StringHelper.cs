using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using ExCSS;

namespace Linq2Css
{
    internal static class StringHelper
    {
        public static string GetIndentation(int nestedLevel)
        {
            return new string(Enumerable.Repeat(' ', nestedLevel * 2).ToArray());
        }
    }
}
