-- 部門年度預算資料表
CREATE TABLE IF NOT EXISTS DepartmentBudget (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Year INT NOT NULL,
    DepartmentCode VARCHAR(20) NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UNIQUE INDEX idx_year_dept (Year, DepartmentCode),
    INDEX idx_year (Year),
    INDEX idx_dept_code (DepartmentCode)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
