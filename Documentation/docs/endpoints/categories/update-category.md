# Update Category

## Endpoint

* **HTTP method:** `PUT`
* **URL:** `api/categories/{id}`

## Description

Updates an existing category based on the provided data and category ID.

## Path parameters

* `id` (required): The unique identifier of the category.

## Request body

* `Name` (required): The updated name of the category.

## Example request

```http
PUT /api/categories/2
Content-Type: application/json

{
  "Name": "Fantasy"
}
```

## Response

* **HTTP 204 No Content**: Indicates a sucessful update.

## Error responses

* **HTTP 400 Bad Request**: If the request body is invalid.

* **HTTP 404 Not found**: If no category is found with the specified ID.
