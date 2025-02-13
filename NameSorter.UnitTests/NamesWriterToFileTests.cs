using Moq;

namespace NameSorter.UnitTests
{
    [TestClass]
    public sealed class NamesWriterToFileTests
    {
        const string filename = "filename";

        [TestMethod]
        public void Given_writeAllLines_When_Write_Then_StringConcatEveryTime()
        {
            List<string> namesAsStrings = [];

            Mock<Action<string, IEnumerable<string>>> writeAllLines = new();
            writeAllLines
                .Setup(w => w.Invoke(filename, It.IsAny<IEnumerable<string>>()))
                .Callback<string, IEnumerable<string>>(
                    (_, c) => namesAsStrings = c.ToList());

            var writer = new NamesWriterToFile(
                filename,
                Utilities.ToString,
                writeAllLines.Object);

            var names = new List<Name>()
            {
                new("Janet", "Parsons"),
                new("Vaughn", "Lewis"),
                new("Adonis Julius", "Archer")
            };
            writer.Write(names);

            writeAllLines.Verify(w => w.Invoke(
                filename,
                It.IsAny<IEnumerable<string>>()),
                Times.Once);

            Assert.AreEqual(names.Count, namesAsStrings.Count);
            foreach (var (name, s) in names.Zip(namesAsStrings))
            {
                Assert.IsTrue(string.Equals(
                    Utilities.ToString(name),
                    s,
                    StringComparison.InvariantCulture));
            }
        }

        [TestMethod]
        public void Given_writeAllLinesThrowsException_When_Write_Then_ThrowException()
        {
            Mock<Action<string, IEnumerable<string>>> writeAllLines = new();
            writeAllLines
                .Setup(w => w.Invoke(It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
                .Throws<DirectoryNotFoundException>();

            var writer = new NamesWriterToFile(
                filename,
                Utilities.ToString,
                writeAllLines.Object);
            var ex = Assert.ThrowsException<Exception>(() => writer.Write([]));
            
            Assert.IsNotNull(ex);
            Assert.IsTrue(ex.Message.Contains($"writing names to {filename}"));
        }
    }
}
