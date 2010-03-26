﻿// <copyright file="GenerateCommand.cs" company="Jason Diamond">
//
// Copyright (c) 2009-2010 Jason Diamond
//
// This source code is released under the MIT License.
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use, copy,
// modify, merge, publish, distribute, sublicense, and/or sell copies
// of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
//
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Mono.Options;

namespace BehaveN.Tool
{
    [Summary("Generates test fixtures for the scenarios contained in text files")]
    public class GenerateCommand : ICommand
    {
        public static void WriteHelp()
        {
            Console.WriteLine("usage: BehaveN.Tool Generate [OPTION]... <filepattern>...");
            Console.WriteLine();
            Console.WriteLine("<filepattern> can be the path to any file. Wildcards like *.txt do work.");
            Console.WriteLine("All files are considered to be text files containing scenarios. A .g.cs");
            Console.WriteLine("file will be generated next to each text file containing the test");
            Console.WriteLine("fixtures.");
            Console.WriteLine();
            Console.WriteLine("NOTE: You need to specify at least one text file!");
            Console.WriteLine();
            Console.WriteLine("options:");
            Console.WriteLine();
            GetOptions().WriteOptionDescriptions(Console.Out);
        }

        static string ns = "MyNamespace";
        static string baseClass = null;
        static bool noSetUp = false;
        static bool noTearDown = false;
        static string assemblyName = null;

        public int Run(string[] args)
        {
            OptionSet options = GetOptions();

            var files = options.Parse(args);

            if (files.Count < 1)
                return -1;

            var expandedFiles = ExpandWildcards(files);

            foreach (var file in expandedFiles)
            {
                var feature = new Feature();
                feature.LoadFile(file);

                string csFile = Path.ChangeExtension(file, ".g.cs");

                Console.WriteLine("Writing to " + csFile);

                StringWriter sw = new StringWriter();

                sw.WriteLine("// This code was generated by the BehaveN tool.");
                sw.WriteLine();

                sw.WriteLine("using BehaveN;");
                sw.WriteLine("using NUnit.Framework;");
                sw.WriteLine("using System.Reflection;");
                sw.WriteLine();
                sw.WriteLine("namespace {0}", ns);
                sw.WriteLine("{");

                sw.WriteLine("    [TestFixture]");

                if (feature.Headers["ignore"] != null)
                {
                    sw.WriteLine("    [Ignore(\"{0}\")]", feature.Headers["ignore"]);
                }

                string className = MakeNameSafeForCSharp(Path.GetFileNameWithoutExtension(file));

                sw.WriteLine("    public partial class {0}{1}", className, (baseClass != null) ? " : " + baseClass : "");
                sw.WriteLine("    {");

                sw.WriteLine(@"        private Feature _feature = new Feature();");

                if (!noSetUp)
                {
                    sw.WriteLine(@"
        [TestFixtureSetUp]
        public void LoadScenarios()
        {{
            _feature.StepDefinitions.UseStepDefinitionsFromAssembly({1});
            _feature.LoadEmbeddedResource(GetType().Assembly, ""{0}"");
        }}", Path.GetFileName(file), (assemblyName == null) ? "GetType().Assembly" : @"Assembly.Load(""" + assemblyName + @""")");
                }

                if (!noTearDown)
                {
                    sw.WriteLine(@"
        [TestFixtureTearDown]
        public void ReportUndefinedSteps()
        {
            _feature.ReportUndefinedSteps();
        }");
                }

                foreach (var scenario in feature.Scenarios)
                {
                    sw.WriteLine();

                    string methodName = MakeNameSafeForCSharp(scenario.Name);

                    sw.WriteLine("        [Test]");
                    sw.WriteLine("        public void {0}()", methodName);
                    sw.WriteLine("        {");
                    sw.WriteLine("            _feature.Scenarios[\"{0}\"].Verify();", scenario.Name);
                    sw.WriteLine("        }");
                }

                sw.WriteLine("    }");

                sw.WriteLine("}");

                string oldText = "";

                if (File.Exists(csFile))
                {
                    oldText = File.ReadAllText(csFile);
                }

                string newText = sw.GetStringBuilder().ToString();

                if (oldText != newText)
                {
                    File.WriteAllText(csFile, newText);
                }
            }

            return 0;
        }

        private List<string> ExpandWildcards(List<string> files)
        {
            var expandedFiles = new List<string>();

            foreach (var file in files)
            {
                if (file.Contains("*"))
                {
                    string path = Path.GetDirectoryName(file);
                    string searchPattern = Path.GetFileName(file);
                    expandedFiles.AddRange(Directory.GetFiles(path, searchPattern));
                }
                else
                {
                    expandedFiles.Add(file);
                }
            }

            return expandedFiles;
        }

        private string MakeNameSafeForCSharp(string name)
        {
            name = Regex.Replace(name, @"\p{P}", "");

            name = name.Replace(" ", "_");

            if (Char.IsDigit(name[0]))
            {
                name = "_" + name;
            }

            return name;
        }

        private static OptionSet GetOptions()
        {
            var options = new OptionSet();
            options.Add("namespace=", "the namespace for the generated classes", s => ns = s);
            options.Add("base-class=", "the base class for the generated classes", s => baseClass = s);
            options.Add("no-setup", "do not generate a test fixture set up method", s => noSetUp = true);
            options.Add("no-teardown", "do not generate a test fixture tear down method", s => noTearDown = true);
            options.Add("assembly=", "the assembly that contains the step definitions", s => assemblyName = s);
            return options;
        }
    }
}
