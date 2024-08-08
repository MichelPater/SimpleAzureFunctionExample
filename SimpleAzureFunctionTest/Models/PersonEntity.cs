using Azure;
using Azure.Data.Tables;

namespace SimpleAzureFunctionTest.Models;

public class PersonEntity : ITableEntity
{
    public PersonEntity(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public PersonEntity()
    {
    }

    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}