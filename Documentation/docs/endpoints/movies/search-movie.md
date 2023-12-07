# Search Movies

## Endpoint

* **HTTP method**: `GET`
* **URL**: `api/movies/search`

## Description

Search for movies based on specified criteria such as name, release year, and category. Retrieve a paginated list of matching movies

### Query Parameters

* `name` (optional): Movie name to search for.
* `releaseYear` (optional): Release year of the movie.
* `categoryId` (optional): ID of the category to filter by.
* `pageNumber` (optional): Page number for pagination (default is 1).
* `pageSize` (optional): Number of items per page (default is 10).

## Example request

```http
GET /api/movies/search?name=Inception&releaseYear=2010&categoryId=1&pageNumber=1&pageSize=10
```

## Response

* **HTTP 200 OK**: Returns a paginated list of movies that match the search criteria.

```json
{
  "count": 1,
  "next": null,
  "previous": null,
  "results": [
    {
      "title": "Spider-Man: Homecoming",
      "releaseDateOf": "2017-07-07T00:00:00",
      "releaseYear": 2017,
      "synopsis": "In Spider-Man: Homecoming, after acting alongside the Avengers, the time has come for Peter Parker (Tom Holland) to return home and to his life, which is no longer so normal.",
      "trailers": [
        {
          "type": 0,
          "plataform": 0,
          "link": "https://www.youtube.com/watch?v=rk-dF1lIbIg",
          "id": 2
        }
      ],
      "director": {
        "firstName": "Jon",
        "lastName": "Watts",
        "id": 1
      },
      "category": {
        "name": "Action",
        "id": 1
      },
      "coverImagePath": "images/cover_image.jpg",
      "id": 1
    }
  ]
}
```

## Error response

* **HTTP 500 Internal Server Error**: If an unexpected error occurs during the search.
