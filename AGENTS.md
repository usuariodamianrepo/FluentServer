# AGENTS.md

## Project Overview

- This repository contains a Blazor project targeting .NET 10.
- The main web project is `src/Clone.Web/Clone.Web.csproj`.
- The solution file is `FluentBlazorServer.slnx`.

## Development Guidelines

- Prefer Blazor patterns and components over Razor Pages or MVC patterns.
- Keep changes focused on the requested behavior and follow the surrounding code style.
- Reuse existing services, components, and packages before introducing new abstractions or dependencies.
- Do not expose secrets, connection strings, or other environment-specific values in source control.

## Validation

- Build the solution with `dotnet build FluentBlazorServer.slnx`.
- Run available tests with `dotnet test FluentBlazorServer.slnx` when changes affect tested behavior.
- Address compilation errors introduced by changes before completing the work.
