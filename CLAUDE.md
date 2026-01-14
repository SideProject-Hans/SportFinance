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
| **Branch Protection** | `Edit`, `Write`, `MultiEdit` | ⛔ Block if on `main` or `master` branch |

### Branch Protection

When blocked, you will see:
```
⛔ Cannot edit files on main branch. Run Phase 0 first:

  git worktree add ../SportFinance-worktrees/<name> -b feature/xxx
  cd ../SportFinance-worktrees/<name>
```

> **Note:** This is a system-level protection. Even if you forget the flow, the hook will block the operation.

---

## Skill Activation

Before executing any task, check if the corresponding skill should be activated:

| Trigger | Skill | When |
|---------|-------|------|
| UI/UX related tasks | `/ui-ux-pro-max` | Before implementation |
| After code changes | `code-simplifier:code-simplifier` | Phase 4 |
| After code changes | `pr-review-toolkit:code-reviewer` | Phase 4 |
| Quality review | `.claude/LINUS_MODE.md` | Phase 4 |

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
│  Phase 2: Requirement Clarification (BLOCKING)                      │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│  2a. 需求理解                                                        │
│      ├── 閱讀並理解使用者需求                                         │
│      ├── 列出所有假設 (Assumptions)                                  │
│      └── 標記不確定項目 [NEEDS CLARIFICATION: ...]                   │
│                         ↓                                           │
│  2b. 需求確認 (必須與使用者互動)                                       │
│      ├── 向使用者確認所有假設                                         │
│      ├── 詢問所有 [NEEDS CLARIFICATION] 項目                         │
│      └── 記錄使用者的回覆                                             │
│                         ↓                                           │
│  2c. 需求確認閘門 ⛔                                                 │
│      ├── 所有 [NEEDS CLARIFICATION] 項目都已解決?                    │
│      ├── 所有假設都已與使用者確認?                                    │
│      └── ❌ 任一未完成 → 不得進入 Phase 3                            │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
                                    ↓
┌─────────────────────────────────────────────────────────────────────┐
│  Phase 3: Specification & Implementation                            │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│  3a. Specification (複雜功能必須，簡單修復可跳過)                      │
│      ├── 建立 specs/<feature-name>.md                               │
│      ├── 定義驗收標準 (Acceptance Criteria)                          │
│      └── 通過簡化閘門檢查                                            │
│                         ↓                                           │
│  3b. Implementation                                                 │
│      ├── 實作功能程式碼                                              │
│      └── If adding Entity → Execute Entity Dev Flow                 │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
                                    ↓
┌─────────────────────────────────────────────────────────────────────┐
│  Phase 4: Quality Review Pipeline (Auto-execute after each change)  │
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
│  Phase 5: Merge to Main (5 Steps)                                   │
├─────────────────────────────────────────────────────────────────────┤
│  Step 1: [feature] git fetch origin main && git merge main          │
│  Step 2: [feature] dotnet build && dotnet test                      │
│  Step 3: [main] git merge --no-ff feature/xxx                       │
│  Step 4: [main] dotnet build && dotnet test ← CRITICAL              │
│  Step 5: [main] git push && cleanup worktree                        │
└─────────────────────────────────────────────────────────────────────┘
```

---

## Requirement Clarification (Phase 2 詳細指引)

### 需求理解流程

每次接收到新任務時，必須執行以下步驟：

**Step 1: 列出假設**

將你對需求的所有理解列為假設清單：
```markdown
## 我的假設
1. [假設 1：例如「這個功能只需要支援單一使用者」]
2. [假設 2：例如「資料不需要即時同步」]
3. [假設 3：例如「使用現有的資料庫結構」]
```

**Step 2: 標記不確定項目**

對於任何不清楚的部分，使用 `[NEEDS CLARIFICATION]` 標記：
```markdown
## 待釐清項目
- [NEEDS CLARIFICATION: 金額是否需要支援小數點？]
- [NEEDS CLARIFICATION: 刪除操作是否需要軟刪除？]
```

**Step 3: 與使用者確認**

主動向使用者詢問：
1. 所有假設是否正確
2. 所有待釐清項目的答案
3. 是否有遺漏的需求

### 需求確認閘門

在進入 Phase 3 之前，必須滿足：

| 檢查項目 | 狀態 |
|----------|------|
| 所有假設都已確認 | ☐ |
| 所有 [NEEDS CLARIFICATION] 都已解答 | ☐ |
| 使用者已確認需求完整 | ☐ |

> ❌ 任一項未完成，不得開始實作

---

## Specification-Driven Development (SDD)

### 何時需要規格

| 情況 | 需要 spec.md? |
|------|---------------|
| 新功能開發 | ✅ 必須 |
| 重大重構 | ✅ 必須 |
| Bug 修復 | ❌ 不需要 |
| 小型調整 | ❌ 不需要 |

### Spec 檔案結構

**位置**: `specs/<feature-name>.md`

```markdown
# <功能名稱>

## 問題描述
[這個功能要解決什麼問題？]

## 已確認需求
[Phase 2 中與使用者確認過的需求列表]

## 驗收標準
- [ ] 標準 1：[具體、可測試]
- [ ] 標準 2：[具體、可測試]
- [ ] 標準 3：[具體、可測試]

## 技術決策
- 選用方案：[方案名稱]
- 原因：[為什麼選這個]

## 任務清單
- [ ] Task 1: [具體任務]
- [ ] Task 2: [具體任務]
```

### 簡化閘門檢查

開始實作前，必須通過以下檢查：

| 問題 | 預期答案 |
|------|----------|
| 是否用現有框架功能？ | ✅ 是 |
| 是否避免過早優化？ | ✅ 是 |
| 是否有不必要的抽象層？ | ❌ 沒有 |
| 是否為假想需求設計？ | ❌ 沒有 |

> 任何一項不通過 → 重新設計，直到通過。

---

## Entity Development Flow

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

---

## Linus Review Gate (BLOCKING)

```
【Taste Rating】🟢 Good / 🟡 Mediocre / 🔴 Garbage
【Fatal Flaw】[Most critical issue]
【Direction】[Improvement path]
```

> **Only 🟢 Good can proceed to test.**
> 🟡 Mediocre or 🔴 Garbage → Fix issues and restart pipeline.

---

## UI/UX Development Flow

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
- 猜測需求 — 必須使用 Phase 2 與使用者確認

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
