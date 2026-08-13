# ParseContent API

Minimal .NET API for parsing Base64-encoded content as CSV or JSON.

## Requirements

- [.NET SDK 9.0+](https://dotnet.microsoft.com/download)

## Run locally

```bash
dotnet run
```

## Endpoint

### `POST /api/v1/parse-content`

**Request:**

```json
{
  "Type": "CSV",
  "Content": "bmFtZSxhZ2UKSm9obiwzMA=="
}
```

`Type`: `"CSV"` or `"INTERNAL_JSON"`
`Content`: Base64-encoded payload

**Response (200):**

```json
{ "count": 1, "records": [ ... ] }
```
