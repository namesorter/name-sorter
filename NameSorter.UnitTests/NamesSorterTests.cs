using Moq;

namespace NameSorter.UnitTests
{
    [TestClass]
    public sealed class NamesSorterTests
    {
        [TestMethod]
        public void Given_inputFile_When_ProcessFile_Then_HappyPath()
        {
            Mock<Func<string, string[]>> readAllLines = new();
            Mock<Comparison<Name>> namesComparison = new();

            Mock<INamesWriter> writer1 = new();
            Mock<INamesWriter> writer2 = new();
            IEnumerable<INamesWriter> writers = [writer1.Object, writer2.Object];

            INamesSorter sorter = new NamesSorter(
                readAllLines.Object,
                namesComparison.Object,
                writers);

            const string inputFile = "inputFile";
            sorter.ProcessFile(inputFile);

            readAllLines.Verify(g => g.Invoke(inputFile), Times.Once);
            namesComparison.Verify(
                s => s.Invoke(It.IsAny<Name>(), It.IsAny<Name>()), Times.Never);
            writer1.Verify(w => w.Write(It.IsAny<IEnumerable<Name>>()), Times.Once);
            writer2.Verify(w => w.Write(It.IsAny<IEnumerable<Name>>()), Times.Once);
        }

        [TestMethod]
        public void Given_BadinputFile_When_ProcessFile_Then_ThrowException()
        {
            const string inputFile = "inputFile";

            Mock<Func<string, string[]>> readAllLines = new();
            readAllLines
                .Setup(r => r.Invoke(inputFile))
                .Throws<DirectoryNotFoundException>();

            Mock<Comparison<Name>> namesComparison = new();
            Mock<INamesWriter> writer1 = new();
            Mock<INamesWriter> writer2 = new();
            IEnumerable<INamesWriter> writers = [writer1.Object, writer2.Object];

            INamesSorter sorter = new NamesSorter(
                readAllLines.Object,
                namesComparison.Object,
                writers);

            var ex = Assert.ThrowsException<Exception>(
                () => sorter.ProcessFile(inputFile));
            Assert.IsNotNull(ex);
            Assert.IsTrue(ex.Message.Contains($"reading names from {inputFile}"));

            readAllLines.Verify(g => g.Invoke(inputFile), Times.Once);
            namesComparison.Verify(
                s => s.Invoke(It.IsAny<Name>(), It.IsAny<Name>()), Times.Never);
            writer1.Verify(w => w.Write(It.IsAny<IEnumerable<Name>>()), Times.Never);
            writer2.Verify(w => w.Write(It.IsAny<IEnumerable<Name>>()), Times.Never);
        }
    }
}
