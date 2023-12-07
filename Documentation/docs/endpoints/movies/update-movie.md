# Update Movie Details

## Endpoint

* **HTTP method**: `PUT`
* **URL**: `api/movies/{id}`

## Description

Update details of a specific movie identified by the provided ID. This includes modifying information such as title, release date, synopsis, director, category, trailers, and cover image.

## Path parameters

* `id` (required): The unique identifier of the movie.

## Request

### Request Headers

* `Content-Type`: `multipart/form-data`

## Request Body

* `Title` (optional): New title for the movie.
* `ReleaseDataOf` (optional): New release date for the movie.
* `Synopsis` (optional): New synopsis for the movie.
* `Cover` (optional): New cover image for the movie.
* `Trailers` (optional): List of trailers to be associated with the movie.

# Example request

```http
PUT /api/movies/1
```

## Request body:

```json
{
  "Title": "Inception 2.0",
  "ReleaseDateOf": "2022-12-01T00:00:00",
  "Synopsis": "A new twist to the dream-sharing saga.",
  "Cover": (new cover image file),
  "Trailers": [
    {
      "Type": 0,
      "Plataform": 0,
      "Link": "https://www.youtube.com/watch?v=newtrailer"
    }
  ]
}
```

## Response

* **HTTP 204 No Content**: Movie details successfully updated.

## Error response

* **HTTP 400 Bad Request**: if the request body is invalid or contains errors.

* **HTTP 404 Not Found**: If no movie is found with the specified ID.
