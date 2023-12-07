# Create Director

## Endpoint

* **HTTP method**: `POST`
* **URL**: `api/directors/`

## Description

Creates a new director based on the provided data.

## Request

### Body

The request body should contain JSON data with the following properties:

- `FirstName` (string, required): First name of the director.
- `LastName` (string, required): Last name of the director.

Example:

```json
{
  "FirstName": "Christopher",
  "LastName": "Nolan"
}
```

## Response

* **HTTP 201 Created**: Returns the created director.

```json
{
    "id": 1,
    "firstName": "Christopher",
    "lastName": "Nolan"
}
```

## Error response

* **HTTP 400 Bad Request**: If the request body is invalid or missing required properties.

* **HTTP 409 Conflict**: If there is a conflict, such as trying to create a director with a duplicate name
