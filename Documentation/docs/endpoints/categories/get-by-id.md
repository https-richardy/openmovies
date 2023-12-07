# Get Category by ID

## Endpoint

* **HTTP method:** `GET`
* **URL:** `api/categories/{id}`

## Description

Retrieves details of a specific category based on the provided ID.

## Path parameters

* `id` (required): The unique identifier of the category.

## Example request

```http
GET /api/categories/1
```

## Response

* **HTTP 200 OK**: Returns details of the specified category.

```json
{
  "Id": 1,
  "Name": "Action"
}
```

## Error response

* **HTTP 404 Not Found**: If no category is found with the specified ID.