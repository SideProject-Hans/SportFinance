-- 銀行初始金額資料表
CREATE TABLE BankInitialBalance (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    BankType VARCHAR(50) NOT NULL,
    InitialBalance DECIMAL(18, 2) NOT NULL DEFAULT 0.00,
    EffectiveYear INT NOT NULL,
    UNIQUE INDEX idx_bank_type (BankType)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
