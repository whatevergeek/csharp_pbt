using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace csharp_pbt
{
    public static class Diamond
    {
        static IEnumerable<char> GetAlphaList(char endChar)
        {
            foreach (var c in "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
            {
                yield return c;
                if (c == endChar)
                {
                    break;
                }
            }
        }

        static string MakeLine(int letterCount, char letter)
        {
            var padding = new String(' ', letterCount - 1);
            return $"{padding ?? string.Empty}{letter}{padding ?? string.Empty}";
        }

        public static string Make(char letter)
        {
            var letters = GetAlphaList(letter);
            
            return letters.Concat(letters.Reverse().Skip(1))
                .Select(s => MakeLine(letters.Count(), s))
                .Aggregate((x, y) => $"{x}{Environment.NewLine}{y}");
        }
    }
}
