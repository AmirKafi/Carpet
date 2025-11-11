# Carpet Invoice System

A simple ASP.NET MVC application for managing carpet invoices with SQLite database.

## Features

- Create, view, and delete invoices
- Carpet search functionality
- Automatic tax calculation (9%)
- Persian/Farsi UI support (RTL)

## Database Setup

1. SQLite database is used - no additional database server required!
2. The database file (`CarpetDb.db`) will be automatically created in the project directory on first run
3. The connection string in `appsettings.json` is already configured:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Data Source=CarpetDb.db"
   }
   ```
4. The database will be automatically created on first run (development mode)

## Project Structure

- **Models/Entities**: Entity models (Invoice, InvoiceItem, Carpet)
- **Models/DTOs**: Data Transfer Objects
- **Data**: DbContext configuration
- **Repositories**: Repository pattern implementations
- **Mappers**: Entity to DTO mappers
- **Controllers**: MVC and API controllers
- **Views**: Razor views for invoice management

## Running the Application

1. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

2. Run the application:
   ```bash
   dotnet run
   ```

3. Navigate to `https://localhost:5001` (or the port shown in the console)

4. The SQLite database file (`CarpetDb.db`) will be created automatically in the project directory on first run

## API Endpoints

- `GET /api/Carpet/search?term={searchTerm}` - Search carpets by name
- `POST /api/InvoiceApi` - Create a new invoice
- `GET /api/InvoiceApi` - Get all invoices
- `GET /api/InvoiceApi/{id}` - Get invoice by ID
- `DELETE /api/InvoiceApi/{id}` - Delete invoice

## MVC Routes

- `/Invoice` - List all invoices
- `/Invoice/Create` - Create new invoice
- `/Invoice/Details/{id}` - View invoice details
- `/Invoice/Delete/{id}` - Delete invoice

## Notes

- The application uses Entity Framework Core with SQLite
- Database is automatically created using `EnsureCreated()` in development mode
- The SQLite database file (`CarpetDb.db`) is stored in the project directory
- For production, consider using migrations instead of `EnsureCreated()`
- Tax is automatically calculated at 9% of the total amount
- SQLite is perfect for development and small-to-medium applications - no database server required!

