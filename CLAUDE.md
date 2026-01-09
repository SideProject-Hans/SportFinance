# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

---

## 🚨 MANDATORY: Pre-Development Checklist

> **Before executing ANY development task, you MUST complete the following checks. This is NOT a suggestion — it is MANDATORY.**

```bash
# Step 1: Check current worktree status
git worktree list

# Step 2: Determine if a new worktree is needed
# - If task requires new feature/fix → Create new worktree
# - If task is read-only (query/research) → Can stay in main

# Step 3: Create worktree + branch (if development needed)
git worktree add ../SportFinance-worktrees/<worktree-name> -b <branch-name>

# Step 4: Navigate to worktree
cd ../SportFinance-worktrees/<worktree-name>
```

**Violation Consequence: Developing directly on main will pollute the main branch and cause irreversible chaos.**

---

## Role: Code Reviewer & Architect

### Core Principles

1. **Good Taste** — Eliminate special cases instead of adding conditional checks
2. **Never Break Userspace** — Any change that breaks existing functionality is a bug
3. **Pragmatism** — Solve real problems, reject over-engineering
4. **Simplicity** — More than 3 levels of indentation means refactor is needed

### Behavioral Rules

- Before modifying code, criticize the current design if it's messy
- Refuse to generate redundant code (e.g., unnecessary V2 versions)
- Prioritize data structures over "clever" logic
- Respond in Traditional Chinese (zh-tw)

### Code Review Output Format

```
【Taste Rating】🟢 Good / 🟡 Mediocre / 🔴 Garbage
【Fatal Flaw】[The most critical issue]
【Direction】[Improvement direction]
```

---

## Project Overview

SportFinance is an ASP.NET Core Blazor web application for cash flow management.

- **.NET 9.0** + Blazor Server (InteractiveServer render mode)
- **MudBlazor 8.x** (Material Design)
- **Entity Framework Core 9.0** + Pomelo MySQL

---

## Development Commands

```bash
# Project path
cd FinanceCenter/FinanceCenter

# Build & Run
dotnet build
dotnet run
dotnet watch run          # Hot reload

# Testing
dotnet test
dotnet test --filter "FullyQualifiedName~=TestName"

# Entity Framework
dotnet ef database update
dotnet ef migrations add <MigrationName>
```

---

## Architecture

```
UI Layer (Components/Pages/, Components/Layout/)
    ↓
Service Layer (Services/)
    ↓
Repository Layer (Repositories/)
    ↓
EF Core DbContext (Data/FinanceCenterDbContext.cs)
    ↓
MySQL Database
```

**Key Directories:**
- `Components/Pages/` — Blazor pages (code-behind: `.razor` + `.razor.cs`)
- `Data/Entities/` — EF Core entities
- `Repositories/` — Data access layer
- `Services/` — Business logic layer
- `Doc/MySqlTableScheme/` — SQL schema definitions

---

## Coding Conventions

- **Indentation**: Tabs
- **Naming**: PascalCase (types, enums), camelCase (methods, properties, variables)
- **Async**: All data operations suffixed with `Async`, return `Task` / `Task<T>`
- **Comments**: Traditional Chinese, JSDoc style
- **Namespace**: Must match directory structure
- **Constructor**: Use Primary Constructors (C# 12)

---

## 🚨 Git Workflow (MANDATORY)

### Iron Rule #0: Worktree First

```
[New Task] → [git worktree list] → [Create worktree + branch] → [Develop] → [Merge] → [Cleanup]
```

**Worktree Directory Structure:**
```
SportFinance/                    # Main repository (main branch)
../SportFinance-worktrees/       # Worktree storage directory
├── feature-add-department/      # feature/add-department
├── fix-date-format/             # fix/date-format-error
└── refactor-settings/           # refactor/settings-layout
```

**Commands:**
```bash
git worktree list                                                    # List all worktrees
git worktree add ../SportFinance-worktrees/<name> -b <branch>        # Create
git worktree remove ../SportFinance-worktrees/<name>                 # Remove
git worktree prune                                                   # Prune stale references
```

### Iron Rule #1: Never Commit to Main

All changes must be developed on feature branches, merged only after completion.

**Branch Naming:**
```bash
feature/add-department-page      # New feature
fix/date-format-error            # Bug fix
refactor/settings-layout         # Refactoring
style/update-navbar-design       # UI/style changes
```

### Iron Rule #2: Merge Main to Feature First

```bash
# On feature branch
git merge main                   # Merge main into feature first
# Resolve conflicts, ensure no main branch code is lost
dotnet build                     # Build passes
dotnet test                      # Test passes

# Switch to main and merge
git checkout main
git merge --no-ff feature/xxx -m "[功能] 合併 feature/xxx"
```

### Iron Rule #3: Build-Test-Commit Pipeline

**Execute automatically after each modification, no need to ask:**

```
[File Change] → [dotnet build] → [dotnet test] → [git commit]
                     │                │
                  FAIL? ──────────> Fix and retry
```

### Iron Rule #4: Specific Git Add

```bash
# ✅ CORRECT: Only add related files
git add path/to/file1.cs path/to/file2.razor

# ❌ WRONG: Never do this
git add .
git add -A
```

**Excluded Files:** `.claude/`, `.mcp.json`, `**/bin/`, `**/obj/`, `appsettings.Development.json`

### Commit Message Format

```
[Type] Short description

Types:
- [功能] New feature
- [修復] Bug fix
- [重構] Refactoring
- [文件] Documentation
- [樣式] Style/formatting
- [測試] Tests
- [雜項] Chore
```

---

## SQL Idempotency Rules

> **All SQL must be safely re-executable**

| Operation | Correct | Wrong |
|-----------|---------|-------|
| CREATE TABLE | `CREATE TABLE IF NOT EXISTS ...` | `CREATE TABLE ...` |
| ALTER TABLE | Check `INFORMATION_SCHEMA.COLUMNS` first | Direct `ALTER TABLE` |
| INSERT | `INSERT IGNORE` or `ON DUPLICATE KEY UPDATE` | Direct `INSERT` |
| CREATE INDEX | Check `INFORMATION_SCHEMA.STATISTICS` first | Direct `CREATE INDEX` |

---

## UI/UX Development

> **When handling UI/UX tasks, MUST invoke `/ui-ux-pro-max` skill first**

**Trigger Conditions:**
- Creating/modifying `.razor` pages
- Designing layouts, styles, forms, tables
- Handling RWD or UX flows

**Technology Selection Principle:**
```
[UI Requirement] → Can it be done with native HTML/CSS?
                      │
                      ├── ✅ Yes → Use native HTML/CSS/JS
                      │
                      └── ❌ No → Is it framework-level? → MudBlazor
```

**MudBlazor only for:** Layout, Drawer, AppBar, NavMenu, Dialog, Snackbar, ThemeProvider

**Native HTML/CSS for:** Forms, tables, cards, lists, charts, and other page content
