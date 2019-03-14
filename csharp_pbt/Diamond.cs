using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace csharp_pbt
{
    public static class Diamond
    {
        static string MakeLine(int letterCount, char letter, int letterIndex)
        {
            var leadingSpace = new String(' ', letterCount - 1 - letterIndex);
            var innerSpace = new String(' ', letterCount - 1 - leadingSpace.Length);
            switch (letter)
            {
                case 'A':
                    return $"{leadingSpace ?? string.Empty}{letter}{leadingSpace ?? string.Empty}";
                default:
                    var left = 
                        $"{leadingSpace ?? string.Empty}{letter}{innerSpace ?? string.Empty}"
                        .ToList();
                    return left.Concat(left.Reverse<char>().Skip(1))
                        .Select(x => x.ToString())
                        .Aggregate((x, y) => $"{x}{y}");
            }
        }

        public static string Make(char letter)
        {
            var letters = Enumerable.Range('A', letter - 'A' + 1).Select(i => (Char)i).ToArray()
                .Select((val, index) => (value: val, index: index));

            return letters.Concat(letters.Reverse().Skip(1))
                    .Select(s => MakeLine(letters.Count(), s.value, s.index))
                    .Aggregate((x, y) => $"{x}{Environment.NewLine}{y}");
        }
    }
}
