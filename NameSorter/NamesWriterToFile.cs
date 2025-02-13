namespace NameSorter
{
    /// <summary>
    /// Writes <see cref="Name"/>s to a file.
    /// </summary>
    /// <param name="filename">The location of the file to write
    /// <see cref="Name"/>s to.</param>
    /// <param name="toString">Convert a <see cref="Name"/> to a
    /// <see cref="string"/>. Allow for injecting something other than
    /// <see cref="Utilities.ToString(Name)"/>.</param>
    /// <param name="writeAllLines">Writes <see cref="Name"/>s to
    /// <paramref name="filename"/>. Can mock out <see cref="File"/> in unit
    /// tests.</param>
    public class NamesWriterToFile(
        string filename,
        Func<Name, string>? toString = null,
        Action<string, IEnumerable<string>>? writeAllLines = null) : INamesWriter
    {
        private readonly string _filename = filename;
        private readonly Func<Name, string> _toString = toString ?? Utilities.ToString;
        private readonly Action<string, IEnumerable<string>> _writeAllLines =
            writeAllLines ?? File.WriteAllLines;

        /// <summary>
        /// Writes <see cref="Name"/>s to a file.
        /// </summary>
        /// <param name="names">The <see cref="Name"/>s to be written to
        /// <see cref="_filename"/>.</param>
        public void Write(IEnumerable<Name> names)
        {
            try
            {
                _writeAllLines(_filename, ConvertNameToString(names));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred writing names to " +
                    $"{_filename}: {ex}");
                throw new Exception($"Error occurred writing names to " +
                    $"{_filename}", ex);
            }
        }

        private IEnumerable<string> ConvertNameToString(IEnumerable<Name> names)
        {
            foreach (var name in names)
            {
                yield return _toString(name);
            }
        }
    }
}
