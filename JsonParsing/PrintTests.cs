public static class PrintTests
{
    public static void PrintPersonAndCarsUsingDictionaryStringSyntax(Message message)
    {
        var personName = (string)message["PersonName"];
        var personAge = (int)message["PersonAge"];
        var personEmail = (string)message["PersonEmail"];
        var personPhone = (string)message["PersonPhone"];

        Console.WriteLine($"Name: {personName}, Age: {personAge}, Email: {personEmail}, Phone: {personPhone}");

        var carsOwned = (List<object>)message["CarsOwned"];
        foreach (IDictionary<string, object> car in carsOwned)
        {
            var make = (string)car["Make"];
            var model = (string)car["Model"];
            var year = (int)car["Year"];
            var color = (string)car["Color"];
            Console.WriteLine($"Car: {make} {model}, Year: {year}, Color: {color}");
        }
    }
    public static void PrintPersonAndCarsByNameUsingDynamicSyntax(Message message)
    {
        // Unfortunately it seems you have to do the initial cast to dynamic first,
        // but after that you can use the "field names" as property names on the class. 
        // You do not get intellisense :(

        dynamic person = (dynamic)message;
        var personName = (string)person.PersonName;
        var personAge = (int)person.PersonAge;
        var personEmail = (string)person.PersonEmail;
        var personPhone = (string)person.PersonPhone;

        Console.WriteLine($"Name: {personName}, Age: {personAge}, Email: {personEmail}, Phone: {personPhone}");

        var carsOwned = (IEnumerable<dynamic>)person.CarsOwned; // Cast to dynamic enumerable
        foreach (dynamic car in carsOwned)
        {
            var make = (string)car.Make;
            var model = (string)car.Model;
            var year = (int)car.Year;
            var color = (string)car.Color;
            Console.WriteLine($"Car: {make} {model}, Year: {year}, Color: {color}");
        }
    }

    public static void PrintPersonAndCarsByIdUsingDictionaryNumericSyntax(Message message)
    {
        var personName = (string)message[0]; // Assuming ID 0 for PersonName
        var personAge = (int)message[1]; // Assuming ID 1 for PersonAge
        var personEmail = (string)message[2]; // Assuming ID 2 for PersonEmail
        var personPhone = (string)message[3]; // Assuming ID 3 for PersonPhone

        Console.WriteLine($"Name: {personName}, Age: {personAge}, Email: {personEmail}, Phone: {personPhone}");

        var carsOwned = (List<object>)message[9]; // Assuming ID 9 for CarsOwned
        foreach (IDictionary<string, object> car in carsOwned)
        {
            var make = (string)car["Make"];
            var model = (string)car["Model"];
            var year = (int)car["Year"];
            var color = (string)car["Color"];
            Console.WriteLine($"Car: {make} {model}, Year: {year}, Color: {color}");
        }
    }

}
