using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BehaveN
{
    /// <summary>
    /// A utility class for parsing method names.
    /// </summary>
    public static class NameParser
    {
        private static readonly Regex _stepDefinitionTester = new Regex(@"^([Gg]iven|[Ww]hen|[Tt]hen)(_|[A-Z])");
        private static readonly Regex _underscoreSplitter = new Regex(@"_+");
        private static readonly Regex _camelCaseSplitter = new Regex(@"(?<!^)(?=[A-Z])");
        private static readonly Regex _prefixRemover = new Regex(@"^(given|when|then) ", RegexOptions.IgnoreCase);

        /// <summary>
        /// Determines if the specified method is a step definition.
        /// </summary>
        /// <param name="methodInfo">The method info.</param>
        /// <returns><c>true</c> if the method name starts with given, when, or then; otherwise <c>false</c></returns>
        public static bool IsStepDefinition(MethodInfo methodInfo)
        {
            return _stepDefinitionTester.IsMatch(methodInfo.Name);
        }

        /// <summary>
        /// Parses the specified method's name.
        /// </summary>
        /// <param name="methodInfo">The method info.</param>
        /// <returns>The parsed name.</returns>
        public static string Parse(MethodInfo methodInfo)
        {
            return Parse(methodInfo, true);
        }

        /// <summary>
        /// Parses the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The parsed name.</returns>
        public static string Parse(string name)
        {
            if (name == null) throw new ArgumentNullException("name");

            return Parse(name, false);
        }

        /// <summary>
        /// Parses the specified method's name.
        /// </summary>
        /// <param name="methodInfo">The method info.</param>
        /// <param name="removePrefix">if set to <c>true</c>, given/when/then prefixes are removed.</param>
        /// <returns>The parsed name.</returns>
        public static string Parse(MethodInfo methodInfo, bool removePrefix)
        {
            string result = methodInfo.Name;

            if (result.Contains("_"))
            {
                result = string.Join(" ", _underscoreSplitter.Split(result.Trim('_')));
            }
            else
            {
                result = string.Join(" ", _camelCaseSplitter.Split(result));
            }

            if (removePrefix)
            {
                result = _prefixRemover.Replace(result, "").ToLowerInvariant();
            }

            return result;
        }

        /// <summary>
        /// Parses the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="removePrefix">if set to <c>true</c>, given/when/then prefixes are removed.</param>
        /// <returns>The parsed name.</returns>
        public static string Parse(string name, bool removePrefix)
        {
            string result = name;

            if (result.Contains("_"))
            {
                result = string.Join(" ", _underscoreSplitter.Split(result.Trim('_')));
            }
            else
            {
                result = string.Join(" ", _camelCaseSplitter.Split(result));
            }

            if (removePrefix)
            {
                result = _prefixRemover.Replace(result, "").ToLowerInvariant();
            }

            return result;
        }
    }
}