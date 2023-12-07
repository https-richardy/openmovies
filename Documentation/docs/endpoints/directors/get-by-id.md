# Get Director by ID

## Endpoint

* **HTTP method**: `GET`
* **URL**: `api/directors/{id}`

## Description

Retrieves details of a specific director based on the provided ID.

## Path parameters

* `id` (required): The unique identifier of the director.

## Example request

```http
GET /api/directors/1
```

## Response

* **HTTP 200 OK**: Returns details of the specified director.

```json
{
    "id": 1,
    "firstName": "Christopher",
    "lastName": "Nolan"
}
```

## Error response

* **HTTP 404 NotFound**: If no director is found with the specified ID.