﻿namespace NameSorter
{
    /// <summary>
    /// The driver of the solution. Requires a method for receiving a list of
    /// names, a method for comparing two names (for sorting) and writers that
    /// produce output based on the sorted list.
    /// </summary>
    public class NamesSorter : INamesSorter
    {
        private readonly Func<string, string[]> _readAllLines;
        private readonly Comparison<Name> _namesComparison;
        private readonly IEnumerable<INamesWriter> _namesWriters;

        /// <summary>
        /// Constructor for <see cref="NamesSorter"/>.
        /// </summary>
        /// <param name="readAllLines">A method that takes a filename and
        /// returns an array of <see cref="string"/>s.</param>
        /// <param name="namesComparison">A method that indicates the relative
        /// position of two <see cref="Name"/>s in sorting order.</param>
        /// <param name="namesWriters">Writers that produce output based on a
        /// list of <see cref="Name"/>s.</param>
        /// <exception cref="ArgumentException">One of the
        /// <paramref name="namesWriters"/>is null.</exception>
        public NamesSorter(
            Func<string, string[]> readAllLines,
            Comparison<Name> namesComparison,
            IEnumerable<INamesWriter> namesWriters)
        {
            _readAllLines = readAllLines;
            _namesComparison = namesComparison;

            foreach (var namesWriter in namesWriters)
            {
                if (namesWriter is null)
                {
                    throw new ArgumentException(
                        $"One of the {nameof(namesWriters)} is null.");
                }
            }

            _namesWriters = namesWriters;
        }

        /// <summary>
        /// Produces sorted output based on the names in a file.
        /// </summary>
        /// <param name="inputFile">The location of the file containing
        /// names.</param>
        /// <exception cref="Exception">Thrown due to an error attempting to
        /// open and read <paramref name="inputFile"/>.</exception>
        public void ProcessFile(string inputFile)
        {
            IEnumerable<string> namesAsStrings = [];

            try
            {
                namesAsStrings = _readAllLines(inputFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred reading names from " +
                    $"{inputFile}: {ex}");
                throw new Exception(
                    $"Error occurred reading names from {inputFile}",
                    ex);
            }

            // Sort by comparing Names instead of using default string
            // comparison. Convert each string just read in, into a Name.
            var names = namesAsStrings.Select(s => s.ToName()).ToList();
            names.Sort(_namesComparison);

            foreach (var namesWriter in _namesWriters)
            {
                namesWriter.Write(names);
            }
        }
    }
}
