using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Text.RegularExpressions;

namespace BehaveN
{
    /// <summary>
    /// Reads specifications in the Given/When/Then style from a plain text file.
    /// </summary>
    public class PlainTextReader
    {
        private readonly string _text;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlainTextReader"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public PlainTextReader(string text)
        {
            _text = text;
        }

        /// <summary>
        /// Reads the contents of the file to the specifications file.
        /// </summary>
        /// <param name="specificationsFile">The specifications file.</param>
        public void ReadTo(SpecificationsFile specificationsFile)
        {
            CompileRegexes();

            List<string> lines = TextParser.GetLines(_text);

            Scenario scenario = null;
            StepType stepType = StepType.Unknown;

            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];

                Match m = _featureRegex.Match(line);

                if (m.Success)
                {
                    i = ParseFeature(i, lines, ref line, ref m, specificationsFile);
                }
                else
                {
                    m = _scenarioRegex.Match(line);
                }

                if (m.Success)
                {
                    scenario = new Scenario();
                    scenario.Name = m.Groups[1].Value;
                    specificationsFile.Scenarios.Add(scenario);
                    stepType = StepType.Unknown;
                }
                else
                {
                    ParseStep(lines, scenario, ref stepType, ref i, line);
                }
            }
        }

        private int ParseFeature(int i, List<string> lines, ref string line, ref Match m, SpecificationsFile specificationsFile)
        {
            specificationsFile.Feature = new Feature();
            specificationsFile.Feature.Name = m.Groups[1].Value;

            List<string> featureLines = new List<string>();

            for (i += 1; i < lines.Count; i++)
            {
                line = lines[i];

                m = _scenarioRegex.Match(line);

                if (m.Success)
                {
                    break;
                }

                featureLines.Add(line);
            }

            specificationsFile.Feature.Description = string.Join("\r\n", featureLines.ToArray()).Trim('\r', '\n');
            return i;
        }

        private void ParseStep(List<string> lines, Scenario scenario, ref StepType stepType, ref int i, string line)
        {
            if (_givenRegex.Match(line).Success)
            {
                stepType = StepType.Given;
            }
            else if (_whenRegex.Match(line).Success)
            {
                stepType = StepType.When;
            }
            else if (_thenRegex.Match(line).Success)
            {
                stepType = StepType.Then;
            }
            else if (_andRegex.Match(line).Success)
            {
                if (stepType == StepType.Unknown)
                    throw new Exception("\"And\" steps cannot appear before \"given\", \"when\", or \"then\" steps.");
            }
            else
            {
                throw new Exception(string.Format("Unrecognized step: \"{0}\".", line));
            }

            if (scenario == null)
                throw new Exception("Steps cannot appear before a scenario is started.");

            IBlock block = ParseBlock(lines, ref i);

            scenario.Steps.Add(stepType, line, block);
        }

        private IBlock ParseBlock(List<string> lines, ref int i)
        {
            IBlock block = null;

            if (Form.NextLineIsForm(lines, i))
            {
                Form form = Form.ParseForm(lines, ++i);
                i += form.Size - 1;
                block = form;
            }
            else if (Grid.NextLineIsGrid(lines, i))
            {
                Grid grid = Grid.ParseGrid(lines, ++i);
                i += grid.RowCount;
                block = grid;
            }

            return block;
        }

        private void CompileRegexes()
        {
            string language = TextParser.DiscoverLanguage(_text);

            ResourceSet strings = Languages.Strings.ResourceManager.GetResourceSet(GetCultureInfo(language), true, true);

            string feature = strings.GetString("Feature");
            string scenario = strings.GetString("Scenario");
            string given = strings.GetString("Given");
            string when = strings.GetString("When");
            string then = strings.GetString("Then");
            string and = strings.GetString("And");

            _featureRegex = new Regex(string.Format(_featurePattern, feature), RegexOptions.IgnoreCase);
            _scenarioRegex = new Regex(string.Format(_scenarioPattern, scenario), RegexOptions.IgnoreCase);
            _givenRegex = new Regex(string.Format(_stepPattern, given), RegexOptions.IgnoreCase);
            _whenRegex = new Regex(string.Format(_stepPattern, when), RegexOptions.IgnoreCase);
            _thenRegex = new Regex(string.Format(_stepPattern, then), RegexOptions.IgnoreCase);
            _andRegex = new Regex(string.Format(_stepPattern, and), RegexOptions.IgnoreCase);
        }

        private CultureInfo GetCultureInfo(string language)
        {
            try
            {
                return CultureInfo.GetCultureInfo(language);
            }
            catch (ArgumentException)
            {
                return CultureInfo.GetCultureInfo("en");
            }
        }

        private Regex _featureRegex;
        private Regex _scenarioRegex;
        private Regex _givenRegex;
        private Regex _whenRegex;
        private Regex _thenRegex;
        private Regex _andRegex;

        private static readonly string _featurePattern = @"^\s*{0}\s*:\s*(.+)";
        private static readonly string _scenarioPattern = @"^\s*{0}\s*\d*\s*:\s*(.+)";
        private static readonly string _stepPattern = @"^\s*({0})\s+.+";
    }
}
