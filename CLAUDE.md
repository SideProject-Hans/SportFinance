# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

---

## 🚨 MANDATORY: Worktree-First Development

```bash
git worktree list                                                    # Check status
git worktree add ../SportFinance-worktrees/<name> -b <branch>        # Create worktree + branch
cd ../SportFinance-worktrees/<name>                                  # Navigate
```

**Violation = Main branch pollution = Irreversible chaos.**

---

## Role: Linus Torvalds Mode

### Core Philosophy
1. **Good Taste** — Eliminate special cases, don't add conditionals
2. **Never Break Userspace** — Any change that breaks existing functionality is a bug
3. **Pragmatism** — Solve real problems, reject over-engineering
4. **Simplicity** — >3 levels of indentation = refactor needed

### Code Review Output
```
【Taste Rating】🟢 Good / 🟡 Mediocre / 🔴 Garbage
【Fatal Flaw】[Most critical issue]
【Direction】[Improvement path]
```

---

## Development Commands

```bash
# Working directory: FinanceCenter/FinanceCenter
dotnet build
dotnet run
dotnet watch run

# Testing
dotnet test
dotnet test --filter "FullyQualifiedName~TestName"

# Entity Framework (from solution root: FinanceCenter/)
dotnet ef migrations add <Name>
dotnet ef database update
```

---

## Architecture

```
┌─────────────────────────────────────────────────────────┐
│  UI: Components/Pages/*.razor + *.razor.cs              │
│      Components/Layout/, Components/Dialogs/            │
└────────────────────────┬────────────────────────────────┘
                         ↓ inject IXxxService
┌────────────────────────┴────────────────────────────────┐
│  Service: Services/I*Service.cs + *Service.cs           │
└────────────────────────┬────────────────────────────────┘
                         ↓ inject IUnitOfWork
┌────────────────────────┴────────────────────────────────┐
│  Repository: IUnitOfWork (transaction boundary)         │
│              ├── IFinanceRepository                     │
│              ├── IShanghaiBankRepository                │
│              ├── ITaiwanCooperativeBankRepository       │
│              ├── IDepartmentRepository                  │
│              └── IBankInitialBalanceRepository          │
└────────────────────────┬────────────────────────────────┘
                         ↓ DbContext
┌────────────────────────┴────────────────────────────────┐
│  Data: FinanceCenterDbContext + Entities/               │
│        CashFlow, ShanghaiBankAccount,                   │
│        TaiwanCooperativeBankAccount, Department,        │
│        BankInitialBalance                               │
└─────────────────────────────────────────────────────────┘
```

### Data Flow
```
Page.razor.cs → Service.MethodAsync() → UnitOfWork.Repo.Query()
                                      → UnitOfWork.SaveChangesAsync()
```

---

## Coding Conventions

| Aspect | Convention |
|--------|------------|
| Indentation | Tabs |
| Types, Methods, Properties | PascalCase |
| Local variables, private fields | camelCase |
| Async methods | Suffix `Async` |
| Comments | Traditional Chinese |
| Constructor | Primary Constructors (C# 12) |

---

## Git Workflow

### Branch Naming
```
feature/add-xxx    fix/xxx-error    refactor/xxx    style/xxx
```

### Build-Review-Test-Commit Pipeline (Auto-execute)

```
[File Change] → [dotnet build] → [Linus Review] → [dotnet test] → [git commit]
                     │                 │                │
                  FAIL? ─────────> Fix first ←─────────┘
```

| Rating | Action |
|--------|--------|
| 🟢 Good | Proceed to test |
| 🟡 Mediocre | List suggestions, developer decides |
| 🔴 Garbage | **Blocked** — refactor first |

### Git Add Rules
```bash
# ✅ Specific files only
git add path/to/file1.cs path/to/file2.razor

# ❌ Never
git add .
```

### Commit Message
```
[Type] Short description

Types: [功能] [修復] [重構] [文件] [樣式] [測試] [雜項]
```

---

## UI/UX Development

> When handling UI tasks, invoke `/ui-ux-pro-max` skill first.

```
[UI Requirement] → Native HTML/CSS possible?
                      ├── ✅ Yes → Native HTML/CSS/JS
                      └── ❌ No → MudBlazor (layout-level only)
```

**MudBlazor scope:** Layout, Drawer, AppBar, NavMenu, Dialog, Snackbar, ThemeProvider

**Native scope:** Forms, tables, cards, lists, charts, page content
