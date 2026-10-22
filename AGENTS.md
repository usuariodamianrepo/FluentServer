# AGENTS.md

## Stack and layout

- .NET 10 Blazor Web App with per-page `InteractiveServer` rendering, FluentUI 5, EF Core 10 + PostgreSQL, and Serilog.
- Solution: `FluentServer.slnx`.
- `src/Clone.Web` — UI, DI, Identity, localization, Serilog setup.
- `src/Clone.DataAccess` — `ApplicationDbContext`, generic repository/UnitOfWork, EF migrations.
- `src/Clone.Models` — entities and DTOs.
- `src/Clone.Utility` — roles (`RD`), constants, display/date extensions.
- Project reference chain: `Clone.Web` → `Clone.DataAccess` → `Clone.Models` → `Clone.Utility`.

## Commands

- Build from the repo root: `dotnet build FluentServer.slnx`.
- Run locally: `dotnet run --project src/Clone.Web --launch-profile http` (http://localhost:5007).
- There are no test projects. Verify with a build plus manual UI exercise against PostgreSQL.
- EF Core CLI is not installed globally in this environment. Install it first with `dotnet tool install --global dotnet-ef`, then use:
  - `dotnet ef migrations add <Name> --project src/Clone.DataAccess --startup-project src/Clone.Web`
  - `dotnet ef database update --project src/Clone.DataAccess --startup-project src/Clone.Web`
- Migrations are not applied on startup; `Program.cs` only enables the migrations endpoint in Development.

## Runtime setup

- `ConnectionStrings:DefaultConnection` must be configured or startup throws; PostgreSQL is the only provider.
- The `Localization` section fixes one process-wide culture (default `es-AR`); there is no per-request culture switching.
- Shared localization keys live in `src/Clone.Web/Resources/SharedResource*.resx`; page keys go under `Resources/Components/Pages/<Page>/`. Inject `IStringLocalizer<SharedResource>` or `IStringLocalizer<Page>`.
- Identity requires confirmed accounts. `IdentityNoOpEmailSender` means confirmation links are rendered on `RegisterConfirmation` instead of being emailed.
- Serilog writes to `logs/clone_.log` (monthly rolling, minimum level `Error`).

## Persistence conventions

- Entities must inherit `AuditableEntity` to be usable through `IUnitOfWork.Repository<T>()`.
- Use `UnitOfWork.Repository<T>()` and `UnitOfWork.SaveAsync()`; do not add a custom repository unless you need custom queries.
- `InsertAudit(user)` / `UpdateAudit(user)` are manual. `SaveAsync()` does not populate audit fields, so CRUD pages must call them before add/update.
- PostgreSQL `xmin` is the concurrency token. Each new entity needs the inherited `xmin` property configured explicitly in `ApplicationDbContext.OnModelCreating`, like `Company`, `Document`, and `DocumentType`. Handle `DbUpdateConcurrencyException` on save/delete.
- Audit timestamps are stored in UTC. Use `ConvertToShow()` when displaying them; `Clone.Utility` converts to `America/Argentina/Buenos_Aires`.

## UI conventions

- CRUD pages live in `src/Clone.Web/Components/Pages/<Plural>/<Plural>.razor` + `.razor.cs`; the code-behind namespace is `Clone.Web.Components.Pages.<Plural>`.
- The `add-ui-crud` skill provides the CRUD template, but its flat `src/Clone.Web.Components.Pages` paths are outdated; use the nested layout above.
- Each page uses `@rendermode InteractiveServer`, `[Authorize(Roles = RD.RoleAdmin)]`, a unique `MESSAGEBAR_*` section, `UnitOfWork`, and `INotificationService`.
- Add new pages to `Components/Layout/NavMenu.razor` inside the `<Authorized>` block; navigation is not automatic.
- `DialogService` is injected once in `Components/_Imports.razor`; do not re-inject it per page.
- Role names are Spanish; always use the `RD.*` constants rather than string literals.
- `docs/entity.ps1` is a legacy scaffold: it assumes execution from `docs/`, emits a `Relationship` FK that may not exist, and appends the `DbSet` outside the context class. Add entities and `DbSet`s manually unless you fix the script.

## AI tooling

- Skills live in `.agents/skills/*/SKILL.md`; the current scaffolder is `add-ui-crud`.
