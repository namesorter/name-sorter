namespace NameSorter
{
    public static class Utilities
    {
        public static string ToString(Name name) => $"{name.ConcatenatedGivens} {name.Last}";
    }
}
