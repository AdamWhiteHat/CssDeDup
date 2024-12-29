using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ExCSS;
using Linq2Css;

namespace CssDeDup
{
    public static class Match
    {
        public static int NameAndProperties(string fileIn1, string fileIn2, string fileOut)
        {
            MatchingJob job = new MatchingJob(fileIn1, fileIn2, fileOut, CssRulePredicates.ExactMatch);
            job.Initialize();

            if (job.Matches.Count == 0)
            {
                Console.WriteLine($"No matching rules found; No action needed.");
            }
            else
            {
                List<CssRule> matchingStyleRules = job.Matches.Select(tup => tup.Item1).ToList();

                File.WriteAllText(job.File_Out3, string.Join(Environment.NewLine + Environment.NewLine, matchingStyleRules));
                Console.WriteLine($"--out :: Wrote {matchingStyleRules.Count} matching rule(s) to output file: \"{job.File_Out3}\".");

                job.RemoveMatchingRulesFromDocuments();
                job.WriteOutDocumentChanges();
            }
            Console.WriteLine($"Exiting...");
            return 0;
        }

        public static int PropertiesOnly(string fileIn1, string fileIn2, string fileOut)
        {
            MatchingJob job = new MatchingJob(fileIn1, fileIn2, fileOut, CssRulePredicates.PropertiesOnlyMatch);
            job.Initialize();

            if (job.Matches.Count == 0)
            {
                Console.WriteLine($"No matching rules found; No action needed.");
            }
            else
            {
                string beginningComment = "                          /*\r\n<MatchingPropertyPair>     */\r\n";
                string endComment = "                         /*\r\n</MatchingPropertyPair>    */\r\n";

                List<string> matchingStyleRules = job.Matches.Select(tup =>
                                                    beginningComment +
                                                    tup.Item1.GetFormattedString(1) +
                                                    Environment.NewLine +
                                                    tup.Item2.GetFormattedString(1) +
                                                    endComment
                                                ).ToList();

                File.WriteAllText(job.File_Out3, string.Join(Environment.NewLine + Environment.NewLine, matchingStyleRules));
                Console.WriteLine($"--out :: Wrote {matchingStyleRules.Count} matching rule(s) to output file: \"{job.File_Out3}\".");

                job.RemoveMatchingRulesFromDocuments();
                job.WriteOutDocumentChanges();
            }
            Console.WriteLine($"Exiting...");
            return 0;
        }

        private class MatchingJob
        {
            public string File_In1 { get; private set; }
            public string File_In2 { get; private set; }
            public string File_Out1 { get; set; }
            public string File_Out2 { get; set; }
            public string File_Out3 { get; private set; }
            public RulePredicate IsMatchPredicate { get; private set; }

            public CssDocument Document_In1 { get; set; }
            public CssDocument Document_In2 { get; set; }
            public List<CssRule> StyleRules_In1 { get; set; }
            public List<CssRule> StyleRules_In2 { get; set; }

            public List<Tuple<CssRule, CssRule>> Matches { get; set; }

            public MatchingJob(string fileIn1, string fileIn2, string fileOut, RulePredicate predicate)
            {
                File_In1 = Path.GetFullPath(fileIn1);
                File_In2 = Path.GetFullPath(fileIn2);
                File_Out1 = File_In1 + ".dedup";
                File_Out2 = File_In2 + ".dedup";
                File_Out3 = Path.GetFullPath(fileOut);

                IsMatchPredicate = predicate;
            }

            public void Initialize()
            {
                Document_In1 = CssDocument.Factory.ParseCssFile(File_In1);
                Document_In2 = CssDocument.Factory.ParseCssFile(File_In2);

                StyleRules_In1 = Document_In1.StyleRules;
                StyleRules_In2 = Document_In2.StyleRules;

                Console.WriteLine($"--in1 :: CssDocument.StyleRules.Count: {StyleRules_In1.Count}");
                Console.WriteLine($"--in2 :: CssDocument.StyleRules.Count: {StyleRules_In2.Count}");

                Matches = StyleRules_In1.PairRulesBy(StyleRules_In2, IsMatchPredicate).ToList();

                Console.WriteLine($"Matches.Count: {Matches.Count}");
            }


            public void RemoveMatchingRulesFromDocuments()
            {
                List<CssRule> toRemove1 = Matches.Select(tup => tup.Item1).ToList();
                Document_In1.StyleRules.RemoveMany(toRemove1);

                List<CssRule> toRemove2 = Matches.Select(tup => tup.Item2).ToList();
                Document_In2.StyleRules.RemoveMany(toRemove2);
            }

            public void WriteOutDocumentChanges()
            {
                File.WriteAllText(File_Out1, Document_In1.ToString());
                Console.WriteLine($"--in1 :: Wrote {Document_In1.StyleRules.Count} CSS rules to deduped file: \"{File_Out1}\".");

                File.WriteAllText(File_Out2, Document_In2.ToString());
                Console.WriteLine($"--in2 :: Wrote {Document_In2.StyleRules.Count} CSS rules to deduped file: \"{File_Out2}\".");
            }

        }


    }
}
