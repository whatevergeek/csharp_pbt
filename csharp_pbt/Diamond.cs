using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace csharp_pbt
{
    public static class Diamond
    {
        static IEnumerable<T> MirrorAndFuse<T>(IEnumerable<T> l) => 
            l.Concat(l.Reverse<T>().Skip<T>(1));
        static string MakeLine(int letterCount, char letter, int letterIndex)
        {
            var leadingSpace = new String(' ', letterCount - 1 - letterIndex);
            var innerSpace = new String(' ', letterCount - 1 - leadingSpace.Length);
            var left =
                $"{leadingSpace ?? string.Empty}{letter}{innerSpace ?? string.Empty}"
                .ToList();
            return MirrorAndFuse(left)
                .Select(x => x.ToString())
                .Aggregate((x, y) => $"{x}{y}");
        }

        public static string Make(char letter)
        {
            var indexedLetters = Enumerable.Range('A', letter - 'A' + 1).Select(i => (Char)i).ToArray()
                .Select((val, index) => (value: val, index: index));

            return MirrorAndFuse(indexedLetters)
                    .Select(s => MakeLine(indexedLetters.Count(), s.value, s.index))
                    .Aggregate((x, y) => $"{x}{Environment.NewLine}{y}");
        }
    }
}
