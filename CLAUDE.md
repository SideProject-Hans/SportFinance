# CLAUDE.md

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

### 需求確認 (實作前必須完成)

```markdown
## 假設
1. [你的理解]

## 待釐清
- [NEEDS CLARIFICATION: 不確定的點]

## 矛盾點
- [CONTRADICTION: 發現的衝突] 或 ✅ 無
```

> ⛔ 假設未確認、待釐清未解答、矛盾點未解決 → 不得開始實作

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

UI 任務先啟用 `/ui-ux-pro-max`。

| 用途 | 技術 |
|------|------|
| Layout, Drawer, AppBar, Dialog | MudBlazor |
| Forms, tables, cards, charts | Native HTML/CSS |

---

## Coding Conventions

| Item | Rule |
|------|------|
| 縮排 | Tabs |
| 命名 | PascalCase (public), camelCase (private/local) |
| Async | `Async` 後綴 |
| 註解 | 繁體中文 |
| Constructor | Primary Constructors (C# 12) |

---

## Code Quality

- 函數 ≤ 20 行
- 縮排 ≤ 3 層
- 禁止 magic numbers
- Service 層捕獲例外

---

## Git

```bash
# 分支
feature/add-xxx    fix/xxx-error    refactor/xxx

# Commit
[功能] / [修復] / [重構] / [文件] / [樣式] / [測試] / [雜項]

# Merge
--no-ff    # 保留分支歷史，方便整個 feature 一次 revert

# 禁止
git add .
```

---

## Quality Gate

變更後執行：

```
code-simplifier → code-reviewer → dotnet build → dotnet test → git commit
```

Linus Review 必須 🟢 Good 才能繼續。

---

## Forbidden

- `git add .`
- `dotnet ef migrations`
- 在 main 直接編輯
- 未確認需求就實作
