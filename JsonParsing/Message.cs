using Newtonsoft.Json.Linq;
using System.Dynamic;

public class Message : DynamicObject
{
    private readonly JObject _jObject;
    private readonly Dictionary<string, Mapping> _mappingsByName;
    private readonly Dictionary<int, Mapping> _mappingsById;
    private readonly Dictionary<string, object> _valueCache = new Dictionary<string, object>();

    public Message(JObject jObject, List<Mapping> mappings)
    {
        _jObject = jObject;
        _mappingsByName = mappings.ToDictionary(m => m.Name, m => m);
        _mappingsById = mappings.ToDictionary(m => m.Id, m => m);
    }

    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
        return TryGetValue(binder.Name, out result);
    }

    public object this[string name]
    {
        get
        {
            if (TryGetValue(name, out var result))
            {
                return result;
            }

            return null; // Return null if the name does not match any mapping.
        }
    }

    public object this[int id]
    {
        get
        {
            return GetByMappingId(id);
        }
    }

    private bool TryGetValue(string name, out object result)
    {
        if (_mappingsByName.TryGetValue(name, out var mapping))
        {
            result = GetValueByMapping(mapping);
            return true;
        }

        result = null; // Ensure null is returned if there is no mapping found.
        return false;
    }

    private object GetByMappingId(int id)
    {
        if (_mappingsById.TryGetValue(id, out var mapping))
        {
            return GetValueByMapping(mapping);
        }
        else
        {
            foreach (var parentMapping in _mappingsById.Values)
            {
                var result = FindNestedMappingById(parentMapping, id);
                if (result != null)
                {
                    return result;
                }
            }
        }
        return null; // Return null if no mapping with the given ID is found.
    }

    private object FindNestedMappingById(Mapping parentMapping, int id)
    {
        foreach (var nestedMapping in parentMapping.NestedMappings)
        {
            if (nestedMapping.Id == id)
            {
                return GetValueByMapping(nestedMapping);
            }
            else
            {
                var result = FindNestedMappingById(nestedMapping, id);
                if (result != null)
                {
                    return result;
                }
            }
        }
        return null;
    }

    private object GetValueByMapping(Mapping mapping)
    {
        return GetOrCreateCachedValue(mapping.JsonPath, () =>
        {
            if (mapping.NestedMappings.Any())
            {
                return _jObject.SelectTokens(mapping.JsonPath).ToList()
                    .Select(item => CreateNestedObject(item, mapping.NestedMappings)).ToList();
            }
            else
            {
                return GetValueByPath(mapping.JsonPath, mapping.Type);
            }
        });
    }

    private object GetOrCreateCachedValue(string key, Func<object> valueFactory)
    {
        if (_valueCache.TryGetValue(key, out var value))
        {
            return value;
        }

        value = valueFactory();
        _valueCache[key] = value;
        return value;
    }

    private dynamic CreateNestedObject(JToken token, List<Mapping> nestedMappings)
    {
        var nestedObject = new ExpandoObject() as IDictionary<string, Object>;
        foreach (var nestedMapping in nestedMappings)
        {
            var selectedToken = token.SelectToken(nestedMapping.JsonPath);
            nestedObject[nestedMapping.Name] = selectedToken != null ? Convert.ChangeType(selectedToken.ToObject(nestedMapping.Type), nestedMapping.Type) : null;
        }
        return nestedObject;
    }

    private object GetValueByPath(string jsonPath, Type type)
    {
        var token = _jObject.SelectToken(jsonPath);
        return token != null ? Convert.ChangeType(token.ToObject(type), type) : null;
    }
    public void PreloadAllMappings()
    {
        foreach (var mapping in _mappingsByName.Values)
        {
            // Check if the mapping has nested mappings.
            if (mapping.NestedMappings.Any())
            {
                // For nested mappings, preload each nested object.
                PreloadNestedMappings(mapping);
            }
            else
            {
                // For simple mappings, just preload the value.
                PreloadSimpleMapping(mapping);
            }
        }
    }

    private void PreloadNestedMappings(Mapping mapping)
    {
        // Resolve the collection of items for the given JSONPath.
        var items = _jObject.SelectTokens(mapping.JsonPath).ToList();
        var resolvedItems = new List<object>();

        foreach (var item in items)
        {
            var nestedObject = CreateNestedObject(item, mapping.NestedMappings);
            resolvedItems.Add(nestedObject);
        }

        // Cache the resolved collection under the mapping's name.
        _valueCache[mapping.Name] = resolvedItems;
    }

    private void PreloadSimpleMapping(Mapping mapping)
    {
        // Directly resolve and cache the value for the given JSONPath.
        var value = GetValueByPath(mapping.JsonPath, mapping.Type);
        _valueCache[mapping.Name] = value;
    }

}
