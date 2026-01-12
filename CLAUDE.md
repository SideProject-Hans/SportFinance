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

## Development Commands

```bash
# Working directory: FinanceCenter/FinanceCenter
dotnet build
dotnet run
dotnet watch run

# Testing
dotnet test
dotnet test --filter "FullyQualifiedName~TestName"
```

---

## Development Pipeline

### Complete Development Flow

```
┌─────────────────────────────────────────────────────────────────────┐
│  Phase 1: Preparation                                               │
├─────────────────────────────────────────────────────────────────────┤
│  git worktree add ../SportFinance-worktrees/<name> -b feature/xxx   │
│  cd ../SportFinance-worktrees/<name>                                │
└─────────────────────────────────────────────────────────────────────┘
                                    ↓
┌─────────────────────────────────────────────────────────────────────┐
│  Phase 2: Development & Changes                                     │
├─────────────────────────────────────────────────────────────────────┤
│  [Code Change] ──→ If adding Entity, execute Entity Dev Flow        │
└─────────────────────────────────────────────────────────────────────┘
                                    ↓
┌─────────────────────────────────────────────────────────────────────┐
│  Phase 3: Quality Review Pipeline (Auto-execute after each change)  │
├─────────────────────────────────────────────────────────────────────┤
│  [code-simplifier:code-simplifier] ← Simplify code                  │
│           ↓                                                         │
│  [pr-review-toolkit:code-reviewer] ← Review bugs & quality          │
│           ↓                                                         │
│  [dotnet build] ─── FAIL? ──┐                                       │
│           ↓                 │                                       │
│  [Linus Review] ─ NOT 🟢? ──┼──→ Fix and restart pipeline           │
│           ↓                 │                                       │
│  [dotnet test] ─── FAIL? ───┘                                       │
│           ↓                                                         │
│  [git commit]                                                       │
└─────────────────────────────────────────────────────────────────────┘
                                    ↓
┌─────────────────────────────────────────────────────────────────────┐
│  Phase 4: Merge to Main (5 Steps)                                   │
├─────────────────────────────────────────────────────────────────────┤
│  Step 1: [feature] git fetch origin main && git merge main          │
│  Step 2: [feature] dotnet build && dotnet test                      │
│  Step 3: [main] git merge --no-ff feature/xxx                       │
│  Step 4: [main] dotnet build && dotnet test ← CRITICAL              │
│  Step 5: [main] git push && cleanup worktree                        │
└─────────────────────────────────────────────────────────────────────┘
```

### Entity Development Flow

```
[Add New Entity]
      ↓
1. Create Data/Entities/<Name>.cs
      ↓
2. Create Doc/MySqlTableScheme/<Name>.sql  ← Manual table creation SQL
      ↓
3. Register in FinanceCenterDbContext
      ↓
❌ DO NOT use dotnet ef migrations
```

**SQL File Location**: `FinanceCenter/FinanceCenter/Doc/MySqlTableScheme/`

**SQL File Format**:
```sql
-- ============================================
-- <Table Description>
-- Created: YYYY-MM-DD
-- ============================================

CREATE TABLE IF NOT EXISTS `<TableName>` (
    `Id` INT NOT NULL AUTO_INCREMENT COMMENT 'Primary key',
    -- columns...
    PRIMARY KEY (`Id`),
    INDEX `idx_<column>` (`<column>`)
) ENGINE=InnoDB
  DEFAULT CHARSET=utf8mb4
  COLLATE=utf8mb4_unicode_ci
  COMMENT='<Table Description>';
```

### Quality Review Gates

| Step | Tool/Reference | Purpose |
|------|----------------|---------|
| 1 | `code-simplifier:code-simplifier` | Simplify code, remove redundancy |
| 2 | `pr-review-toolkit:code-reviewer` | Check bugs, security, quality |
| 3 | `.claude/LINUS_MODE.md` | Linus taste review |

### Linus Review Gate (BLOCKING)

```
【Taste Rating】🟢 Good / 🟡 Mediocre / 🔴 Garbage
【Fatal Flaw】[Most critical issue]
【Direction】[Improvement path]
```

> **Only 🟢 Good can proceed to test.**
> 🟡 Mediocre or 🔴 Garbage → Fix issues and restart pipeline.

### UI/UX Development Flow

> When handling UI tasks, invoke `/ui-ux-pro-max` skill first.

```
[UI Requirement] → Native HTML/CSS possible?
                      ├── ✅ Yes → Native HTML/CSS/JS
                      └── ❌ No → MudBlazor (layout-level only)
```

**MudBlazor scope:** Layout, Drawer, AppBar, NavMenu, Dialog, Snackbar, ThemeProvider

**Native scope:** Forms, tables, cards, lists, charts, page content

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

### Repository = Business Boundary (Not Table Boundary)

> Repository is organized by business domain, not by table.
> One repository may manage multiple tables in the future.

---

## Role: Linus Torvalds Mode

### Core Philosophy
1. **Good Taste** — Eliminate special cases, don't add conditionals
2. **Never Break Userspace** — Any change that breaks existing functionality is a bug
3. **Pragmatism** — Solve real problems, reject over-engineering
4. **Simplicity** — >3 levels of indentation = refactor needed

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

## Code Quality Rules

1. **Function ≤ 20 lines** — Split if exceeded
2. **Indentation ≤ 3 levels** — Use early return or extract function
3. **No magic numbers** — Numbers must have names
4. **Error handling at boundaries** — Service layer catches, don't let exceptions penetrate to UI
5. **Explicit null contract** — Mark `?` if may return null, don't mark if not possible

---

## Git Rules

### Branch Naming
```
feature/add-xxx    fix/xxx-error    refactor/xxx    style/xxx
```

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

> **Why `--no-ff`?** Preserves branch history, enables single-commit revert of entire feature.
