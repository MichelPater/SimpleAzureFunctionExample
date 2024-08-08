using Azure.Data.Tables;
using SimpleAzureFunctionTest.Models;

namespace SimpleAzureFunctionTest.Services;

public class PersonTableService
{
    private const string TableName = "PersonTable";
    private string? ConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

    public PersonTableService()
    {
    }

    public async Task<IPersonResponse> AddPerson(Person person)
    {
        // Get TableClient
        var tableClient = await GetTableClientAndCreateTableIfNotExists();

        // Check for duplicates
        var queryResults = tableClient
            .Query<PersonEntity>(entity => entity.FirstName == person.FirstName && entity.LastName == person.LastName)
            .ToList();
        if (queryResults.Count > 0)
        {
            return new PersonResponses.PersonAlreadyExists { Id = queryResults.Single().RowKey };
        }

        // Create a new PersonEntity and insert it
        var newEntity = new PersonEntity
        {
            PartitionKey = person.LastName,
            RowKey = Guid.NewGuid().ToString(),
            FirstName = person.FirstName,
            LastName = person.LastName
        };

        await tableClient.AddEntityAsync(newEntity);

        return new PersonResponses.AddedPersonSucceeded { Id = newEntity.RowKey };
    }

    public async Task<IPersonResponse> GetPeople()
    {
        // Get TableClient
        var tableClient = await GetTableClientAndCreateTableIfNotExists();

        // Query all entities in the table
        var people = new List<PersonResponse>();
        await foreach (var personEntity in tableClient.QueryAsync<PersonEntity>())
        {
            people.Add(new PersonResponse()
            {
                Id = personEntity.RowKey,
                FirstName = personEntity.FirstName,
                LastName = personEntity.LastName
            });
        }

        return await Task.FromResult(new PersonResponses.GetPeopleSucceeded() { People = people });
    }

    private async Task<TableClient> GetTableClientAndCreateTableIfNotExists()
    {
        var tableClient = new TableClient(ConnectionString, TableName);
        await tableClient.CreateIfNotExistsAsync();
        return tableClient;
    }
}