using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft;
internal class Program
{
    private static void Main(string[] args)
    {
        JObject tableJSON;
        using (StreamReader reader = File.OpenText(args[0]))
        {
            tableJSON = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
        }

        using (StreamWriter file = File.CreateText(args[1]))
        using (JsonTextWriter writer = new JsonTextWriter(file))
        {
            tableJSON.WriteTo(writer);
        }
        Dictinory<String, List<Order>> table = JsonConvert.DeserializeObject<Dictinory<string, List<Order>>>(tableJSON);
        Console.WriteLine(table["taskName"]);
        
    }
}