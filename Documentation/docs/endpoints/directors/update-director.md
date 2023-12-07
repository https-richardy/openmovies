# Update Director by ID

## Endpoint

* **HTTP method**: `PUT`
* **URL**: `api/directors/{id}`

## Description

Updates the information of a specific director based on the provided ID.

## Path parameters

* `id` (required): The unique identifier of the director.

## Example request

```http
PUT /api/directors/1
Content-Type: application/json

{
  "firstName": "UpdatedFirstName",
  "lastName": "UpdatedLastName"
}
```

## Response

* **HTTP 204 No Content**: The director information is successfully updated.

## Error response

* **HTTP 400 Bad Request**: If the request payload is invalid.