namespace SimpleAzureFunctionTest.Models;

public interface IPersonResponse
{
}

public class PersonResponses : IPersonResponse
{
    public class FailedToAddPerson : IPersonResponse
    {
    }

    public class AddedPersonSucceeded : IPersonResponse
    {
        public string Id { get; set; }
    }
    
    public class PersonAlreadyExists : IPersonResponse
    {
        public string Id { get; set; }
    }

    public class FailedToGetPeople : IPersonResponse
    {
    }

    public class GetPeopleSucceeded : IPersonResponse
    {
        public List<PersonResponse> People { get; set; }
    }
}