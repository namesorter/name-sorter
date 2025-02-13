namespace NameSorter
{
    public interface INamesWriter
    {
        public void Write(IEnumerable<Name> names);
    }
}