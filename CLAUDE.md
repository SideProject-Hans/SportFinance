# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

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

## Branch Protection (System-enforced)

編輯操作在 `main`/`master` 分支會被 hook 自動阻擋。

```bash
git worktree add ../SportFinance-worktrees/<name> -b feature/xxx
```

---

## Development Flow

```
1. 環境    git worktree list → 在 main? → 建立 worktree
2. 需求    列假設 → 標記不確定項 → 與使用者確認 → ⛔ 未確認不得實作
3. 實作    code → build → test → commit
4. 合併    fetch main → merge main → [main] merge --no-ff → push
```

### 需求確認

實作前必須列出：**假設** / **待釐清** / **矛盾點**，經使用者確認後才能開始。

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

> Repository 按業務領域組織，不是按資料表。

---

## Entity Development

```
1. Data/Entities/<Name>.cs
2. Doc/MySqlTableScheme/<Name>.sql   ← 手寫，禁用 ef migrations
3. 註冊到 FinanceCenterDbContext
```

---

## UI/UX

UI 任務使用 `/ui-ux-pro-max`。

| 用途 | 技術 |
|------|------|
| Layout, Drawer, AppBar, Dialog | MudBlazor |
| Forms, tables, cards, charts | Native HTML/CSS |

---

## Coding Rules

| Item | Rule |
|------|------|
| 縮排 | Tabs，≤ 3 層 |
| 命名 | PascalCase (public), camelCase (private/local) |
| Async | `Async` 後綴 |
| 註解 | 繁體中文 |
| Constructor | Primary Constructors (C# 12) |
| 函數 | ≤ 20 行 |
| 常數 | 禁止 magic numbers |
| 例外 | Service 層捕獲 |

---

## Git

```bash
# 分支
feature/add-xxx    fix/xxx-error    refactor/xxx

# Commit (Conventional Commits)
feat: / fix: / refactor: / docs: / style: / test: / chore:

# Merge
--no-ff

# 禁止
git add .
```

---

## Quality Gate

變更後執行：code-simplifier → code-review → Linus-review（須 🟢 Good）→ build → test → commit

> - code-simplifier：Task tool `code-simplifier:code-simplifier`
> - code-review：Task tool `pr-review-toolkit:code-reviewer`
> - Linus-review：讀取 `.claude/LINUS_MODE.md` 進行審查

---

## Forbidden

- `git add .`
- `dotnet ef migrations`
- 在 main 直接編輯
- 未確認需求就實作
