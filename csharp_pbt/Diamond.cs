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

        public static string Make(char letter) =>
            GetAlphaList(letter)
            .Select(s => s.ToString())
            .Aggregate((x, y) => $"{x}{Environment.NewLine}{y}");
 
    }
}
