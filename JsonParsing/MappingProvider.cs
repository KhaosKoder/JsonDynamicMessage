

public static class MappingProvider
{
    public static List<Mapping> GetMappings()
    {
        var mappings = new List<Mapping>
        {
    new Mapping(0, "PersonName", "$.Person.Name", typeof(string)),
    new Mapping(1, "PersonAge", "$.Person.Age", typeof(int)),
    new Mapping(2, "PersonEmail", "$.Person.Contact.Email", typeof(string)),
    new Mapping(3, "PersonPhone", "$.Person.Contact.Phone", typeof(string)),
    new Mapping(4, "PersonAddress", "$.Person.Address", typeof(object), new List<Mapping>
    {
        new Mapping(5, "Street", "$.Street", typeof(string)),
        new Mapping(6, "City", "$.City", typeof(string)),
        new Mapping(7, "State", "$.State", typeof(string)),
        new Mapping(8, "ZipCode", "$.ZipCode", typeof(string))
    }),
    new Mapping(9, "CarsOwned", "$.Person.CarsOwned[*]", typeof(List<object>), new List<Mapping>
    {
        new Mapping(10, "Make", "$.Make", typeof(string)),
        new Mapping(11, "Model", "$.Model", typeof(string)),
        new Mapping(12, "Year", "$.Year", typeof(int)),
        new Mapping(13, "Color", "$.Color", typeof(string))
    })
};
        return mappings;

    }
}
