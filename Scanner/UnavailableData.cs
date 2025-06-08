namespace Scanner;

internal class UnavailableData
{
    private List<string> _fields;

    public string Name { get; }
    public IReadOnlyList<string> Fields => _fields;
    public bool HasUnavailableData => !_fields.IsNullOrEmpty();

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
