-- ============================================
-- 純現金流管理資料表 (Pure Cash Flow Management)
-- 建立日期: 2025-12-29
-- ============================================

CREATE TABLE IF NOT EXISTS `CashFlow` (
    `Id` INT NOT NULL AUTO_INCREMENT COMMENT '唯一識別碼',
    `CreateDay` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '建立日期',
    `Department` VARCHAR(100) NOT NULL COMMENT '申請部門',
    `Applicant` VARCHAR(50) NOT NULL COMMENT '申請人',
    `Reason` VARCHAR(500) NOT NULL COMMENT '申請原因',
    `Expense` DECIMAL(18, 2) NOT NULL DEFAULT 0.00 COMMENT '支出金額',
    `Income` DECIMAL(18, 2) NOT NULL DEFAULT 0.00 COMMENT '收入金額',
    `Fee` DECIMAL(18, 2) NOT NULL DEFAULT 0.00 COMMENT '手續費',
    PRIMARY KEY (`Id`),
    INDEX `idx_createday` (`CreateDay`),
    INDEX `idx_department` (`Department`),
    INDEX `idx_applicant` (`Applicant`)
) ENGINE=InnoDB
  DEFAULT CHARSET=utf8mb4
  COLLATE=utf8mb4_unicode_ci
  COMMENT='純現金流管理資料表';