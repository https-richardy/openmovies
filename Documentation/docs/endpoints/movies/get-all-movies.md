# Get all movies

## Endpoint

* **HTTP Method**: `GET`
* **URL**: `api/movies/`

## Description

Retrieves a paginated list of all movies.

## Query Parameters

* `pageNumber` (optional): Page number of pagination (default: 1).
* `pageSize` (optional): Number of items per page (default: 10).

# Response

* **HTTP 200 OK**: Returns a paginated list of movies.

```json
{
  "Count": 20,
  "Next": "/api/movies/?pageNumber=3",
  "Previous": "/api/movies/?pageNumber=1",
  "Results": [
    {
      "Id": 1,
      "Title": "Movie Title",
      "ReleaseDateOf": "2022-01-01T00:00:00",
      "ReleaseYear": 2022,
      "Synopsis": "Movie synopsis...",
      "Trailers": [],
      "Director": {
        "Id": 1,
        "FirstName": "John",
        "LastName": "Doe"
      },
      "Category": {
        "Id": 1,
        "Name": "Action"
      },
      "CoverImagePath": "/images/movie_cover.jpg"
    },
    // ... additional movies ...
  ]
}
```