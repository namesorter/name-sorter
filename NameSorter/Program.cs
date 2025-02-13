using NameSorter;

// Expect a filename containing names, which this program sorts.
if (args.Length < 1)
{
    Console.WriteLine("No path to a file (of names) was provided.");
    return;
}

// When this program has sorted the names, provide two forms of output:
// console and file.
List<INamesWriter> namesWriters = [
    new NamesWriterToConsole(),
    new NamesWriterToFile("sorted-names-list.txt")];

// Inject core functionality into an INamesSorter.
INamesSorter nameSorter = new NamesSorter(
    File.ReadAllLines,
    Name.Compare,
    namesWriters);
nameSorter.ProcessFile(args[0]);