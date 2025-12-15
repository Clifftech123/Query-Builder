# Query Builder API Documentation

## Overview
This API provides a SQL Server Management Studio-like interface for managing industries, products, configurations, and executing SQL queries. The system allows users to upload Excel files with configuration data and execute raw SQL queries against dynamically created tables.

**Base URL:** `http://localhost:5000/api`

---

## Table of Contents
1. [Industry Management](#industry-management)
2. [Product Management](#product-management)
3. [Product SubType Management](#product-subtype-management)
4. [Configuration Management](#configuration-management)
5. [Excel Upload](#excel-upload)
6. [SQL Query Execution](#sql-query-execution)
7. [Workflow Example](#workflow-example)
8. [API Response Format](#api-response-format)

---

## API Response Format

All endpoints return a consistent response format:

```json
{
  "success": true,
  "message": "Operation successful",
  "data": { /* response data */ },
  "statusCode": 200
}
```

**Error Response:**
```json
{
  "success": false,
  "message": "Error description",
  "data": null,
  "statusCode": 400
}
```

---

## Industry Management

Industries are the top-level hierarchy. Each industry can have multiple products.

### 1. Create Industry
**Endpoint:** `POST /api/Industry`

**Request Body:**
```json
{
  "name": "Banking"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Industry created successfully",
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "name": "Banking",
    "createdAt": "2025-12-15T10:30:00Z"
  },
  "statusCode": 201
}
```

**cURL Example:**
```bash
curl -X POST "http://localhost:5000/api/Industry" \
  -H "Content-Type: application/json" \
  -d '{"name": "Banking"}'
```

### 2. Get All Industries
**Endpoint:** `GET /api/Industry`

**Response:**
```json
{
  "success": true,
  "message": "Industries retrieved successfully",
  "data": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "name": "Banking",
      "createdAt": "2025-12-15T10:30:00Z"
    }
  ],
  "statusCode": 200
}
```

**cURL Example:**
```bash
curl -X GET "http://localhost:5000/api/Industry"
```

### 3. Get Industry by ID
**Endpoint:** `GET /api/Industry/{id}`

**Response:**
```json
{
  "success": true,
  "message": "Industry retrieved successfully",
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "name": "Banking",
    "createdAt": "2025-12-15T10:30:00Z"
  },
  "statusCode": 200
}
```

**cURL Example:**
```bash
curl -X GET "http://localhost:5000/api/Industry/3fa85f64-5717-4562-b3fc-2c963f66afa6"
```

### 4. Update Industry
**Endpoint:** `PUT /api/Industry/{id}`

**Request Body:**
```json
{
  "name": "Banking & Finance"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Industry updated successfully",
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "name": "Banking & Finance",
    "createdAt": "2025-12-15T10:30:00Z",
    "updatedAt": "2025-12-15T11:00:00Z"
  },
  "statusCode": 200
}
```

**cURL Example:**
```bash
curl -X PUT "http://localhost:5000/api/Industry/3fa85f64-5717-4562-b3fc-2c963f66afa6" \
  -H "Content-Type: application/json" \
  -d '{"name": "Banking & Finance"}'
```

### 5. Delete Industry
**Endpoint:** `DELETE /api/Industry/{id}`

**Response:**
```json
{
  "success": true,
  "message": "Industry deleted successfully",
  "data": null,
  "statusCode": 200
}
```

**cURL Example:**
```bash
curl -X DELETE "http://localhost:5000/api/Industry/3fa85f64-5717-4562-b3fc-2c963f66afa6"
```

---

## Product Management

Products belong to an Industry. Each product can have multiple product subtypes.

### 1. Create Product
**Endpoint:** `POST /api/Product`

**Request Body:**
```json
{
  "name": "Credit Card",
  "industryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Product created successfully",
  "data": {
    "id": "4fb85f64-5717-4562-b3fc-2c963f66afa7",
    "name": "Credit Card",
    "industryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "industryName": "Banking",
    "createdAt": "2025-12-15T10:35:00Z"
  },
  "statusCode": 201
}
```

**cURL Example:**
```bash
curl -X POST "http://localhost:5000/api/Product" \
  -H "Content-Type: application/json" \
  -d '{"name": "Credit Card", "industryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"}'
```

### 2. Get All Products
**Endpoint:** `GET /api/Product`

**Response:**
```json
{
  "success": true,
  "message": "Products retrieved successfully",
  "data": [
    {
      "id": "4fb85f64-5717-4562-b3fc-2c963f66afa7",
      "name": "Credit Card",
      "industryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "industryName": "Banking",
      "createdAt": "2025-12-15T10:35:00Z"
    }
  ],
  "statusCode": 200
}
```

**cURL Example:**
```bash
curl -X GET "http://localhost:5000/api/Product"
```

### 3. Get Product by ID
**Endpoint:** `GET /api/Product/{id}`

### 4. Update Product
**Endpoint:** `PUT /api/Product/{id}`

**Request Body:**
```json
{
  "name": "Premium Credit Card",
  "industryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

### 5. Delete Product
**Endpoint:** `DELETE /api/Product/{id}`

---

## Product SubType Management

Product SubTypes belong to a Product.

### 1. Create Product SubType
**Endpoint:** `POST /api/ProductSubType`

**Request Body:**
```json
{
  "name": "Platinum",
  "productId": "4fb85f64-5717-4562-b3fc-2c963f66afa7"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Product SubType created successfully",
  "data": {
    "id": "5fc85f64-5717-4562-b3fc-2c963f66afa8",
    "name": "Platinum",
    "productId": "4fb85f64-5717-4562-b3fc-2c963f66afa7",
    "productName": "Credit Card",
    "createdAt": "2025-12-15T10:40:00Z"
  },
  "statusCode": 201
}
```

**cURL Example:**
```bash
curl -X POST "http://localhost:5000/api/ProductSubType" \
  -H "Content-Type: application/json" \
  -d '{"name": "Platinum", "productId": "4fb85f64-5717-4562-b3fc-2c963f66afa7"}'
```

### 2. Get All Product SubTypes
**Endpoint:** `GET /api/ProductSubType`

### 3. Get Product SubType by ID
**Endpoint:** `GET /api/ProductSubType/{id}`

### 4. Update Product SubType
**Endpoint:** `PUT /api/ProductSubType/{id}`

### 5. Delete Product SubType
**Endpoint:** `DELETE /api/ProductSubType/{id}`

---

## Configuration Management

Configurations link Industry, Product, and ProductSubType together. They are used when uploading Excel files.

### 1. Create Configuration
**Endpoint:** `POST /api/Configuration`

**Request Body:**
```json
{
  "industryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "productId": "4fb85f64-5717-4562-b3fc-2c963f66afa7",
  "productSubTypeId": "5fc85f64-5717-4562-b3fc-2c963f66afa8",
  "configurationType": 1,
  "transactionMode": 1
}
```

**Field Descriptions:**
- `configurationType`: `1` = Transaction
- `transactionMode`: `1` = ExcelUpload, `2` = API

**Response:**
```json
{
  "success": true,
  "message": "Configuration created successfully",
  "data": {
    "id": "6fd85f64-5717-4562-b3fc-2c963f66afa9",
    "industryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "industryName": "Banking",
    "productId": "4fb85f64-5717-4562-b3fc-2c963f66afa7",
    "productName": "Credit Card",
    "productSubTypeId": "5fc85f64-5717-4562-b3fc-2c963f66afa8",
    "productSubTypeName": "Platinum",
    "configurationType": 1,
    "transactionMode": 1,
    "createdAt": "2025-12-15T10:45:00Z"
  },
  "statusCode": 201
}
```

**cURL Example:**
```bash
curl -X POST "http://localhost:5000/api/Configuration" \
  -H "Content-Type: application/json" \
  -d '{
    "industryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "productId": "4fb85f64-5717-4562-b3fc-2c963f66afa7",
    "productSubTypeId": "5fc85f64-5717-4562-b3fc-2c963f66afa8",
    "configurationType": 1,
    "transactionMode": 1
  }'
```

### 2. Get All Configurations
**Endpoint:** `GET /api/Configuration`

**Response:**
```json
{
  "success": true,
  "message": "Configurations retrieved successfully",
  "data": [
    {
      "id": "6fd85f64-5717-4562-b3fc-2c963f66afa9",
      "industryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "industryName": "Banking",
      "productId": "4fb85f64-5717-4562-b3fc-2c963f66afa7",
      "productName": "Credit Card",
      "productSubTypeId": "5fc85f64-5717-4562-b3fc-2c963f66afa8",
      "productSubTypeName": "Platinum",
      "configurationType": 1,
      "transactionMode": 1,
      "createdAt": "2025-12-15T10:45:00Z"
    }
  ],
  "statusCode": 200
}
```

### 3. Get Configuration by ID
**Endpoint:** `GET /api/Configuration/{id}`

### 4. Update Configuration
**Endpoint:** `PUT /api/Configuration/{id}`

### 5. Delete Configuration
**Endpoint:** `DELETE /api/Configuration/{id}`

---

## Excel Upload

Upload Excel files to create dynamic database tables. The system automatically adds Industry, Product, and ProductSubType columns to the uploaded data.

### Upload Excel File
**Endpoint:** `POST /api/Excel/upload`

**Content-Type:** `multipart/form-data`

**Form Parameters:**
- `file`: Excel file (.xlsx)
- `configurationId`: GUID of the configuration

**Expected Excel Format:**
The Excel file should have columns starting from column 4 (A, B, C are reserved for Industry, Product, ProductSubType which will be added automatically):

Example Excel:
| Column D | Column E | Column F | Column G |
|----------|----------|----------|----------|
| CustomerName | Amount | Date | Status |
| John Doe | 1000 | 2025-01-15 | Active |
| Jane Smith | 2500 | 2025-01-16 | Active |

**What Happens:**
The system will automatically insert 3 columns at the beginning:
- Column A: Industry (from configuration)
- Column B: Product (from configuration)
- Column C: ProductSubType (from configuration)

Final table will look like:
| Industry | Product | ProductSubType | CustomerName | Amount | Date | Status |
|----------|---------|----------------|--------------|--------|------|--------|
| Banking | Credit Card | Platinum | John Doe | 1000 | 2025-01-15 | Active |
| Banking | Credit Card | Platinum | Jane Smith | 2500 | 2025-01-16 | Active |

**Response:**
```json
{
  "success": true,
  "message": "Excel uploaded successfully with configuration data",
  "data": {
    "success": true,
    "message": "Excel uploaded successfully with configuration data",
    "tableName": "Upload_6fd85f645717",
    "rowCount": 2,
    "columns": [
      {
        "columnName": "Industry",
        "dataType": "String",
        "columnNumber": 1
      },
      {
        "columnName": "Product",
        "dataType": "String",
        "columnNumber": 2
      },
      {
        "columnName": "ProductSubType",
        "dataType": "String",
        "columnNumber": 3
      },
      {
        "columnName": "CustomerName",
        "dataType": "String",
        "columnNumber": 4
      },
      {
        "columnName": "Amount",
        "dataType": "Double",
        "columnNumber": 5
      },
      {
        "columnName": "Date",
        "dataType": "DateTime",
        "columnNumber": 6
      },
      {
        "columnName": "Status",
        "dataType": "String",
        "columnNumber": 7
      }
    ],
    "validationErrors": []
  },
  "statusCode": 200
}
```

**Important Notes:**
- The file is saved to `UploadedFiles` folder for audit purposes
- A dynamic table is created in the database with the name pattern: `Upload_{configurationId}_{timestamp}`
- Industry, Product, and ProductSubType values are automatically populated from the configuration
- The table name is returned in the response for use in SQL queries

**cURL Example:**
```bash
curl -X POST "http://localhost:5000/api/Excel/upload" \
  -F "file=@/path/to/your/file.xlsx" \
  -F "configurationId=6fd85f64-5717-4562-b3fc-2c963f66afa9"
```

---

## SQL Query Execution

Execute raw SQL queries against the database (similar to SQL Server Management Studio).

### 1. Execute SQL Query
**Endpoint:** `POST /api/SqlQuery/execute`

**Request Body:**
```json
{
  "query": "SELECT * FROM Upload_6fd85f645717 WHERE Amount > 1500"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Query executed successfully",
  "data": {
    "success": true,
    "message": "Query executed successfully",
    "rowCount": 1,
    "columns": [
      "Industry",
      "Product",
      "ProductSubType",
      "CustomerName",
      "Amount",
      "Date",
      "Status"
    ],
    "rows": [
      {
        "Industry": "Banking",
        "Product": "Credit Card",
        "ProductSubType": "Platinum",
        "CustomerName": "Jane Smith",
        "Amount": 2500,
        "Date": "2025-01-16T00:00:00",
        "Status": "Active"
      }
    ],
    "executionTime": "00:00:00.1234567"
  },
  "statusCode": 200
}
```

**Supported Query Types:**
- `SELECT` queries: Returns data with columns and rows
- `INSERT`, `UPDATE`, `DELETE`: Returns rows affected count
- `CREATE TABLE`, `ALTER TABLE`, `DROP TABLE`: DDL operations
- Any valid T-SQL query

**cURL Example:**
```bash
curl -X POST "http://localhost:5000/api/SqlQuery/execute" \
  -H "Content-Type: application/json" \
  -d '{"query": "SELECT * FROM Upload_6fd85f645717 WHERE Amount > 1500"}'
```

### 2. Get Database Metadata
**Endpoint:** `GET /api/SqlQuery/metadata`

**Response:**
```json
{
  "success": true,
  "message": "Metadata retrieved successfully",
  "data": {
    "tables": [
      {
        "tableName": "Industries",
        "columns": [
          {
            "columnName": "Id",
            "dataType": "uniqueidentifier",
            "isNullable": false
          },
          {
            "columnName": "Name",
            "dataType": "nvarchar",
            "isNullable": false,
            "maxLength": 100
          },
          {
            "columnName": "CreatedAt",
            "dataType": "datetime2",
            "isNullable": false
          }
        ]
      },
      {
        "tableName": "Upload_6fd85f645717",
        "columns": [
          {
            "columnName": "Industry",
            "dataType": "nvarchar",
            "isNullable": true
          },
          {
            "columnName": "Product",
            "dataType": "nvarchar",
            "isNullable": true
          },
          {
            "columnName": "Amount",
            "dataType": "float",
            "isNullable": true
          }
        ]
      }
    ],
    "configurations": [
      {
        "id": "6fd85f64-5717-4562-b3fc-2c963f66afa9",
        "industry": "Banking",
        "product": "Credit Card",
        "productSubType": "Platinum"
      }
    ]
  },
  "statusCode": 200
}
```

**Purpose:**
- Get list of all tables in the database
- Get column information for each table
- Get available configurations
- Useful for building query builders and autocomplete in the frontend

**cURL Example:**
```bash
curl -X GET "http://localhost:5000/api/SqlQuery/metadata"
```

---

## Workflow Example

### Complete End-to-End Workflow

#### Step 1: Create Industry
```bash
curl -X POST "http://localhost:5000/api/Industry" \
  -H "Content-Type: application/json" \
  -d '{"name": "Banking"}'
```
**Save the returned `id` (e.g., `industryId`)**

#### Step 2: Create Product
```bash
curl -X POST "http://localhost:5000/api/Product" \
  -H "Content-Type: application/json" \
  -d '{"name": "Credit Card", "industryId": "{industryId}"}'
```
**Save the returned `id` (e.g., `productId`)**

#### Step 3: Create Product SubType
```bash
curl -X POST "http://localhost:5000/api/ProductSubType" \
  -H "Content-Type: application/json" \
  -d '{"name": "Platinum", "productId": "{productId}"}'
```
**Save the returned `id` (e.g., `productSubTypeId`)**

#### Step 4: Create Configuration
```bash
curl -X POST "http://localhost:5000/api/Configuration" \
  -H "Content-Type: application/json" \
  -d '{
    "industryId": "{industryId}",
    "productId": "{productId}",
    "productSubTypeId": "{productSubTypeId}",
    "configurationType": 1,
    "transactionMode": 1
  }'
```
**Save the returned `id` (e.g., `configurationId`)**

#### Step 5: Upload Excel File
```bash
curl -X POST "http://localhost:5000/api/Excel/upload" \
  -F "file=@transactions.xlsx" \
  -F "configurationId={configurationId}"
```
**Save the returned `tableName` (e.g., `Upload_6fd85f645717`)**

#### Step 6: Query the Uploaded Data
```bash
curl -X POST "http://localhost:5000/api/SqlQuery/execute" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "SELECT Industry, Product, ProductSubType, CustomerName, Amount FROM Upload_6fd85f645717 WHERE Amount > 1000"
  }'
```

#### Step 7: Get Database Metadata (Optional)
```bash
curl -X GET "http://localhost:5000/api/SqlQuery/metadata"
```

---

## Error Handling

### Common Error Codes

| Status Code | Description |
|-------------|-------------|
| 200 | Success |
| 201 | Created |
| 400 | Bad Request (validation errors) |
| 404 | Not Found |
| 500 | Internal Server Error |

### Error Response Examples

**Validation Error:**
```json
{
  "success": false,
  "message": "Validation failed",
  "data": null,
  "statusCode": 400
}
```

**Not Found Error:**
```json
{
  "success": false,
  "message": "Industry with ID 3fa85f64-5717-4562-b3fc-2c963f66afa6 not found",
  "data": null,
  "statusCode": 404
}
```

**Domain Validation Error:**
```json
{
  "success": false,
  "message": "Industry name cannot be empty",
  "data": null,
  "statusCode": 400
}
```

---

## Frontend Implementation Notes

### 1. SQL Query Editor
Build a SQL editor interface similar to SQL Server Management Studio with:
- Syntax highlighting
- Query history
- Result grid display
- Export results to CSV/Excel

### 2. Configuration Workflow
Guide users through creating configurations:
1. Select/Create Industry
2. Select/Create Product
3. Select/Create ProductSubType
4. Create Configuration

### 3. Excel Upload Interface
- File upload with drag-and-drop
- Show preview of columns that will be created
- Display Industry, Product, ProductSubType that will be added
- Show upload progress
- Display table name after successful upload

### 4. Query Builder
Use the metadata endpoint to build a visual query builder:
- Table selection dropdown
- Column selection
- WHERE clause builder
- JOIN support

### 5. Table Explorer
Display all tables and their columns from metadata endpoint:
- Tree view of database schema
- Click to insert table/column names into query editor

---

## Database Schema

### Core Tables
- **Industries**: Top-level categorization
- **Products**: Belong to Industries
- **ProductSubTypes**: Belong to Products
- **Configurations**: Link Industry + Product + ProductSubType

### Dynamic Tables
- Created when Excel files are uploaded
- Named pattern: `Upload_{configurationId}_{timestamp}`
- Always include Industry, Product, ProductSubType columns
- Additional columns based on Excel file structure

---

## Best Practices

1. **Always create a configuration before uploading Excel files**
2. **Save table names from upload responses for future queries**
3. **Use metadata endpoint to discover available tables and columns**
4. **Validate SQL queries on frontend before sending to API**
5. **Handle long-running queries with appropriate timeouts**
6. **Implement query result pagination for large datasets**
7. **Store frequently used queries in frontend local storage**

---

## Support

For questions or issues, please contact the API development team.

**API Version:** 1.0
**Last Updated:** 2025-12-15
