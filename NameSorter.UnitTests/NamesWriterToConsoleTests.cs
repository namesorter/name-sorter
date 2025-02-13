using Moq;

namespace NameSorter.UnitTests
{
    [TestClass]
    public sealed class NamesWriterToConsoleTests
    {
        [TestMethod]
        public void Given_writeLine_When_Write_Then_writeLineCalledEveryTime()
        {
            Mock<Action<string>> writeLine = new();
            var writer = new NamesWriterToConsole(Utilities.ToString, writeLine.Object);

            var names = new List<Name>()
            {
                new("Janet", "Parsons"),
                new("Vaughn", "Lewis"),
                new("Adonis Julius", "Archer")
            };
            writer.Write(names);

            foreach (var name in names)
            {
                writeLine.Verify(w => w.Invoke(Utilities.ToString(name)), Times.Once);
            }
        }
    }
}
