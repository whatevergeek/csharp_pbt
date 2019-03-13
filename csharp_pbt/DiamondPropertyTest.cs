using FsCheck;
using FsCheck.Xunit;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace csharp_pbt
{
    public class DiamondPropertyTest
    {
        private int count = 0;

        public static class Letters
        {
            public static Arbitrary<char> Char =>
                Arb.Default.Char()
                .Filter(c => 'A' <= c && c <= 'Z');
        }

        public class DiamondProperty : PropertyAttribute
        {
            public DiamondProperty() : base()
            {
                Arbitrary = new Type[] { typeof(Letters) };
            }
        }


        [Property]
        public bool TestProperty_DiamondIsNonEmpty(char letter)
        {
            var actual = Diamond.Make(letter);
            return !string.IsNullOrWhiteSpace(actual);
        }

        [Property(Arbitrary = new Type[] { typeof(Letters) })]
        public bool TestProperty_DiamondIsNonEmptyV2(char letter)
        {
            var actual = Diamond.Make(letter);
            return !string.IsNullOrWhiteSpace(actual);
        }

        [DiamondProperty]
        public bool TestProperty_DiamondIsNonEmptyV3(char letter)
        {
            count++;
            var actual = Diamond.Make(letter);
            return !string.IsNullOrWhiteSpace(actual);
        }

        string[] Split(string x) => x.Split(Environment.NewLine, options: StringSplitOptions.None);

        [DiamondProperty]
        public bool TestProperty_FirstRowContainsA(char letter)
        {
            var actual = Diamond.Make(letter);
            return Split(actual)?[0]?.Contains("A") ?? false;
        }

        [DiamondProperty]
        public bool TestProperty_AllRowsMustHaveSymmetricContour(char letter)
        {
            var actual = Diamond.Make(letter);
            var alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

            string GetLeadingSpaces(string x)
            {
                var indexOfNonSpace = x.IndexOfAny(alpha);
                return x.Substring(0, indexOfNonSpace);
            }

            string GetTrailingSpaces(string x)
            {
                var trailingIndexOfNonSpace = x.LastIndexOfAny(alpha);
                return x.Substring(trailingIndexOfNonSpace + 1);
            }

            var rows = Split(actual);
            return Array.TrueForAll<string>(rows, r => GetLeadingSpaces(r) == GetTrailingSpaces(r));
        }

        IEnumerable<char> GetAlphaList(char endChar)
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

        [DiamondProperty]
        public bool TestProperty_FigureTopHasCorrectLettersAndOrder(char letter)
        {
            var actual = Diamond.Make(letter);
            var expected = GetAlphaList(letter);

            var rows = Split(actual);
            var firstNonWhiteSpaceLetters =
                rows.Take(expected.Count())
                .ToList<string>()
                .Select(s => s.Trim()[0]);
            return expected.SequenceEqual(firstNonWhiteSpaceLetters);
        }

        [DiamondProperty]
        public bool TestProperty_FigureIsHorizontallySymmetric(char letter)
        {
            var actual = Diamond.Make(letter);

            var rows = Split(actual);

            var topRows =
                rows
                .TakeWhile(r => !r.Contains(letter))
                .ToList<string>();
            var bottomRows =
                rows
                .SkipWhile(r => !r.Contains(letter))
                .Skip(1)
                .ToList().Reverse<string>();

            return topRows.SequenceEqual(bottomRows);
        }

        [DiamondProperty]
        public bool TestProperty_DiamondIsAsWideAsItsHeight(char letter)
        {
            var actual = Diamond.Make(letter);

            var rows = Split(actual);
            var expectedRowLength = rows.Length;

            return Array.TrueForAll<string>(rows, row => row.Length == expectedRowLength);
        }
    }
}
