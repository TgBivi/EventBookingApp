# EventBookingApp Project

A scalable website built with Umbraco CMS 15, leveraging .NET 9 and EF 9.0 for robust content management, with SQLite as the database.

## Prerequisites
- [.NET 9](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQLite](https://www.sqlite.org/download.html)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or another IDE
- [Git](https://git-scm.com/)

## Installation
1. **Clone the Repository**
   ```bash
   git clone <repository-url>
   cd <repository-folder>
   ```

2. **Restore NuGet Packages**
   ```bash
   dotnet restore
   ```

3. **Configure SQLite**
   Update the `Program.cs` file with your SQLite connection string and Db context class:
   ```c#
  builder.Services.AddDbContext<BookEventDbContext>(options =>
            options.UseSqlite(@"Data Source=C:\Temp\EventPlan.sqlite.db"));

   ```

4. **Build and Run**
   ```bash
   dotnet build
   dotnet run
   ```
   Open `http://localhost:<port>` and complete the Umbraco setup wizard.

## Project Structure
This project has 3 main sections
1. The main App, `/BookEvent`, has the following section
- `/umbraco/`: Umbraco CMS core (ignored in Git).
- `/App_Data/`: SQLite database and cache files (mostly ignored).
- `/wwwroot/`: Static assets (CSS, JS).
- `/Views/`: MVC views and templates.
- `/Controllers/`: Custom logic handling.
2. The `Core` contains the database connection classes;
- `/Models/`: EF 9.0 and Models Builder models.
- `/Migration/`: This contains the migration logics.
- `/Contex/`: This contains the DbSet for adding tables to the db.
  3.The `/Logic` contains mostly the services and methods for this project.


## Key Features
- **Content Management**: Umbraco 15 backoffice.
- **Custom Templates**: Responsive design.
- **Performance**: Optimized with EF 9.0.

## Development
- **Version Control**: Use Git, respecting `.gitignore`.
- **Config**: Manage settings via `appsettings.json`.
- **Models Builder**: Enabled for strongly-typed models.

## Troubleshooting
- **Database**: Verify SQLite connection string.
- **Build Errors**: Check .NET 9 and EF 9.0 compatibility.
- **Umbraco**: Refer to [Umbraco 15 Docs](https://docs.umbraco.com/umbraco-cms).

## License
[MIT License](LICENSE).
