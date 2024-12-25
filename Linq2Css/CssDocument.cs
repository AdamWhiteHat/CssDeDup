using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using ExCSS;

namespace Linq2Css
{
    public partial class CssDocument
    {
        public CssCharset Charset { get; set; } = null;
        public List<CssImport> Imports { get; set; } = new List<CssImport>();
        public List<CssNamespace> Namespaces { get; set; } = new List<CssNamespace>();
        public List<CssMediaRule> MediaRules { get; set; } = new List<CssMediaRule>();
        public List<CssPageRule> PageRules { get; set; } = new List<CssPageRule>();
        public List<CssContainerRule> ContainerRules { get; set; } = new List<CssContainerRule>();
        public List<CssRule> StyleRules { get; set; } = new List<CssRule>();

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (Charset != null)
            {
                sb.AppendLine(Charset.GetFormattedString(0));
                sb.AppendLine();
            }
            if (Imports.Any())
            {
                sb.AppendLine(string.Join(Environment.NewLine + Environment.NewLine, Imports.Select(i => i.GetFormattedString(0))));
                sb.AppendLine();
            }
            if (Namespaces.Any())
            {
                sb.AppendLine(string.Join(Environment.NewLine + Environment.NewLine, Namespaces.Select(ns => ns.GetFormattedString(0))));
                sb.AppendLine();
            }
            if (MediaRules.Any())
            {
                sb.AppendLine(string.Join(Environment.NewLine + Environment.NewLine, MediaRules.Select(mr => mr.GetFormattedString(0))));
                sb.AppendLine();
            }
            if (PageRules.Any())
            {
                sb.AppendLine(string.Join(Environment.NewLine + Environment.NewLine, PageRules.Select(pr => pr.GetFormattedString(0))));
                sb.AppendLine();
            }
            if (ContainerRules.Any())
            {
                sb.AppendLine(string.Join(Environment.NewLine + Environment.NewLine, ContainerRules.Select(pr => pr.GetFormattedString(0))));
                sb.AppendLine();
            }
            if (StyleRules.Any())
            {
                sb.AppendLine(string.Join(Environment.NewLine + Environment.NewLine, StyleRules.Select(sr => sr.GetFormattedString(0))));
                sb.AppendLine();
            }


            return sb.ToString();
        }
    }
}
