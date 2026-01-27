# Repository Guidelines

## Response Language
- 所有回覆內容一律使用 zh-tw。

## Project Structure & Module Organization
- `FinanceCenter/FinanceCenter/` — Blazor Server app (.NET 9)
  - `Components/Pages/`, `Components/Layout/`, `Components/Dialogs/` — UI pages and layouts
  - `Services/` — business logic (`I*Service` + implementations)
  - `Repositories/` — data access (`I*Repository`, `IUnitOfWork`)
  - `Data/Entities/` + `FinanceCenterDbContext` — EF Core models and context
  - `wwwroot/` — static assets (CSS, images)
- `FinanceCenter/FinanceCenter.Tests/` — xUnit test project
- `FinanceCenter/FinanceCenter/Doc/MySqlTableScheme/` — hand-written MySQL schema files (EF migrations are not used)

## Build, Test, and Development Commands
Run commands from the paths shown:
- `dotnet build` (from `FinanceCenter/FinanceCenter`) — build the app
- `dotnet run` (from `FinanceCenter/FinanceCenter`) — run the Blazor Server host
- `dotnet test` (from `FinanceCenter/`) — run all tests
- `dotnet test --filter "FullyQualifiedName~TestName"` (from `FinanceCenter/`) — run a single test

## Coding Style & Naming Conventions
- Indentation: tabs; keep nesting to 3 levels or fewer.
- Naming: PascalCase for public types/members, camelCase for locals/private fields.
- Async methods use the `Async` suffix.
- Prefer primary constructors (C# 12).
- Keep methods short (≈20 lines), avoid magic numbers, and keep comments in Traditional Chinese.
- Repositories are organized by business domain, not by database table.

## Testing Guidelines
- Frameworks: xUnit + Moq (`FinanceCenter/FinanceCenter.Tests`).
- Test files follow `*Tests.cs` naming; place new tests near the feature’s service/repository.
- Coverage tooling: `coverlet.collector` is referenced; use `dotnet test --collect "XPlat Code Coverage"` if you need coverage output.

## Commit & Pull Request Guidelines
- Commit messages follow Conventional Commits with optional scope (observed in history):
  - Examples: `feat(settings): ...`, `fix(Services): ...`, `docs: ...`, `refactor(...)`, `style: ...`, `chore: ...`, `test: ...`.
- Avoid `git add .`; stage files explicitly.
- PRs should include: clear summary, test commands run (or “not run” with reason), linked issue/task, and UI screenshots for page/layout changes.

## Configuration Tips
- Update DB connection strings in `FinanceCenter/FinanceCenter/appsettings*.json` locally; do not commit secrets.
