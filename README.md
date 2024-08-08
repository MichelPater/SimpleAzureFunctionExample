
# Simple Azure Function Example to Manage Person Data in Table Storage

This project contains a (possible) Azure Functions solution to handle person objects in Azure Table Storage. It includes two endpoints: one for adding a person and another for retrieving all stored persons.

## Endpoints

### Add Person

**HTTP Method:** POST  
**URL:** `/api/AddPerson`

**Request Body:**
```json
{
  "FirstName": "John",
  "LastName": "Doe"
}

```

Response:

    200 OK with the ID of the saved person.
    If the person already exists (unique on FirstName and LastName), returns the ID of the existing entry.

###Retrieve All Persons

HTTP Method: GET
URL: /api/GetPeople
```json
[
  {
    "Id": "unique-id-1",
    "FirstName": "John",
    "LastName": "Doe"
  },
  {
    "Id": "unique-id-2",
    "FirstName": "Jane",
    "LastName": "Smith"
  }
]
```

#Requirements

    Each person must be unique based on the combination of FirstName and LastName.
    If a duplicate person is detected, the ID of the existing entry should be returned.
    Provide an endpoint to retrieve all stored persons, including their IDs.

#Implementation
##Add Person Function

    Function Name: HttpTriggerFunction
    Trigger: HTTP POST
    Description: Receives a JSON object with FirstName and LastName, checks for duplicates, and stores the person in Azure Table Storage. Returns the ID of the saved or existing person.

##Retrieve All Persons Function

    Function Name: GetPersonsFunction
    Trigger: HTTP GET
    Description: Queries Azure Table Storage and returns a list of all stored persons with their IDs, FirstNames, and LastNames.