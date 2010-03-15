// <copyright file="TextParser.cs" company="Jason Diamond">
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

using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace BehaveN
{
    internal static class TextParser
    {
        private static readonly Regex _languageRegex = new Regex(@"#\s*language\s*:\s*(\S+)", RegexOptions.IgnoreCase);

        internal static string DiscoverLanguage(string text)
        {
            Match m = _languageRegex.Match(text);

            if (m.Success) return m.Groups[1].Value;

            return "en";
        }

        internal static List<string> GetLines(string text)
        {
            List<string> lines = new List<string>();

            using (StringReader reader = new StringReader(text))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();

                    if (LineIsNotEmptyAndNotAComment(line))
                    {
                        lines.Add(line);
                    }
                }
            }

            return lines;
        }

        private static bool LineIsNotEmptyAndNotAComment(string line)
        {
            return line != "" && !line.StartsWith("#");
        }
    }
}
