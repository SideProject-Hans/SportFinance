#!/bin/bash
# ============================================
# TDD Red Phase 驗證腳本
# 確保測試在實作前必須失敗
# ============================================

TEST_MODE_FILE=".tdd-test-mode"
RED_VERIFIED_FILE=".tdd-red-verified"
TEST_PROJECT="FinanceCenter/FinanceCenter.Tests/FinanceCenter.Tests.csproj"

echo "🔴 TDD Red Phase 驗證中..."
echo ""

# 取得腳本所在目錄的父目錄（專案根目錄）
SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
ROOT_DIR="$(dirname "$SCRIPT_DIR")"
cd "$ROOT_DIR"

# 執行測試並捕獲結果
TEST_OUTPUT=$(dotnet test "$TEST_PROJECT" 2>&1)
TEST_EXIT_CODE=$?

if [ $TEST_EXIT_CODE -ne 0 ]; then
    # 測試失敗 = Red Phase 成功
    echo "$TEST_OUTPUT" | tail -20
    echo ""
    echo "✅ Red Phase 驗證成功！測試如預期失敗。"
    echo ""
    
    # 清除測試模式，創建紅燈驗證標記
    rm -f "$TEST_MODE_FILE"
    touch "$RED_VERIFIED_FILE"
    
    echo "   當前階段: Implementation (可編輯實作文件)"
    echo "   標記文件: $RED_VERIFIED_FILE"
    echo ""
    echo "   下一步:"
    echo "   1. 撰寫最少程式碼讓測試通過"
    echo "   2. 執行 ./scripts/tdd-green.sh 驗證測試通過"
    exit 0
else
    # 測試通過 = Red Phase 失敗
    echo "$TEST_OUTPUT" | tail -10
    echo ""
    echo "⛔ Red Phase 驗證失敗！"
    echo ""
    echo "   測試在實作前就通過了，這表示："
    echo "   1. 測試沒有正確測試新功能，或"
    echo "   2. 功能已經存在"
    echo ""
    echo "   請檢查並修正測試後重新執行此腳本。"
    exit 1
fi
