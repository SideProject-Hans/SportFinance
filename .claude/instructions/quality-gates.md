# Quality Gates

> Required quality checks before code can be committed

---

## Flow

```
code-simplifier → code-review → Linus-review (🟢) → build → test → commit
```

Each step must pass before proceeding to the next.

---

## Steps

### 1. Code Simplifier
**Purpose:** Simplify code, remove unnecessary complexity

**Tool:** Task tool `code-simplifier:code-simplifier`

**Checks:**
- Function length ≤ 20 lines
- Indentation ≤ 3 levels
- Duplicate logic that can be merged
- Dead code that can be removed

---

### 2. Code Review
**Purpose:** Check code quality and best practices

**Tool:** Task tool `pr-review-toolkit:code-reviewer`

**Checks:**
- Naming conventions compliance
- Potential bugs
- Architecture layer compliance
- Security issues

---

### 3. Linus Review
**Purpose:** Review with Linus Torvalds' strict standards

**Tool:** Load `.claude/LINUS_MODE.md`

**Rating Scale:**
| Rating | Meaning | Can Commit? |
|--------|---------|-------------|
| 🟢 Good | Code has taste | Yes |
| 🟡 Mediocre | Acceptable but improvable | Should improve |
| 🔴 Garbage | Needs rewrite | No |

**Linus 5-Layer Analysis:**
1. **Data Structure** — What's the core data? How does it flow?
2. **Edge Cases** — Can they be eliminated by redesign?
3. **Complexity** — Can it be simplified by half? Again?
4. **Destructive** — Will it break existing features?
5. **Practicality** — Does this problem actually exist?

---

### 4. Build
**Command:** `dotnet build` (from App Dir)

**Must pass:** Zero errors, zero warnings

---

### 5. Test
**Command:** `dotnet test` (from Solution Dir)

**Must pass:** All tests green

---

## Quick Checklist

```
□ code-simplifier executed
□ code-review passed
□ Linus-review rated 🟢
□ dotnet build zero errors
□ dotnet test all passed
□ Ready to commit
```

---

## References

- `.claude/LINUS_MODE.md` — Full Linus review guidelines
