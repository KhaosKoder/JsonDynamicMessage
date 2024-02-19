public class Mapping
{
    public int Id { get; }
    public string Name { get; }
    public string JsonPath { get; }
    public Type Type { get; }
    public List<Mapping> NestedMappings { get; }

    public Mapping(int id, string name, string jsonPath, Type type, List<Mapping> nestedMappings = null)
    {
        Id = id;
        Name = name;
        JsonPath = jsonPath;
        Type = type;
        NestedMappings = nestedMappings ?? new List<Mapping>();
    }
}
