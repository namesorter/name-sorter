using Moq;

namespace NameSorter.UnitTests
{
    [TestClass]
    public sealed class NamesSorterTests
    {
        const string InputFile = "inputFile";

        [TestMethod]
        public void Given_inputFile_When_ProcessFile_Then_HappyPath()
        {
            Mock<Func<string, string[]>> readAllLines = new();
            readAllLines
                .Setup(r => r.Invoke(InputFile))
                .Returns([
                    "Janet Parsons",
                    "Vaughn Lewis"
                    ]);

            Mock<Comparison<Name>> namesComparison = new();
            Mock<INamesWriter> writer1 = new();
            Mock<INamesWriter> writer2 = new();
            IEnumerable<INamesWriter> writers = [writer1.Object, writer2.Object];

            INamesSorter sorter = new NamesSorter(
                readAllLines.Object,
                namesComparison.Object,
                writers);
            sorter.ProcessFile(InputFile);

            readAllLines.Verify(g => g.Invoke(InputFile), Times.Once);
            namesComparison.Verify(
                s => s.Invoke(It.IsAny<Name>(), It.IsAny<Name>()), Times.Once);
            writer1.Verify(w => w.Write(It.IsAny<IEnumerable<Name>>()), Times.Once);
            writer2.Verify(w => w.Write(It.IsAny<IEnumerable<Name>>()), Times.Once);
        }

        [TestMethod]
        public void Given_NoNames_When_ProcessFile_Then_HappyPath()
        {
            Mock<Func<string, string[]>> readAllLines = new();
            readAllLines
                .Setup(r => r.Invoke(InputFile))
                .Returns([]);

            Mock<Comparison<Name>> namesComparison = new();
            Mock<INamesWriter> writer1 = new();
            Mock<INamesWriter> writer2 = new();
            IEnumerable<INamesWriter> writers = [writer1.Object, writer2.Object];

            INamesSorter sorter = new NamesSorter(
                readAllLines.Object,
                namesComparison.Object,
                writers);
            sorter.ProcessFile(InputFile);

            readAllLines.Verify(g => g.Invoke(InputFile), Times.Once);
            namesComparison.Verify(
                s => s.Invoke(It.IsAny<Name>(), It.IsAny<Name>()), Times.Never);
            writer1.Verify(w => w.Write(It.IsAny<IEnumerable<Name>>()), Times.Once);
            writer2.Verify(w => w.Write(It.IsAny<IEnumerable<Name>>()), Times.Once);
        }

        [TestMethod]
        public void Given_BadinputFile_When_ProcessFile_Then_ThrowException()
        {
            Mock<Func<string, string[]>> readAllLines = new();
            readAllLines
                .Setup(r => r.Invoke(InputFile))
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
                () => sorter.ProcessFile(InputFile));
            Assert.IsNotNull(ex);
            Assert.IsTrue(ex.Message.Contains($"reading names from {InputFile}"));

            readAllLines.Verify(g => g.Invoke(InputFile), Times.Once);
            namesComparison.Verify(
                s => s.Invoke(It.IsAny<Name>(), It.IsAny<Name>()), Times.Never);
            writer1.Verify(w => w.Write(It.IsAny<IEnumerable<Name>>()), Times.Never);
            writer2.Verify(w => w.Write(It.IsAny<IEnumerable<Name>>()), Times.Never);
        }

        [TestMethod]
        public void Given_nullWriters_When_CreateNamesSorter_Then_ThrowException()
        {
            Mock<Func<string, string[]>> readAllLines = new();
            Mock<Comparison<Name>> namesComparison = new();
            Mock<INamesWriter> writer = new();

            {
                List<INamesWriter> writers = [null, writer.Object];

                var ex = Assert.ThrowsException<ArgumentException>(
                    () => new NamesSorter(
                        readAllLines.Object,
                        namesComparison.Object,
                        writers));
                Assert.IsNotNull(ex);
                Assert.IsTrue(ex.Message.Contains(
                    $"namesWriters is null"));
            }

            {
                List<INamesWriter> writers = [writer.Object, null];

                var ex = Assert.ThrowsException<ArgumentException>(
                    () => new NamesSorter(
                        readAllLines.Object,
                        namesComparison.Object,
                        writers));
                Assert.IsNotNull(ex);
                Assert.IsTrue(ex.Message.Contains(
                    $"namesWriters is null"));
            }
        }
    }
}
