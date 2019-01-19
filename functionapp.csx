#r "Newtonsoft.Json"

using System.Net;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

public static async Task<IActionResult> Run(HttpRequest req, ILogger log)
{
    log.LogInformation("C# HTTP trigger function processed a request.");

    string digits = req.Query["digits"];

    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
    dynamic data = JsonConvert.DeserializeObject(requestBody);
    digits = digits ?? data?.digits;

    try
    {
        int nrOfDigits=Int32.Parse(digits);
        var stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
        string results=PiNumberFinder(nrOfDigits);
        stopwatch.Stop();
        TimeSpan ts = stopwatch.Elapsed;
        return (ActionResult)new OkObjectResult($"Time:{ts}, PI:{results} ");
    }
    catch (Exception ex)
    {
        return new BadRequestObjectResult("Wrong number in query!");
    }

    /*return digits != null
        ? (ActionResult)new OkObjectResult($"Hello, {digits}")
        : new BadRequestObjectResult("Please pass a digits to calculate PI");*/
}

    public static string PiNumberFinder(int digitNumber)
    {
        string piNumber = "3,";
        int dividedBy = 11080585;
        int divisor = 78256779;
        int result;

        for (int i = 0; i < digitNumber; i++)
        {
            if (dividedBy < divisor)
                dividedBy *= 10;

            result = dividedBy / divisor;

            string resultString = result.ToString();
            piNumber += resultString;

            dividedBy = dividedBy - divisor * result;
        }

        return piNumber;
    }
