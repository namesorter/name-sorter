namespace NameSorter
{
    public static class Extensions
    {
        /// <summary>
        /// Builds a <see cref="Name"/> based on <paramref name="s"/>.
        /// </summary>
        /// <param name="s">The <see cref="string"/> from which to build a
        /// <see cref="Name"/>.</param>
        /// <returns>A <see cref="Name"/> based on <paramref name="s"/>.</returns>
        /// <exception cref="ArgumentException">A single word or more than 3
        /// given names.</exception>
        public static Name ToName(this string s)
        {
            var fullName = s.Trim();

            // check there's a space before a person's last name
            var indexLastSpace = fullName.LastIndexOf(' ');
            if (indexLastSpace < 0)
            {
                throw new ArgumentException($"The last name couldn't be " +
                    $" determined for {fullName} because there is no space.");
            }

            var lastName = fullName[(indexLastSpace + 1)..];

            var givenNamesConcat = fullName[..indexLastSpace];
            var givenNames = givenNamesConcat.Split(' ');
            if (givenNames.Length > 3)
            {
                throw new ArgumentException($"A person has more than 3 given names: " +
                    $"{givenNamesConcat}.");
            }

            return new Name(givenNamesConcat, lastName);
        }
    }
}
