# Coding Standards

> Based on [.NET Runtime Coding Style](https://github.com/dotnet/runtime/blob/main/docs/coding-guidelines/coding-style.md)
> and [Microsoft C# Identifier Names](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names)

---

## Naming Conventions

| Type | Convention | Example |
|------|------------|---------|
| Class / Struct | PascalCase | `DataService`, `CashFlow` |
| Interface | `I` + PascalCase | `IFinanceService` |
| Method | PascalCase | `GetBalanceAsync` |
| Property | PascalCase | `IsValid`, `TotalAmount` |
| Public Field | PascalCase | `MaxRetryCount` |
| Private Field | `_` + camelCase | `_workerQueue` |
| Static Private Field | `s_` + camelCase | `s_instance` |
| Parameter / Local | camelCase | `cashFlow`, `isValid` |
| Constant | PascalCase | `DefaultTimeout` |
| Generic Type | `T` + PascalCase | `TEntity`, `TResult` |
| Async Method | PascalCase + `Async` | `SaveAsync`, `GetAllAsync` |

---

## Formatting Rules

| Item | Rule |
|------|------|
| Indentation | Tabs, ≤ 3 levels |
| Braces | Allman style (new line) |
| Single Statement | May omit braces, but no nesting |
| Blank Lines | Max one, required between methods |
| Visibility | Always explicit (`private` required) |

---

## Types & Syntax

| Item | Correct | Incorrect |
|------|---------|-----------|
| Built-in Types | `int`, `string`, `bool` | `Int32`, `String`, `Boolean` |
| Null Check | `is null`, `is not null` | `== null`, `!= null` |
| String Concat | `$"Hello {name}"` | `"Hello " + name` |
| var Usage | When type is obvious | When type is unclear |

---

## Project-Specific Rules

| Item | Rule |
|------|------|
| Comment Language | Traditional Chinese |
| Constructor | Primary Constructors (C# 12+) |
| Function Length | ≤ 20 lines |
| Magic Numbers | Forbidden, use constants |
| Exception Handling | Catch at Service layer |

---

## Type Design

- Internal/private types should be `static` or `sealed` unless derivation is required
- Fields should be placed at the top of type declarations
- Use `nameof(...)` instead of hardcoded strings

---

## References

- [.NET Runtime Coding Style](https://github.com/dotnet/runtime/blob/main/docs/coding-guidelines/coding-style.md)
- [C# Identifier Names](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names)
- [C# Coding Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- `.github/instructions/csharp.instructions.md` - Detailed C# guidelines in project
