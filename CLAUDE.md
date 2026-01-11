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

### Repository = 業務邊界（非 Table 邊界）

> Repository 按業務領域劃分，非按 Table 劃分。
> 一個 Repository 未來可管理多張表。

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

1. **函數 ≤ 20 行** — 超過就拆
2. **縮排 ≤ 3 層** — 超過就用 early return 或抽函數
3. **No magic numbers** — 數字要有名字
4. **Error 在邊界處理** — Service 層捕捉，不要讓 Exception 穿透到 UI
5. **Null 契約明確** — 回傳可能 null 就標 `?`，不可能就別標

---

## Git Workflow

### Branch Naming
```
feature/add-xxx    fix/xxx-error    refactor/xxx    style/xxx
```

### Development Pipeline (Auto-execute)

```
[Code Change]
      ↓
[code-simplifier:code-simplifier] ← Simplify & refine code
      ↓
[pr-review-toolkit:code-reviewer] ← Review for bugs & quality
      ↓
[dotnet build] ─── FAIL? ──┐
      ↓                    │
[Linus Review] ─── NOT 🟢? ─┼──→ Fix and restart pipeline
      ↓                    │
[dotnet test] ─── FAIL? ───┘
      ↓
[git commit]
```

**🚨 MANDATORY: Review Gates**

| Step | Tool/Reference | Purpose |
|------|----------------|---------|
| 1 | `code-simplifier:code-simplifier` | Simplify code, remove redundancy |
| 2 | `pr-review-toolkit:code-reviewer` | Check bugs, security, quality |
| 3 | `.claude/LINUS_MODE.md` | Linus taste review |

**Linus Review Gate (BLOCKING):**

```
【Taste Rating】🟢 Good / 🟡 Mediocre / 🔴 Garbage
【Fatal Flaw】[Most critical issue]
【Direction】[Improvement path]
```

> **Only 🟢 Good can proceed to test.**
> 🟡 Mediocre or 🔴 Garbage → Fix issues and restart pipeline.

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

### Merge to Main (5 Steps)

```
Step 1: [feature] Merge main into feature
        git fetch origin main
        git merge main
        # Resolve conflicts if any

Step 2: [feature] Verify feature branch
        dotnet build
        dotnet test
        # FAIL? → Fix and retry

Step 3: [main] Merge feature with --no-ff
        git checkout main
        git pull origin main
        git merge --no-ff feature/xxx -m "[功能] 合併 feature/xxx"

Step 4: [main] Verify main branch ← CRITICAL
        dotnet build
        dotnet test
        # FAIL? → git reset --hard HEAD~1, go back to feature and fix

Step 5: [main] Push and cleanup
        git push origin main
        git worktree remove ../SportFinance-worktrees/<name>
        git branch -d feature/xxx
```

> **Why `--no-ff`?** Preserves branch history, enables single-commit revert of entire feature.

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
