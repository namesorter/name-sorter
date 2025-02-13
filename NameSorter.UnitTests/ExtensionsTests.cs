using NameSorter;

namespace NameSorter.UnitTests
{
    [TestClass]
    public sealed class ExtensionsTests
    {
        [TestMethod]
        [DataRow("Janet Parsons")]
        [DataRow("   Vaughn   Lewis  ")]
        [DataRow("Adonis Julius Archer")]
        [DataRow("Hunter Uriah Mathew Clarke")]
        public void Given_ValidNameInput_When_CallToName_Then_ReturnName(string s)
        {
            var name = s.ToName();
            Assert.IsTrue(string.Equals(
                s.Trim(),
                Utilities.ToString(name),
                StringComparison.InvariantCulture));
        }

        [TestMethod]
        [DataRow("Janet")]
        [DataRow("   Lewis  ")]
        public void Given_OnlyLastName_When_CallToName_Then_ThrowArgumentException(string s)
        {
            var ex = Assert.ThrowsException<ArgumentException>(s.ToName);
            Assert.IsNotNull(ex);
            Assert.IsTrue(ex.Message.Contains("no space"));
        }

        [TestMethod]
        [DataRow("Vaughn Hunter Uriah Mathew Clarke")]
        [DataRow("   Vaughn  Hunter Uriah Mathew Clarke  ")]
        public void Given_MoreThanThreeGivenNames_When_CallToName_Then_ThrowArgumentException(string s)
        {
            var ex = Assert.ThrowsException<ArgumentException>(s.ToName);
            Assert.IsNotNull(ex);
            Assert.IsTrue(ex.Message.Contains("more than 3"));
        }
    }
}
