# BudgetTracker

## Objective
Using .NET MVC, Entity Framework, and the provided database, create a budgeting web application that allows a user to log transactions, set monthly allocations, and track their budget progress. 

## .NET CLI Commands

```bash
# Creating a new MVC web application in current directory
dotnet new mvc

# Creating a new MVC web application in new directory
dotnet new mvc -o {OUTPUT_DIRECTORY}

# Add a package
dotnet add package {PACKAGE_NAME}

# Builds web application and deploys local server
dotnet run

# Build web application and deploys local server with hot reload
dotnet watch

# Installing Entity Framework and related packages
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools

# Install global tools for Entity Framework
dotnet tool install --global dotnet-ef

# Generating the DB Context for Entity Framework
dotnet ef dbcontext scaffold {DB_CONNECTION_STRING} Microsoft.EntityFrameworkCore.Sqlite --output-dir {OUTPUT_DIRECTORY}

# Template generation
dotnet tool install --global dotnet-aspnet-codegenerator
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design

# Generating CRUD MVC Controller using Entity Framework
dotnet aspnet-codegenerator controller \
  -name {CONTROLLER_NAME} \
  -m {MODEL_NAME} \
  -dc {DB_CONTEXT} \
  --relativeFolderPath Controllers \
  --useDefaultLayout
```