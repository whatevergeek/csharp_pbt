using FsCheck;
using FsCheck.Xunit;
using Xunit;
using System;

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
            public DiamondProperty(): base()
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
    }
}