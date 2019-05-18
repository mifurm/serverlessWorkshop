//how to references external dll's
#r "Newtonsoft.Json"
//how to references external dll's

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

//stage1 - add output bindings
public static async Task<IActionResult> Run(HttpRequest req, ILogger log, ICollector<CalcResult> outputTable, ICollector<string> outputQueueItem )
//stage1 - add output bindings
{
    log.LogInformation("C# HTTP trigger function processed a request.");

    string name = req.Query["name"];

    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
    dynamic data = JsonConvert.DeserializeObject(requestBody);
    name = name ?? data?.name;

    //stage0 - get env. name
    var envName = Environment.GetEnvironmentVariable("eventName");
    log.LogInformation("EventName:{0}",envName);
    //stage0 - get env. name

    //stage2 - add output data to the queue
    string msg=String.Format("MSG:{0},TIME:{1}, ENV:{2}",name,DateTime.Now.ToShortDateString() ,envName);
    outputQueueItem.Add(msg);
    //stage2 - add output data to the queue
    
    //stage4 - add output data written to the table
    outputTable.Add(new CalcResult()
        {
            PartitionKey="Result", 
            RowKey=Guid.NewGuid().ToString(), 
            Message=msg
        }
    );
    //stage4 - add output data written to the table
    
    return name != null
        ? (ActionResult)new OkObjectResult($"Hello, {name}")
        : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
}

//stage3 - add type to describe data written to the table
public class CalcResult
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public string Message { get; set; }
}
//stage3 - add type to describe data written to the table
