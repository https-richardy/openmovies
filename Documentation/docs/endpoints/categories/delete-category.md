# Delete Category

Deletes a category based on the provided ID.

## Endpoint

* **HTTP method:** `DELETE`
* **URL:** `/api/categories/{id}`

## Path parameters

* `id` (required): The unique identifier of the category.

## Example request

```http
DELETE /api/categories/1
```

## Response

* **HTTP 204 No Content**: Successfully deletes the category.

## Error response

* **HTTP 404 Not Found**: If no category is found with the specified ID.