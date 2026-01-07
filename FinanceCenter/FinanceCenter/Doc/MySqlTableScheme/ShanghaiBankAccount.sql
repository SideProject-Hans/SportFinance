-- ============================================
-- 上海銀行帳戶明細資料表 (Shanghai Bank Account)
-- 建立日期: 2026-01-07
-- ============================================

CREATE TABLE IF NOT EXISTS `ShanghaiBankAccount` (
    `Id` INT NOT NULL AUTO_INCREMENT COMMENT '唯一識別碼',
    `CreateDay` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '建立日期',
    `RemittanceDate` DATE NULL COMMENT '匯款日期',
    `Department` VARCHAR(100) NOT NULL COMMENT '申請部門',
    `Applicant` VARCHAR(50) NOT NULL COMMENT '申請人',
    `Reason` VARCHAR(500) NOT NULL COMMENT '申請原因',
    `Expense` DECIMAL(18, 2) NOT NULL DEFAULT 0.00 COMMENT '支出金額',
    `Income` DECIMAL(18, 2) NOT NULL DEFAULT 0.00 COMMENT '收入金額',
    `Fee` DECIMAL(18, 2) NOT NULL DEFAULT 0.00 COMMENT '手續費',
    PRIMARY KEY (`Id`),
    INDEX `idx_scsb_createday` (`CreateDay`),
    INDEX `idx_scsb_department` (`Department`),
    INDEX `idx_scsb_applicant` (`Applicant`)
) ENGINE=InnoDB
  DEFAULT CHARSET=utf8mb4
  COLLATE=utf8mb4_unicode_ci
  COMMENT='上海銀行帳戶明細資料表';
