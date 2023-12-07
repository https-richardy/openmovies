# Get All Categories

## Endpoint

* **HTTP method:** `GET`
* **URL:** `api/categories/`

## Description

Retrieves a list of all categories available.

## Example request

```http
GET /api/categories/
```

# Response

* **HTTP 200 OK**: Returns a list of all categories.

```json
[
  {
    "Id": 1,
    "Name": "Action"
  },
  {
    "Id": 2,
    "Name": "Comedy"
  },
  {
    "Id": 3,
    "Name": "Drama"
  }
  // ... other categories
]
```