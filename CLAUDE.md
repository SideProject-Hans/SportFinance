# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Quick Facts

| Item | Value |
|------|-------|
| **Stack** | .NET 9, Blazor Server, MySQL, MudBlazor 8.x |
| **App Dir** | `FinanceCenter/FinanceCenter` |
| **Solution Dir** | `FinanceCenter/` |
| **Build** | `dotnet build` (from App Dir) |
| **Run** | `dotnet run` (from App Dir) |
| **Test** | `dotnet test` (from Solution Dir) |
| **Test Single** | `dotnet test --filter "FullyQualifiedName~TestName"` |
| **Test Framework** | xUnit + Moq |

---

## Development Flow

```
1. Requirements    List assumptions → Mark uncertainties → Confirm with user → ⛔ No implementation without confirmation
2. Implementation  code → build → test → commit
```

### Requirements Confirmation

Before implementation, you must list: **Assumptions** / **Items to clarify** / **Contradictions**, and can only proceed after user confirmation.

---

## Architecture

```
Page.razor.cs → IXxxService → IUnitOfWork → DbContext
```

| Layer | Location |
|-------|----------|
| UI | `Components/Pages/`, `Components/Layout/`, `Components/Dialogs/` |
| Service | `Services/I*Service.cs`, `Services/*Service.cs` |
| Repository | `IUnitOfWork` + `I*Repository` |
| Data | `Data/Entities/`, `FinanceCenterDbContext` |

> Repositories are organized by business domain, not by database table.

---

## Entity Development

```
1. Data/Entities/<Name>.cs
2. Doc/MySqlTableScheme/<Name>.sql   ← Hand-written, EF migrations forbidden
3. Register in FinanceCenterDbContext
```

---

## UI/UX

Use `/ui-ux-pro-max` for UI tasks.

| Purpose | Technology |
|---------|------------|
| Layout, Drawer, AppBar, Dialog | MudBlazor |
| Forms, tables, cards, charts | Native HTML/CSS |

---

## Coding Rules

| Item | Rule |
|------|------|
| Indentation | Tabs, ≤ 3 levels |
| Naming | PascalCase (public), camelCase (private/local) |
| Async | `Async` suffix |
| Comments | Traditional Chinese |
| Constructor | Primary Constructors (C# 12) |
| Functions | ≤ 20 lines |
| Constants | No magic numbers |
| Exceptions | Catch at Service layer |

---

## Git

```bash
# Commit (Conventional Commits)
feat: / fix: / refactor: / docs: / style: / test: / chore:

# Forbidden
git add .
```

---

## Quality Gate

After changes: code-simplifier → code-review → Linus-review (must be 🟢 Good) → build → test → commit

> - code-simplifier: Task tool `code-simplifier:code-simplifier`
> - code-review: Task tool `pr-review-toolkit:code-reviewer`
> - Linus-review: Load `.claude/LINUS_MODE.md` for review

---

## Special Modes

| Trigger | Action |
|---------|--------|
| Command contains `Linus` | Load `.claude/LINUS_MODE.md` for Linus Torvalds style review |

---

## Forbidden

- `git add .`
- `dotnet ef migrations`
- Implementation without confirmed requirements
