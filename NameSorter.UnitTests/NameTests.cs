namespace NameSorter.UnitTests
{
    [TestClass]
    public sealed class NameTests
    {
        [TestMethod]
        public void Given_DifferentLastNames_When_Compare_Then_BasedOnLastNames()
        {
            Name name1 = new("Janet", "Parsons");
            Name name2 = new("Vaughn", "Lewis");

            Assert.AreEqual(
                string.Compare(name1.Last, name2.Last),
                Name.Compare(name1, name2));
            Assert.AreEqual(
                string.Compare(name2.Last, name1.Last),
                Name.Compare(name2, name1));
        }

        [TestMethod]
        public void Given_SameLastNamesDifferentGivenNames_When_Compare_Then_BasedOnGivenNames()
        {
            Name name1 = new("Janet", "Parsons");
            Name name2 = new("Jane", "parsOns");

            Assert.AreEqual(
                string.Compare(name1.ConcatenatedGivens, name2.ConcatenatedGivens),
                Name.Compare(name1, name2));
            Assert.AreEqual(
                string.Compare(name2.ConcatenatedGivens, name1.ConcatenatedGivens),
                Name.Compare(name2, name1));
        }

        [TestMethod]
        public void Given_SameNames_When_Compare_Then_ReturnIdentical()
        {
            Name name = new("Janet", "Parsons");

            Assert.AreEqual(0, Name.Compare(name, name));
        }
    }
}
