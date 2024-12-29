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
        public static class Factory
        {
            public static CssDocument ParseCssFile(string filePath)
            {
                StylesheetParser parser = new StylesheetParser();
                string cssText = File.ReadAllText(filePath);
                Stylesheet cssSheet = parser.Parse(cssText);

                CssDocument outDocument = new CssDocument();

                outDocument.Charset = GetCharset(cssSheet);

                outDocument.Imports = GetImports(cssSheet);

                outDocument.Namespaces = GetNamespaces(cssSheet);

                List<CssMediaRule> outMediaRules = GetMediaRules(cssSheet);
                if (outMediaRules.Any())
                {
                    outDocument.MediaRules = outMediaRules;
                }

                List<CssPageRule> outPageRules = GetPageRules(cssSheet);
                if (outPageRules.Any())
                {
                    outDocument.PageRules = outPageRules;
                }

                List<CssContainerRule> outContainerRules = GetContainerRules(cssSheet);
                if (outContainerRules.Any())
                {
                    outDocument.ContainerRules = outContainerRules;
                }

                List<CssRule> outStyleRules = GetCssRules(cssSheet);
                if (outStyleRules.Any())
                {
                    outDocument.StyleRules = outStyleRules;
                }

                return outDocument;
            }

            private static CssCharset GetCharset(ExCSS.Stylesheet source)
            {
                if (source.CharacterSetRules.Any())
                {
                    var charsetRule = source.CharacterSetRules.Single();

                    if (!string.IsNullOrWhiteSpace(charsetRule.CharacterSet))
                    {
                        CssCharset newCharset = new CssCharset()
                        {
                            CharacterSet = charsetRule.CharacterSet
                        };
                        return newCharset;
                    }
                }
                return null;
            }

            private static List<CssImport> GetImports(ExCSS.Stylesheet source)
            {
                List<CssImport> results = new List<CssImport>();
                if (source.ImportRules.Any())
                {
                    foreach (var importRule in source.ImportRules.ToList())
                    {
                        if (!string.IsNullOrWhiteSpace(importRule.Href))
                        {
                            CssImport newImport = new CssImport()
                            {
                                Href = importRule.Href
                            };
                            results.Add(newImport);
                        }
                    }
                }
                return results;
            }

            private static List<CssNamespace> GetNamespaces(ExCSS.Stylesheet source)
            {
                List<CssNamespace> results = new List<CssNamespace>();
                if (source.NamespaceRules.Any())
                {
                    foreach (var namespaceRule in source.NamespaceRules.ToList())
                    {
                        if (!string.IsNullOrWhiteSpace(namespaceRule.NamespaceUri))
                        {
                            CssNamespace newNamespace = new CssNamespace()
                            {
                                NamespaceUri = namespaceRule.NamespaceUri
                            };

                            if (!string.IsNullOrWhiteSpace(namespaceRule.Prefix))
                            {
                                newNamespace.Prefix = namespaceRule.Prefix;
                            }
                            results.Add(newNamespace);
                        }
                    }
                }
                return results;
            }

            private static List<CssMediaRule> GetMediaRules(ExCSS.Stylesheet source)
            {
                List<CssMediaRule> results = new List<CssMediaRule>();

                if (source.MediaRules.Any())
                {

                    var mediaStyleRules = source.MediaRules.Select(mr => mr.Children.ToList()).ToList();
                    foreach (var mediaRule in mediaStyleRules)
                    {
                        CssMediaRule newMedia = new CssMediaRule();

                        foreach (var childRule in mediaRule)
                        {
                            if (childRule.GetType() == typeof(ExCSS.MediaList))
                            {
                                ExCSS.MediaList mediaList = childRule as ExCSS.MediaList;
                                if (mediaList != null)
                                {
                                    newMedia.Constraints = mediaList.MediaText;
                                }
                            }
                            else if (childRule.GetType() == typeof(ExCSS.StyleRule))
                            {
                                IStyleRule styleRule = childRule as IStyleRule;
                                if (styleRule != null)
                                {
                                    CssRule newRule = GetCssRule(styleRule);
                                    if (newRule != null)
                                    {
                                        newMedia.Rules.Add(newRule);
                                    }
                                }
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(newMedia.Constraints) && newMedia.Rules.Any())
                        {
                            results.Add(newMedia);
                        }
                    }
                }

                return results;
            }

            private static List<CssPageRule> GetPageRules(ExCSS.Stylesheet source)
            {
                List<CssPageRule> results = new List<CssPageRule>();

                if (source.PageRules.Any())
                {
                    foreach (var pageRule in source.PageRules)
                    {
                        if (!string.IsNullOrWhiteSpace(pageRule.SelectorText))
                        {
                            List<CssProperty> newProperties = GetProperties(pageRule.Style);

                            if (newProperties.Any() && !string.IsNullOrWhiteSpace(pageRule.SelectorText))
                            {
                                CssPageRule newPage = new CssPageRule()
                                {
                                    Selector = pageRule.SelectorText,
                                    Properties = newProperties
                                };
                                results.Add(newPage);
                            }
                        }
                    }
                }

                return results;
            }

            private static List<CssContainerRule> GetContainerRules(ExCSS.Stylesheet source)
            {
                List<CssContainerRule> results = new List<CssContainerRule>();

                if (source.ContainerRules.Any())
                {
                    foreach (var containerRule in source.ContainerRules)
                    {
                        CssContainerRule newContainer = new CssContainerRule()
                        {
                            Name = containerRule.Name,
                            Conditions = containerRule.ConditionText,
                            Rules = GetCssRules(containerRule.Rules)
                        };

                        if (!string.IsNullOrWhiteSpace(newContainer.Name) &&
                            !string.IsNullOrWhiteSpace(newContainer.Conditions) &&
                            newContainer.Rules.Any())
                        {
                            results.Add(newContainer);
                        }
                    }
                }

                return results;
            }

            private static List<CssRule> GetCssRules(ExCSS.Stylesheet source)
            {
                List<CssRule> results = new List<CssRule>();

                List<IStyleRule> styleRules = source.StyleRules.ToList();
                if (styleRules.Any())
                {
                    foreach (var styleRule in styleRules)
                    {
                        CssRule newRule = GetCssRule(styleRule);
                        if (newRule != null)
                        {
                            results.Add(newRule);
                        }
                    }
                }

                return results;
            }

            private static List<CssRule> GetCssRules(IRuleList source)
            {
                List<CssRule> results = new List<CssRule>();

                List<IRule> ruleList = source.ToList();
                if (ruleList.Any())
                {
                    foreach (var iRule in ruleList)
                    {
                        if (iRule.Type == RuleType.Style)
                        {
                            IStyleRule styleRule = iRule as IStyleRule;
                            if (styleRule != null)
                            {
                                CssRule newRule = GetCssRule(styleRule);
                                if (newRule != null)
                                {
                                    results.Add(newRule);
                                }
                            }
                        }
                    }
                }

                return results;
            }

            private static CssRule GetCssRule(ExCSS.IStyleRule source)
            {
                List<CssProperty> newProperties = GetProperties(source.Style);

                if (newProperties.Any() && !string.IsNullOrWhiteSpace(source.SelectorText))
                {
                    CssRule result = new CssRule()
                    {
                        Selector = source.SelectorText,
                        Properties = newProperties
                    };
                    return result;
                }

                return null;
            }

            private static List<CssProperty> GetProperties(ExCSS.StyleDeclaration source)
            {
                List<Property> properties = source.Select(prop => ((Property)prop))
                                    .Where(p => !p.IsInitial)
                                    .ToList();

                List<CssProperty> results = new List<CssProperty>();
                if (properties.Any())
                {
                    foreach (Property prop in properties)
                    {
                        if (!string.IsNullOrWhiteSpace(prop.Name) && !string.IsNullOrWhiteSpace(prop.Value))
                        {
                            CssProperty newProperty = new CssProperty()
                            {
                                Name = prop.Name,
                                Value = ConvertRgbColorToHexColor(prop.Value),
                                IsImportant = prop.IsImportant
                            };
                            results.Add(newProperty);
                        }
                    }
                }

                return results;
            }

            private static string ConvertRgbColorToHexColor(string rgbColor)
            {
                // rgb(255, 255, 255)
                if (string.IsNullOrWhiteSpace(rgbColor) || !rgbColor.Contains(" rgb("))
                {
                    return rgbColor;
                }

                int startIndex = rgbColor.IndexOf("rgb(");

                string startString = rgbColor.Substring(0, startIndex);
                string rgbString = rgbColor.Substring(startIndex);

                string toParse = rgbString.Replace(" ", "").Replace("rgb(", "").Replace(")", "");
                string[] valueStrings = toParse.Split(',');
                if (valueStrings.Length != 3)
                {
                    return rgbColor;
                }
                string convertedRgbString = $"#{string.Join("", valueStrings.Select(vs => byte.Parse(vs).ToString("X2")))}";
                return startString + convertedRgbString;
            }
        }
    }
}
