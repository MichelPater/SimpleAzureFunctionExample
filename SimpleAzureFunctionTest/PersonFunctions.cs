using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SimpleAzureFunctionTest.Models;
using SimpleAzureFunctionTest.Services;

namespace SimpleAzureFunctionTest;

public class PersonFunctions
{
    [Function("AddPerson")]
    public async Task<IActionResult> AddPerson(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
    {
        // Try to read post data with Person info
        var person = await JsonSerializer.DeserializeAsync<Person>(req.Body);

        if (person == null)
        {
            // Person data not present or malformed
            return new BadRequestResult();
        }
        
        // interact with tablestorage
        var personTableStorageService = new PersonTableService();

        var result = await personTableStorageService.AddPerson(person);

        return result switch
        {
            PersonResponses.FailedToAddPerson _ => new BadRequestResult(),
            PersonResponses.PersonAlreadyExists personAlreadyExists => new OkObjectResult(JsonSerializer.Serialize(personAlreadyExists.Id)),
            PersonResponses.AddedPersonSucceeded succeeded => new OkObjectResult(JsonSerializer.Serialize(succeeded.Id)),
            _ => throw new Exception(nameof(result))
        };
    }

    [Function("GetPeople")]
    public async Task<IActionResult> GetPeople(
        [HttpTrigger(AuthorizationLevel.Function, "get")]
        HttpRequestData req)
    {
        var personTableStorageService = new PersonTableService();
        var result = await personTableStorageService.GetPeople();
        
        return result switch
        {
            PersonResponses.FailedToGetPeople _ => new BadRequestResult(),
            PersonResponses.GetPeopleSucceeded succeeded => new OkObjectResult(JsonSerializer.Serialize(succeeded.People)),
            _ => throw new Exception(nameof(result))
        };
    }
}