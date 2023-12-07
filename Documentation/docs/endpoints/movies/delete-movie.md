# Delete Movie

## Endpoint

* **HTTP method**: `DELETE`
* **URL**: `api/movies/{id}`

## Description

Delete a specific movie based on the provided ID.

## Path parameters

* `id` (required): The unique identifier of the movie.

## Example request

```http
DELETE api/movies/1
```

## Response

* **HTTP 204 No Content**: Indicates successful deletion of the specified movie.

## Error response

* **HTTP 404 Not Found**: If no movies is found with the specified ID.
