# JsonDynamicMessage
 A utility class for parsing json messages in a dynamic and efficient way.

 This is just me fooling around. I have to parse very many Json messages (from Kafka)
 very quickly. The messages sometimes have nested fields/arrays and each message has it's own
 set of fields.

 So, I was looking for a way to parse the message once (Newtonsoft) and then to apply 
 a number of JsonPath queries to the message to find the individual fields.

 Obviously, things like the MappingsProvider is just a placeholder for a much more elaborate
 mapping system that stores mappings in the database. 

 I wanted to support 3 syntaxes:

 * message[10] : Where 10 is the key of the field in the database.
 * message["Name"] : So here we don't know the key - but we know the name.
 * message.Name : This is almost just for fun, but it does clean up the code nicely....


 This code is NOT production ready. For one thing - it doesn't handle missing fields in the Json gracefully at all. 

 So, feel free to play, use, steal or generally do as you want with this code - but at your own risk.

 
