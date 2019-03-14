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

        char[] alpha = Enumerable.Range('A', 'Z' - 'A' + 1).Select(i => (Char) i).ToArray();
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

        [DiamondProperty]
        public bool TestProperty_AllRowsMustHaveSymmetricContour(char letter)
        {
            var actual = Diamond.Make(letter);
            var rows = Split(actual);
            return Array.TrueForAll<string>(rows, r => GetLeadingSpaces(r) == GetTrailingSpaces(r));
        }

        [DiamondProperty]
        public bool TestProperty_FigureTopHasCorrectLettersAndOrder(char letter)
        {
            var actual = Diamond.Make(letter);
            var expected = Enumerable.Range('A', letter - 'A' + 1).Select(i => (Char)i).ToArray();

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

        [DiamondProperty]
        public bool TestProperty_AllRowsExceptTopAndBottomHaveTwoIdenticalLetters(char letter)
        {
            var actual = Diamond.Make(letter);

            bool hasIdenticalLetters(string x) => x.Distinct().Count() == 1;
            bool hasTwoLetters(string x) => x.Length == 2;
            bool hasTwoIdenticalLetters(string x) => hasIdenticalLetters(x) && hasTwoLetters(x);

            var rows = Split(actual)
                .Where(x => !x.Contains("A"))
                .Select(x => x.Replace(" ", string.Empty))
                .ToArray();

            return Array.TrueForAll<string>(rows, row => hasTwoIdenticalLetters(row));
        }

        [DiamondProperty]
        public bool TestProperty_LowerLeftSpaceIsAnIsoscelesRightTriangle(char letter)
        {
            var actual = Diamond.Make(letter);
            var rows = Split(actual);
            var lowerLeftSpace =
                rows.SkipWhile(x => !(x.Contains(letter.ToString())))
                .Select(x => GetLeadingSpaces(x))
                .ToList();
            var spaceCounts = lowerLeftSpace.Select(x => x.Length);
            var expected = Enumerable.Range(0, spaceCounts.Count());

            return expected.SequenceEqual(spaceCounts);
        }
    }
}
