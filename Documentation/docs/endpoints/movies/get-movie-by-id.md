# Get movie by ID

## Endpoint

* **HTTP method**: `GET`
* **URL**: `api/movies/{id}`

## Description

Retrieves details of a specific movie based on the provided ID. This includes information about the movie and links to embedded trailers.

## Path parameters

* `id` (required): The unique identifier of the movie.

## Example request

```http
GET /api/movies/1
```

## Response

* **HTTP 200 OK**: Returns details of the specified movie, including embedded links for each trailer. 

```json
{
  "Id": 1,
  "Title": "Inception",
  "ReleaseDateOf": "2010-07-16T00:00:00",
  "ReleaseYear": 2010,
  "Synopsis": "A thief who steals corporate secrets through the use of dream-sharing technology.",
  "Trailers": [
    {
      "Id": 1,
      "Type": 0,
      "Platform": 0,
      "Link": "https://www.youtube.com/embed/d3A3-zSOBT4",
    }
  ],
  "Director": {
    "Id": 1,
    "FirstName": "Christopher",
    "LastName": "Nolan"
  },
  "Category": {
    "Id": 1,
    "Name": "Sci-Fi"
  },
  "CoverImagePath": "/images/inception_cover.jpg"
}
```

## Error response

* **HTTP 404 Not Found**: If no movie is found with the specified ID.