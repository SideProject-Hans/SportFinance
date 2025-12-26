package main

import (
	"net/http"

	"github.com/gin-gonic/gin"
)

// 簡單的測試 API

func main() {
	r := gin.Default()

	// 健康檢查
	r.GET("/health", func(c *gin.Context) {
		c.JSON(http.StatusOK, gin.H{
			"status":  "healthy",
			"service": "finance-api",
		})
	})

	// API v1 群組
	api := r.Group("/api/v1")
	{
		// 測試端點
		api.GET("/ping", func(c *gin.Context) {
			c.JSON(http.StatusOK, gin.H{
				"message": "pong",
			})
		})

		// 模擬使用者資料
		api.GET("/users", func(c *gin.Context) {
			users := []gin.H{
				{"id": 1, "name": "Alice", "email": "alice@example.com"},
				{"id": 2, "name": "Bob", "email": "bob@example.com"},
			}
			c.JSON(http.StatusOK, gin.H{
				"success": true,
				"data":    users,
			})
		})

		// 模擬帳戶資料
		api.GET("/accounts", func(c *gin.Context) {
			accounts := []gin.H{
				{"id": 1, "name": "現金", "type": "cash", "balance": 10000},
				{"id": 2, "name": "銀行", "type": "bank", "balance": 50000},
			}
			c.JSON(http.StatusOK, gin.H{
				"success": true,
				"data":    accounts,
			})
		})

		// 模擬交易資料
		api.GET("/transactions", func(c *gin.Context) {
			transactions := []gin.H{
				{"id": 1, "type": "expense", "amount": 500, "description": "午餐"},
				{"id": 2, "type": "income", "amount": 30000, "description": "薪水"},
			}
			c.JSON(http.StatusOK, gin.H{
				"success": true,
				"data":    transactions,
			})
		})
	}

	// 啟動伺服器
	r.Run(":8080")
}
