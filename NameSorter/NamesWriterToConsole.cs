namespace NameSorter
{
    /// <summary>
    /// Writes <see cref="Name"/>s to the console.
    /// </summary>
    /// <param name="toString">Convert a <see cref="Name"/> to a
    /// <see cref="string"/>. Allow for injecting something other than
    /// <see cref="Utilities.ToString(Name)"/>.</param>
    /// <param name="writeLine">For unit tests e.g. inject a mock to check
    /// each <see cref="Name"/> is written.</param>
    public class NamesWriterToConsole(
        Func<Name, string>? toString = null,
        Action<string>? writeLine = null) : INamesWriter
    {
        private readonly Func<Name, string> _toString = toString ?? Utilities.ToString;
        private readonly Action<string> _writeLine = writeLine ?? Console.WriteLine;

        /// <summary>
        /// Writes <see cref="Name"/>s to the console.
        /// </summary>
        /// <param name="names">The <see cref="Name"/>s to be written to the
        /// console.</param>
        public void Write(IEnumerable<Name> names)
        {
            foreach (var name in names)
            {
                _writeLine(_toString(name));
            }
        }
    }
}
