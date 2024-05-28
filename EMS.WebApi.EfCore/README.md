# ASP.NET Core Web API with EF Core

- Create new project with template **ASP.NET Core Empty**.
- Configure Web API service and middleware in Program.cs

		var builder = WebApplication.CreateBuilder(args);
		
		builder.Services.AddControllers(); // Web Api service
		
		var app = builder.Build();
		
		app.MapGet("/", () => "EMS Web Api!");
		
		app.MapControllers(); // Web Api middleware
		
		app.Run();
- Install EF Core packages to interact with database.
	- Common
		- Microsoft.EntityFrameworkCore	
		- Microsoft.EntityFrameworkCore.Design
		- Microsoft.EntityFrameworkCore.Tools
	- Database Specific
		- Microsoft.EntityFrameworkCore.SqlServer
		- Npgsql.EntityFrameworkCore.PostgreSQL
		- Pomelo.EntityFrameworkCore.MySql Or MySql.EntityFrameworkCore
		- Microsoft.EntityFrameworkCore.Cosmos
		- MongoDB.EntityFrameworkCore
- As we are working with SQL Server install SQL Server database specific packages
- You can install packages using Manage Nuget options or run below command in visual studio package manager console for each package.
	Install-Package Microsoft.EntityFrameworkCore
- Add model Employee under Models folder
- Add DbContext EmsDbContext class under Data folder.
- Configure connection string in appsettings.json
- Register Ef Core Db context and connection string in Program.cs

		builder.Services.AddDbContext<EmsDbContext>(options => 
		  options.UseSqlServer(builder.Configuration.GetConnectionString("EmsEfConnectionString")));
- Run below command to apply changes to database

		Add-Migration initial
		Update-Database
- Add a controller EmployeesController under Controllers folder	

	
