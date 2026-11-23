# 💻 FluentServer .NET 10

Fluent Blazor Web + PostgreSQL + Serilog

## ℹ️ About The Project

FluentServer is a simple project server side with [FluentUI](https://www.fluentui-blazor.net/) Components. It was created with the template [Fluent Blazor Web App](https://www.fluentui-blazor.net/Templates). The idea is use this code for simple project where you don't need very much business logic and the connection is good.

**_Configuration Template_**

    - Framework: .NET Core 10
    - Authentication Type: Individual Accounts
    - Configure for https: true
    - Interactive render mode: Server
    - Interactive location: Per page/component
    - Include sample pages: true

## 🚧 Tech Stack

- Fluent UI Blazor v5.1
- Blazor Web App (Server)
- EntityFrameworkCore
- PostgreSQL
- Serilog

## 🚀 Getting started

1. Clone the project.
2. Configurate data base, update `appsettings.json` file with your connection string `DefaultConnection`.
3. Execute the command `update-databse` to create the data base and the user roles.
4. Go to PostgueSQL and run the scripts into the folder `FluentServer/scripts/`.
5. Run the app and create your user. The default role is `RoleCustomer`, at the first stage you need to chage to `RoleAdmin` from data base. After that you can administrate the users from Users Administration form.

> **Prerequisites:** [.NET 10 SDK](https://dotnet.microsoft.com/download) · [PostgreSQL](https://www.postgresql.org/download/)

## License

Distributed under the MIT License.
