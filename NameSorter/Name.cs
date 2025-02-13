namespace NameSorter
{
    /// <summary>
    /// A breakdown of a name into given names and last name.
    /// </summary>
    /// <param name="concatenatedGivens">A person's given names.</param>
    /// <param name="last">A person's last name.</param>
    public class Name(string concatenatedGivens, string last)
    {
        public string ConcatenatedGivens { get; } = concatenatedGivens;

        public string Last { get; } = last;

        /// <summary>
        /// Indicates the relative position of two <see cref="Name"/>s in
        /// sorting order.
        /// </summary>
        /// <param name="name1">The first <see cref="Name"/> to compare.</param>
        /// <param name="name2">The second <see cref="Name"/> to compare.</param>
        /// <returns>The relative position of <paramref name="name1"/> and
        /// <paramref name="name2"/> in sorting order.</returns>
        public static int Compare(Name name1, Name name2)
        {
            // Case can be ignored
            var comparison = StringComparison.InvariantCultureIgnoreCase;

            // Compare using last names first. Use given names in case of a tie
            // between last names.
            if (string.Equals(name1.Last, name2.Last, comparison))
            {
                return string.Compare(
                    name1.ConcatenatedGivens,
                    name2.ConcatenatedGivens,
                    comparison);
            }

            return string.Compare(
                name1.Last,
                name2.Last,
                comparison);
        }
    }
}