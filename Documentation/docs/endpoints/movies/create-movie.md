# Create Movie

## Endpoint

* **HTTP method**: `POST`
* **URL**: `api/movies`
* **Content Type**: `multipart/form-data`

## Description

Create a new movie the provided information, including title, release date, synopsis, cover image, trailers, director, and category.

## Request Body

| Parameter      | Type          | Description                                  |
| -------------- | ------------- | -------------------------------------------- |
| `Title`        | String        | The title of the movie.                      |
| `ReleaseDateOf`| DateTime      | The release date of the movie.               |
| `Synopsis`     | String        | A brief description or synopsis of the movie.|
| `Cover`        | File          | The cover image of the movie.                |
| `Trailers`     | List of Objects| A list of trailers for the movie.           |
| `DirectorId`   | Integer       | The unique identifier of the director.       |
| `CategoryId`   | Integer       | The unique identifier of the category.       |

### Trailer Object

| Parameter      | Type (Enum)   | Description                                  |
| -------------- | ------------- | -------------------------------------------- |
| `Type`         | Enum (0/1)     | The type of the trailer (0: Official, 1: Teaser).|
| `Platform`     | Enum (0/1)     | The platform where the trailer is hosted (0: Youtube, 1: Vimeo).|
| `Link`         | String        | The link to the trailer.                     |

### Example request

```http
POST /api/movies
Contenty-Type: multipart/form-data

Title=Example Film
ReleaseDateOf=2022-01-01 00:00:00
Synopsis=An example movie synopsis.
Cover=@example-cover.jpg;type=image/jpeg
Trailers[0].Type=0
Trailers[0].Plataform=0
Trailers[0].Link=https://www.youtube.com/watch?v=example
DirectorId=1
CategoryId=1
```

## Response

* **HTTP 201 Created**: Returns the details of the created movie


# Error response

* **HTTP 400 Bad Request**: If the request is invalid, providing details about the errors.

* **HTTP 404 Not Found**: If the specified director or category is not found

