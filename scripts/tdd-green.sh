#!/bin/bash
# ============================================
# TDD Green Phase 完成腳本
# 確認測試通過並清除狀態
# ============================================

RED_VERIFIED_FILE=".tdd-red-verified"
TEST_PROJECT="FinanceCenter/FinanceCenter.Tests/FinanceCenter.Tests.csproj"

echo "🟢 TDD Green Phase 驗證中..."
echo ""

# 取得腳本所在目錄的父目錄（專案根目錄）
SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
ROOT_DIR="$(dirname "$SCRIPT_DIR")"
cd "$ROOT_DIR"

# 執行測試並捕獲結果
TEST_OUTPUT=$(dotnet test "$TEST_PROJECT" 2>&1)
TEST_EXIT_CODE=$?

if [ $TEST_EXIT_CODE -eq 0 ]; then
    # 測試通過 = Green Phase 成功
    echo "$TEST_OUTPUT" | tail -10
    echo ""
    echo "✅ Green Phase 完成！所有測試通過。"
    echo ""
    echo "   已清除標記文件: $RED_VERIFIED_FILE"
    echo "   可以進入 Phase 3 品質審查。"
    rm -f "$RED_VERIFIED_FILE"
    exit 0
else
    # 測試失敗 = Green Phase 未完成
    echo "$TEST_OUTPUT" | tail -20
    echo ""
    echo "⚠️  Green Phase 未完成"
    echo ""
    echo "   測試仍有失敗，請繼續實作直到所有測試通過。"
    exit 1
fi
