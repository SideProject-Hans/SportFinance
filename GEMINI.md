# SportFinance (FinanceCenter)

## Project Overview

**SportFinance** is an ASP.NET Core Blazor web application designed for cash flow management.

*   **Framework:** .NET 9.0
*   **Platform:** Blazor Server (InteractiveServer render mode)
*   **UI Library:** MudBlazor 8.x (Material Design) - *Used primarily for Layout, Drawer, AppBar, etc.*
*   **Database:** Entity Framework Core 9.0 + Pomelo MySQL
*   **Architecture:** Layered (UI -> Service -> Repository -> EF Core -> MySQL)

## Building and Running

All commands should typically be run from the project directory: `FinanceCenter/FinanceCenter`.

### Prerequisites
*   .NET 9.0 SDK
*   MySQL Server

### Key Commands

```bash
# Navigate to the main project directory
cd FinanceCenter/FinanceCenter

# Build the application
dotnet build

# Run the application
dotnet run

# Run with Hot Reload
dotnet watch run

# Run Tests
cd ../FinanceCenter.Tests
dotnet test
# Or from the project root:
dotnet test --filter "FullyQualifiedName~=TestName"
```

### Database Operations
```bash
# Update Database
dotnet ef database update

# Add Migration
dotnet ef migrations add <MigrationName>
```

## Development Conventions

### Code Style & Structure
*   **Indentation:** Tabs
*   **Naming:** 
    *   `PascalCase` for types, enums
    *   `camelCase` for methods, properties, variables
*   **Async:** All async data operations must be suffixed with `Async` and return `Task` or `Task<T>`.
*   **Constructors:** Use C# 12 Primary Constructors.
*   **Comments:** Traditional Chinese (zh-tw), JSDoc style.
*   **Namespaces:** Must match the directory structure.

### Git Workflow (Mandatory)
*   **Worktrees:** Use `git worktree` for parallel development. Avoid developing directly on `main`.
*   **Branching:**
    *   `feature/...` for new features
    *   `fix/...` for bug fixes
    *   `refactor/...` for refactoring
*   **Commit Pipeline:** `dotnet build` -> `dotnet test` -> `git commit`. Ensure build and tests pass before committing.
*   **Commit Messages:** `[Type] Short description` (e.g., `[功能] New feature`, `[修復] Bug fix`).

### UI/UX Guidelines
*   **Native First:** Prefer native HTML/CSS for content (forms, tables, cards, charts).
*   **MudBlazor:** Use only for structural components (Layout, Drawer, AppBar, NavMenu, Dialog, Snackbar).

### Directory Structure
*   `FinanceCenter/FinanceCenter.slnx`: Main Solution file.
*   `FinanceCenter/FinanceCenter/`: Main Web Project.
    *   `Components/Pages/`: Blazor pages (`.razor` + `.razor.cs`).
    *   `Services/`: Business logic.
    *   `Repositories/`: Data access.
    *   `Data/`: EF Core DbContext and Entities.
*   `FinanceCenter/FinanceCenter.Tests/`: Unit and Integration Tests.
