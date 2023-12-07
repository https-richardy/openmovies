# Get All Directors

## Endpoint

* **HTTP method**: `GET`
* **URL**: `api/directors`

## Description

Returns a list of all directors.

## Example request

```http
GET /api/directors
```

```json
[
  {
    "Id": 1,
    "FirstName": "Christopher",
    "LastName": "Nolan"
  },
  {
    "Id": 2,
    "FirstName": "Quentin",
    "LastName": "Tarantino"
  },
  // ... other directors
]
```