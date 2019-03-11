using FsCheck.Xunit;
using Xunit;

namespace csharp_pbt
{
    public class DiamondPropertyTest
    {
        [Property]
        public bool TestProperty_DiamondIsNonEmpty(char letter)
        {
            var actual = Diamond.Make(letter);
            return !string.IsNullOrWhiteSpace(actual);
        }
    }
}
