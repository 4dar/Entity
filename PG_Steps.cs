-   Decide where you want your app folder to be and enter the following into your terminal:

    yo candyman <your app name>

-   In your .csproj file inside your <ItemGroup> add the first line to use "dotnet watch run" and the last part for postgress:

    <DotNetCliToolReference Include="Microsoft.DotNet.Watcher.Tools" Version="1.0.0" />
    <ItemGroup>
    <DotNetCliToolReference Include="Microsoft.EntityFrameworkCore.Tools.DotNet" Version="1.0.0" />
    </ItemGroup>    

-   Add these MySql packages that allow us to configure our connection for MySql databases, type the following in your terminal:

    dotnet add package Microsoft.Extensions.Configuration.Json
    dotnet add package Microsoft.EntityFrameworkCore.Tools -v=1.1.1
    dotnet add package Microsoft.EntityFrameworkCore.Tools.DotNet -v=1.0.1
    dotnet add package Microsoft.AspNetCore.Session -v=1.1
    dotnet add package microsoft.aspnetcore.Identity -v=1.1.5
    dotnet add package Microsoft.AspNetCore.Identity --version 1.1.5
    dotnet add package Microsoft.Extensions.Options.ConfigurationExtensions -v=1.1
    dotnet restore

-   Add this dependency manually in your csproj file for postgress:

    <PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="1.0.0" />
    <PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="2.0.0" />

-   Create a folder called Models. Create a [[PROJECTNAME]].cs file for your models and a [[PROJECT NAME]]Context.cs for your Context.

-   Inside your models file put the following:

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    namespace [[YourNamespace]].Models
    {
        public abstract class BaseEntity {}
        public class Register : BaseEntity
        {
            [Required]
            [MinLength(3)]
            public string name { get; set; }
            [Required]
            [EmailAddress]
            public string email { get; set; }
            [Required]
            [DataType(DataType.Password)]
            public string password { get; set; }
            [Required]
            [Compare("password", ErrorMessage = "Password and confirmation must match.")]
            [DataType(DataType.Password)]
            public string conpassword { get; set; }
        }
    }

-   In your context file put the following:

    using Microsoft.EntityFrameworkCore;

    namespace Bank.Models
    {
        public class BankContext : DbContext
        {
            public DbSet<User> Users { get; set; }
            public BankContext(DbContextOptions<BankContext> options) : base(options) { }
        }
    }

-   In your Startup.cs put the following inside your ConfigureServices and also put the following using statements:

    services.AddDbContext<[[YOUR]]Context>(options => options.UseNpgsql(Configuration["DBInfo:ConnectionString"]));

    using System;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.EntityFrameworkCore;
    using [[PROJECTNAME]].Models;
    using Microsoft.Extensions.Configuration;
    using Microsoft.AspNetCore.Hosting;

-   Create a file called "appsettings.json" and paste the following inside:

{
    "DBInfo":
    {
        "Name": "PostGresConnect",
        // PostGres doesn't use the "SslMode" property, make sure to remove it or the connection will be refused
        "ConnectionString": "server=localhost;userId=postgres;password=postgres;port=5432;database=bankDB;"
    },
    "tools": {
    "Microsoft.EntityFrameworkCore.Tools": "1.0.0-preview2-final"
    },
    "dependencies": {
        //Other dependencies
        "Microsoft.Extensions.Configuration.Json": "1.0.0",
        "Npgsql.EntityFrameworkCore.PostgreSQL": "1.0.0-*",
        "Microsoft.EntityFrameworkCore.Design": {
            "version": "1.0.0-preview2-final",
            "type": "build"
        }
    }
}

-   Add the following to the top of and inside your Controller's class:

    using System.Linq;
    using [[YOURSPACENAME]].Models;

    private BankContext _context;
        public HomeController(BankContext context)
        {
            _context = context;
        }

-   Here's something you can put in your cshtml files for now:

    <h1>Yo! Sup?</h1>

-   Restore (in the terminal):

    dotnet restore

-   Migrate! (in the terminal):

    dotnet ef migrations add [[MIGRATION NAME]]

-   Update the database!:

    dotnet ef database update