# Finance API

個人財務管理 REST API，使用 Go + Gin + MySQL 建構。

## 專案結構

```
finance-api/
├── cmd/
│   └── api/
│       └── main.go              # 程式進入點
├── internal/
│   ├── config/
│   │   ├── config.go            # 設定載入
│   │   └── database.go          # 資料庫連線
│   ├── handler/
│   │   ├── user_handler.go      # 使用者 API 處理
│   │   ├── account_handler.go   # 帳戶 API 處理
│   │   ├── category_handler.go  # 分類 API 處理
│   │   ├── transaction_handler.go # 交易 API 處理
│   │   └── budget_handler.go    # 預算 API 處理
│   ├── middleware/
│   │   ├── jwt.go               # JWT 認證
│   │   ├── cors.go              # CORS 設定
│   │   ├── error_handler.go     # 錯誤處理
│   │   └── logger.go            # 日誌記錄
│   ├── model/
│   │   ├── user.go              # 使用者模型
│   │   ├── account.go           # 帳戶模型
│   │   ├── category.go          # 分類模型
│   │   ├── transaction.go       # 交易模型
│   │   ├── budget.go            # 預算模型
│   │   └── request/             # 請求模型
│   ├── repository/
│   │   ├── user_repository.go   # 使用者資料存取
│   │   ├── account_repository.go # 帳戶資料存取
│   │   ├── category_repository.go # 分類資料存取
│   │   ├── transaction_repository.go # 交易資料存取
│   │   └── budget_repository.go # 預算資料存取
│   └── service/
│       ├── user_service.go      # 使用者業務邏輯
│       ├── account_service.go   # 帳戶業務邏輯
│       ├── category_service.go  # 分類業務邏輯
│       ├── transaction_service.go # 交易業務邏輯
│       └── budget_service.go    # 預算業務邏輯
├── migrations/                   # 資料庫遷移檔案
├── pkg/
│   ├── response/
│   │   └── response.go          # API 回應格式
│   └── validator/
│       └── validator.go         # 自訂驗證器
├── .env.example                  # 環境變數範例
├── .gitignore
├── go.mod
└── README.md
```

## 技術堆疊

- **Go 1.21**
- **Gin v1.9** - Web 框架
- **sqlx v1.3** - SQL 資料庫擴充
- **MySQL** - 資料庫
- **JWT** - 認證機制
- **bcrypt** - 密碼雜湊

## 快速開始

### 1. 安裝依賴

```bash
cd finance-api
go mod download
```

### 2. 設定環境變數

複製 `.env.example` 為 `.env` 並修改設定：

```bash
cp .env.example .env
```

### 3. 建立資料庫

```sql
CREATE DATABASE finance_db CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
```

### 4. 執行資料庫遷移

```bash
# 安裝 migrate CLI
go install -tags 'mysql' github.com/golang-migrate/migrate/v4/cmd/migrate@latest

# 執行遷移
migrate -path migrations -database "mysql://user:password@tcp(localhost:3306)/finance_db" up
```

### 5. 啟動伺服器

```bash
go run cmd/api/main.go
```

伺服器將在 `http://localhost:8080` 啟動。

## API 端點

### 認證

| 方法 | 端點 | 描述 |
|------|------|------|
| POST | `/api/v1/auth/register` | 使用者註冊 |
| POST | `/api/v1/auth/login` | 使用者登入 |

### 使用者

| 方法 | 端點 | 描述 |
|------|------|------|
| GET | `/api/v1/users/profile` | 取得個人資料 |

### 帳戶

| 方法 | 端點 | 描述 |
|------|------|------|
| POST | `/api/v1/accounts` | 建立帳戶 |
| GET | `/api/v1/accounts` | 取得所有帳戶 |
| GET | `/api/v1/accounts/:id` | 取得單一帳戶 |
| PUT | `/api/v1/accounts/:id` | 更新帳戶 |
| DELETE | `/api/v1/accounts/:id` | 刪除帳戶 |

### 分類

| 方法 | 端點 | 描述 |
|------|------|------|
| POST | `/api/v1/categories` | 建立分類 |
| GET | `/api/v1/categories` | 取得所有分類 |
| GET | `/api/v1/categories/:id` | 取得單一分類 |
| PUT | `/api/v1/categories/:id` | 更新分類 |
| DELETE | `/api/v1/categories/:id` | 刪除分類 |

### 交易

| 方法 | 端點 | 描述 |
|------|------|------|
| POST | `/api/v1/transactions` | 建立交易 |
| GET | `/api/v1/transactions` | 取得所有交易 |
| GET | `/api/v1/transactions/:id` | 取得單一交易 |
| PUT | `/api/v1/transactions/:id` | 更新交易 |
| DELETE | `/api/v1/transactions/:id` | 刪除交易 |

### 預算

| 方法 | 端點 | 描述 |
|------|------|------|
| POST | `/api/v1/budgets` | 建立預算 |
| GET | `/api/v1/budgets` | 取得所有預算 |
| GET | `/api/v1/budgets/:id` | 取得單一預算 |
| GET | `/api/v1/budgets/:id/spent` | 取得預算及花費 |
| PUT | `/api/v1/budgets/:id` | 更新預算 |
| DELETE | `/api/v1/budgets/:id` | 刪除預算 |

## API 回應格式

### 成功回應

```json
{
  "success": true,
  "message": "操作成功",
  "data": { ... }
}
```

### 分頁回應

```json
{
  "success": true,
  "message": "操作成功",
  "data": [ ... ],
  "pagination": {
    "total": 100,
    "page": 1,
    "page_size": 20,
    "total_pages": 5,
    "has_next": true,
    "has_previous": false
  }
}
```

### 錯誤回應

```json
{
  "success": false,
  "message": "錯誤訊息",
  "error": { ... }
}
```

## 認證

API 使用 JWT Bearer Token 進行認證。登入後取得 token，在後續請求的 Header 中加入：

```
Authorization: Bearer <your_token>
```

## License

MIT
