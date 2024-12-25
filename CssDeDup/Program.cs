using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;

using System.CommandLine;
using System.CommandLine.Parsing;

namespace CssDeDup
{
    internal class Program
    {
        static int Main(string[] args)
        {
            Option<string?> optionFileIn1 = new Option<string?>(
                name: "--in1",
                description: "The first CSS file.");
            optionFileIn1.IsRequired = true;
            optionFileIn1.Arity = ArgumentArity.ExactlyOne;
            
            Option<string?> optionFileIn2 = new Option<string?>(
                name: "--in2",
                description: "The second CSS file.");
            optionFileIn2.IsRequired = true;
            optionFileIn2.Arity = ArgumentArity.ExactlyOne;

            Option<string?> optionFileOut = new Option<string?>(
                name: "--out",
                description: "The output CSS file.");
            optionFileOut.IsRequired = true;
            optionFileOut.Arity = ArgumentArity.ExactlyOne;

            var commandMatchNameAndProperties = new Command("nameandproperties", "Style rules must match: 1) rule name, 2) properties, and 3) property values, to be considered duplicates.");            
            commandMatchNameAndProperties.AddOption(optionFileIn1);
            commandMatchNameAndProperties.AddOption(optionFileIn2);
            commandMatchNameAndProperties.AddOption(optionFileOut);

            var commandMatchProperties = new Command("properties", "Style rules must match: 1) properties, and 2) property values, to be considered duplicates.");
            commandMatchProperties.AddOption(optionFileIn1);
            commandMatchProperties.AddOption(optionFileIn2);
            commandMatchProperties.AddOption(optionFileOut);
            
            //var commandMatch = new Command("match");

            var rootCommand = new RootCommand("CssDeDup: Takes two .css files and creates a third file containing duplicate style rules between the two files. It then writes out the two original files, sans the duplicate rules, with a .dedup file extension.");
            rootCommand.AddCommand(commandMatchNameAndProperties);
            rootCommand.AddCommand(commandMatchProperties);
            
            commandMatchNameAndProperties.SetHandler((fileIn1, fileIn2, fileOut) =>
            {
                Match.NameAndProperties(fileIn1, fileIn2, fileOut);
            },
                optionFileIn1, optionFileIn2, optionFileOut);

            commandMatchProperties.SetHandler((fileIn1, fileIn2, fileOut) =>
            {
                Match.PropertiesOnly(fileIn1, fileIn2, fileOut);
            },
                optionFileIn1, optionFileIn2, optionFileOut);

            return rootCommand.Invoke(args);
        }
    }
}
