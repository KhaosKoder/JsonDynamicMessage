using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main()
    {
        var json = System.IO.File.ReadAllText("data.json");

        var mappings = MappingProvider.GetMappings();


        var jObject = JObject.Parse(json);
        var message = new Message(jObject, mappings);
       
        // Test it with this line and without.
        // Basically, this line makes it parse the message immediately (as we currently do).
        // Removing this line makes it parse the message only if/when a field is actually needed. 
        // So, it should speed up when this line is removed, but it makes handling missing fields harder - since you won't know the fields are missing until the moment that you want to write the record to SQL.
        message.PreloadAllMappings();


        Console.WriteLine(); Console.WriteLine(message);
        Console.WriteLine("DICTIONARY STRING SYNTAX");
        PrintTests.PrintPersonAndCarsUsingDictionaryStringSyntax(message);

        Console.WriteLine(); Console.WriteLine(message);
        Console.WriteLine("DICTIONARY NUMERIC SYNTAX");
        PrintTests.PrintPersonAndCarsByIdUsingDictionaryNumericSyntax(message);

        Console.WriteLine(); Console.WriteLine(message);
        Console.WriteLine("DYNAMIC NAMED SYNTAX");
        PrintTests.PrintPersonAndCarsByNameUsingDynamicSyntax(message);
    }
}
