-- 預算項目明細資料表
CREATE TABLE IF NOT EXISTS BudgetItem (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    DepartmentBudgetId INT NOT NULL,
    ItemName VARCHAR(100) NOT NULL,
    Amount DECIMAL(18, 2) NOT NULL DEFAULT 0.00,
    Description VARCHAR(500) NULL,
    SortOrder INT NOT NULL DEFAULT 0,
    INDEX idx_budget_id (DepartmentBudgetId)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
