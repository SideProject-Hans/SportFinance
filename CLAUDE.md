# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

---

## Quick Facts

| Item | Value |
|------|-------|
| **Stack** | .NET 8, Blazor Server, MySQL, MudBlazor |
| **Working Dir** | `FinanceCenter/FinanceCenter` |
| **Build** | `dotnet build` |
| **Run** | `dotnet run` |
| **Test** | `dotnet test` |
| **Test Single** | `dotnet test --filter "FullyQualifiedName~TestName"` |

---

## System Hooks (Auto-enforced)

The following rules are enforced at system level via `.claude/settings.local.json`:

| Hook | Trigger | Action |
|------|---------|--------|
| **PreToolUse** | `Edit`, `Write`, `MultiEdit` | ⛔ Block if on `main` or `master` branch |

When blocked, you will see:
```
⛔ Cannot edit files on main branch. Run Phase 0 first:

  git worktree add ../SportFinance-worktrees/<name> -b feature/xxx
  cd ../SportFinance-worktrees/<name>
```

> **Note:** This is a system-level protection. Even if you forget Phase 0, the hook will block the operation.

---

## Skill Activation

Before executing any task, check if the corresponding skill should be activated:

| Trigger | Skill | When |
|---------|-------|------|
| UI/UX related tasks | `/ui-ux-pro-max` | Before implementation |
| After code changes | `code-simplifier:code-simplifier` | Phase 3 |
| After code changes | `pr-review-toolkit:code-reviewer` | Phase 3 |
| Quality review | `.claude/LINUS_MODE.md` | Phase 3 |

---

## Development Pipeline

### Complete Development Flow

```
┌─────────────────────────────────────────────────────────────────────┐
│  Phase 0: Environment Check (BLOCKING - Must run before any change) │
├─────────────────────────────────────────────────────────────────────┤
│  git worktree list                                                  │
│  pwd                                                                │
│                                                                     │
│  Decision:                                                          │
│  ├── In main worktree (SportFinance/) → Must create new worktree    │
│  └── In feature worktree              → ✅ Proceed to Phase 1       │
│                                                                     │
│  ⛔ Skipping Phase 0 = Cannot proceed to Phase 1                    │
└─────────────────────────────────────────────────────────────────────┘
                                    ↓
┌─────────────────────────────────────────────────────────────────────┐
│  Phase 1: Preparation (If Phase 0 requires creating worktree)       │
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

### Complexity
- **Function ≤ 20 lines** — Split if exceeded
- **Indentation ≤ 3 levels** — Use early return or extract function
- **No magic numbers** — Numbers must have names

### Error Handling
- **Service layer catches** — Don't let exceptions penetrate to UI
- **Explicit null contract** — Mark `?` if may return null, don't mark if not possible

### Forbidden
- `dotnet ef migrations` — Write SQL manually instead
- `git add .` — Only add specific files
- Modifying code on main branch — Must use worktree

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
       [Feature] [Fix] [Refactor] [Docs] [Style] [Test] [Chore]
```

> **Why `--no-ff`?** Preserves branch history, enables single-commit revert of entire feature.
