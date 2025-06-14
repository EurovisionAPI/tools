namespace Scanner;

internal class UnavailableData
{
    private List<string> _fields;

    public string Name { get; }
    public IReadOnlyList<string> Fields => _fields;
    public virtual bool HasUnavailableData => _fields.Count > 0;

    public UnavailableData(string name, IEnumerable<string> fields)
    {
        Name = name;
        _fields = new List<string>(fields);
    }

    public UnavailableData(string name) : this(name, Enumerable.Empty<string>()) 
    { }

    public void Add(string field)
    {
        _fields.Add(field);
    }
}
