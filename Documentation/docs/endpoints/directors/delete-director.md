# Delete Director by ID

## Endpoint

* **HTTP method**: `DELETE`
* **URL**: `api/directors/{id}`

## Description

Deletes a director based on the provided ID.

## Path parameters

* `id` (required): The unique identifier of the director.

## Example request

```http
DELETE /api/directors/1
```

## Response

* **HTTP 204 No Content**: The director is successfully deleted.

## Error response

* **HTTP 404 Not Found**: If no director is found with the specified ID.