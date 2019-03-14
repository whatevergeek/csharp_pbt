using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace csharp_pbt
{
    public static class Diamond
    {
        static string MakeLine(int letterCount, char letter)
        {
            var padding = new String(' ', letterCount - 1);
            switch(letter)
            {
                case 'A':
                    return $"{padding ?? string.Empty}{letter}{padding ?? string.Empty}";
                default:
                    var left = 
                        $"{letter}{padding ?? string.Empty}"
                        .ToList();
                    return left.Concat(left.Reverse<char>().Skip(1))
                        .Select(x => x.ToString())
                        .Aggregate((x, y) => $"{x}{y}");
            }
        }

        public static string Make(char letter)
        {
            var letters = Enumerable.Range('A', letter - 'A' + 1).Select(i => (Char)i).ToArray();

            return letters.Concat(letters.Reverse().Skip(1))
                    .Select(s => MakeLine(letters.Count(), s))
                    .Aggregate((x, y) => $"{x}{Environment.NewLine}{y}");
        }
    }
}
