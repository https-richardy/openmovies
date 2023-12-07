# Create Category

## Endpoint

* **HTTP method:** `POST`
* **URL:** `api/categories`

## Description

Creates a new category based on the provided data.

## Request body

* `Name` (required): The name of the category.

## Example request

```http
POST /api/categories
Content-Type: application/json

{
  "Name": "Adventure"
}
```

## Response

* **HTTP 201 Create**: Returns details of the newly created category.

```json
{
    "id": 2,
    "name": "Adventure"
}
```

## Error responses

* **HTTP 400 Bad Request**: If the request body is invalid.

* **HTTP 409 Conflict**: If a category with the same name already exists.