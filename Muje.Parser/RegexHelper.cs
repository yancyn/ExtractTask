using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Muje.Parser
{
    public class RegexHelper
    {
        private static string ToSingleCharacterPattern(string pattern)
        {
            string output = string.Empty;
            foreach (char c in pattern)
                output += "[" + c.ToString() + "]";
            return output;
        }
        /// <summary>
        /// Return clean string after removing prefix and suffix from match result.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="prefix">Anything match after this string.</param>
        /// <param name="suffix">Anything match before this string.</param>
        /// <returns></returns>
        public static string GrabPattern(string source, string prefix, string suffix)
        {
            string result = string.Empty;

            char[] prefixes = new char[prefix.Length];
            List<char> pre = new List<char>();
            foreach (char c in prefix)
                pre.Add(c);
            prefixes = pre.ToArray();

            char[] suffixes = new char[suffix.Length];
            List<char> suf = new List<char>();
            foreach (char c in suffix)
                suf.Add(c);
            suffixes = suf.ToArray();

            string pattern = ToSingleCharacterPattern(prefix) + ".+" + ToSingleCharacterPattern(suffix);
            Regex regex = new Regex(pattern);
            Match match = regex.Match(source);
            if (match.Success)
            {
                //System.Diagnostics.Debug.WriteLine(match.Value);
                string chock = match.Value;
                chock = chock.Replace(prefix, string.Empty);
                chock = chock.Replace(suffix, string.Empty);
                result = chock.Trim();
            }
            return result;
        }
    }
}