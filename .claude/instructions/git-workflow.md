# Git Workflow

---

## Commit Convention

Use [Conventional Commits](https://www.conventionalcommits.org/):

| Prefix | Usage |
|--------|-------|
| `feat:` | New feature |
| `fix:` | Bug fix |
| `refactor:` | Code refactoring (no behavior change) |
| `docs:` | Documentation only |
| `style:` | Formatting, whitespace (no code change) |
| `test:` | Adding or updating tests |
| `chore:` | Build, tooling, dependencies |

---

## Commit Message Format

```
<type>: <subject>

[optional body]

[optional footer]
```

**Example:**
```
feat: add department deletion with confirmation dialog

- Add GlassConfirmDialog component
- Implement soft delete in DepartmentRepository
- Update Settings page UI
```

---

## Rules

| Rule | Description |
|------|-------------|
| `git add .` | Forbidden - may include sensitive files |
| `git add -A` | Forbidden - same reason |
| `git add <file>` | Stage files individually |

---

## Branch Naming

```
<type>/<short-description>
```

**Examples:**
- `feat/department-management`
- `fix/balance-calculation`
- `refactor/service-layer`

---

## References

- [Conventional Commits](https://www.conventionalcommits.org/)
- `.github/instructions/git.instructions.md` (if exists)
