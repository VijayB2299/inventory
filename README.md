# Inventory API

## Overview
This projects manages Inventory using ASP .NET Core Web API.

## Architecture
- **Inventory.Core**
  - `Domain/` (immutable `Product`)
  - `Services/` (`IInventoryService`, `InventoryService`)
  - `Exceptions/` (`ProductNotFoundException`, `DuplicateProductNameException`)
- **InventoryApi**
  - `Controllers/`
  - `Dtos/` request/response models and validation

## Endpoints
- `GET /api/products` → 200 OK
- `GET /api/products/{id}` → 200 OK, 404 Not Found
- `POST /api/products` → 201 Created, 400 Bad Request (validation), 409 Conflict (duplicate name)
- `PUT /api/products/{id}/price` → 200 OK, 400 Bad Request (validation), 404 Not Found
- `PUT /api/products/{id}/quantity` → 200 OK, 400 Bad Request (validation), 404 Not Found
- `DELETE /api/products/{id}` → 204 No Content, 404 Not Found

## Status Codes
- **201 Created** for successful product creation
- **400 Bad Request** for invalid inputs
- **404 Not Found** when a product id does not exist
- **409 Conflict** for duplicate product names

## Run
```bash
dotnet run