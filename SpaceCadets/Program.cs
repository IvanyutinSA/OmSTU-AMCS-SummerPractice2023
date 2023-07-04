using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SpaceCadets;
class Program
{
    static void Main(string[] args)
    {
        Dictionary<string, List<object>> task = new Dictionary<string, List<object>>();
        
        JObject tableJSON;
        using (StreamReader reader = File.OpenText(args[0]))
        {
            tableJSON = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
        }

        string taskName = tableJSON["taskName"]?.ToString() ?? "";
        List<AcademicPerformance> data = tableJSON["data"]?.ToObject<List<AcademicPerformance>>() ??
            new List<AcademicPerformance>();
    

        var taskMethod = new Dictionary<string, Func<List<AcademicPerformance>,string>>();
        
        taskMethod["CalculateGPAByDiscipline"] = CalculateGPAByDiscipline;
        taskMethod["GetBestGroupsByDiscipline"] = GetBestGroupsByDiscipline;
        taskMethod["GetStudentsWithHighestGPA"] = GetStudentsWithHighestGPA;

        var taskResult = JObject.Parse(taskMethod[taskName](data));

        using (StreamWriter file = File.CreateText(args[1]))
        using (JsonTextWriter writer = new JsonTextWriter(file))
        {
            // tableJSON.WriteTo(writer);
            taskResult.WriteTo(writer);
            
        } 

        // Console.WriteLine(table["taskName"]);
        // using(JsonTextReader reader = new JsonTextReader(new StringReader()))
    }
    
    public static string CalculateGPAByDiscipline(List<AcademicPerformance> data)
    {
        var result = data.GroupBy(x => x.Discipline)
                         .Select(x => new Dictionary<string, double> {[x.Key]=x.Average(x => x.Mark)});
        var response = new Dictionary<string, object>() {["Response"]=result};

        return JsonConvert.SerializeObject(response); 
    }

    public static string GetBestGroupsByDiscipline(List<AcademicPerformance> data)
    {
        var result = data
            .GroupBy(aPer => aPer.Discipline)
            .Select(
                discipline => new {
                    Discipline = discipline.Key,
                    GoupAndDPA = discipline.GroupBy(aPer => aPer.Group)
                                           .Select(
                                                group => new {
                                                    Group = group.Key,
                                                    GPA = group.Average(aPref => aPref.Mark)
                                                }
                                           )
                                           .OrderByDescending(x => x.GPA)
                                           .First()
                }
            )
            .Select(
                pair => new {
                    Discipline = pair.Discipline,
                    Group = pair.GoupAndDPA.Group,
                    GPA = pair.GoupAndDPA.GPA
                }
            );
        
        var response = new {Response = result};        
        return JsonConvert.SerializeObject(response);
    }

    public static string GetStudentsWithHighestGPA(List<AcademicPerformance> data)
    {
        var result = data
            .GroupBy(aPer => aPer.Name)
            .Select(
                cadet => new {
                    Cadet = cadet.Key,
                    GPA = cadet.Average(x => x.Mark)
                }
            )
            .GroupBy(x => x.GPA)
            .OrderByDescending(x => x.Key)
            .First();

        var response = new {Response = result};
        return JsonConvert.SerializeObject(response);
        
    }
}

class AcademicPerformance
{
    public string Name {get; set;} = "Undefined";
    public string Group {get; set;} = "Undefined";
    public string Discipline {get; set;} = "Undefined";
    public int Mark {get; set;}
}

