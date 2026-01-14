# Development State

## Task Info
- **Task**: [任務描述]
- **Branch**: [branch-name]
- **Created**: [YYYY-MM-DD HH:MM]
- **Last Updated**: [YYYY-MM-DD HH:MM]

---

## Phase Progress

### Phase 0: 環境檢查
- [ ] 確認不在 main branch
- [ ] 建立/進入 worktree
- **Status**: PENDING
- **Completed**: -

### Phase 1: 準備工作
- [ ] worktree 已建立
- **Status**: PENDING
- **Completed**: -

### Phase 2: 需求確認
- [ ] 列出假設
- [ ] 標記待釐清項目
- [ ] 矛盾點審查 (think harder)
- [ ] 使用者確認所有假設
- [ ] 使用者確認所有待釐清項目
- [ ] 使用者確認所有矛盾點
- **Status**: PENDING
- **Completed**: -

#### 假設清單
<!-- 格式: 1. ✅/❓ 假設內容（已確認/待確認） -->

#### 待釐清項目
<!-- 格式: - [NEEDS CLARIFICATION] 問題 → 答案：xxx / 待回答 -->

#### 矛盾點
<!-- 格式: - [CONTRADICTION] 矛盾描述 → 解決方案：xxx / 待解決 -->

### Phase 3: 規格與實作
- [ ] 建立 spec 檔案（如需要）
- [ ] 通過簡化閘門
- [ ] 實作功能
- **Status**: PENDING
- **Completed**: -

### Phase 4: 品質檢查
- [ ] code-simplifier
- [ ] code-reviewer
- [ ] dotnet build
- [ ] Linus Review (需達到 🟢)
- [ ] dotnet test
- [ ] git commit
- **Status**: PENDING
- **Completed**: -

### Phase 5: 合併
- [ ] fetch & merge main
- [ ] merge to main (--no-ff)
- [ ] main build & test
- [ ] push & cleanup
- **Status**: PENDING
- **Completed**: -

---

## Blocking Gates

| Gate | Status | Requirement |
|------|--------|-------------|
| Phase 2 → 3 | 🔴 | 所有假設、待釐清、矛盾點都需解決 |
| Phase 4 Linus | 🔴 | 必須達到 🟢 Good |
| Phase 4 → 5 | 🔴 | build & test 必須通過 |

---

## Session Log
<!-- 記錄每次對話的進度更新 -->

### [YYYY-MM-DD HH:MM] Session Start
- 載入狀態，繼續 Phase X
- 完成項目：...
