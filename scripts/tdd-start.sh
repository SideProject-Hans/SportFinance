#!/bin/bash
# ============================================
# TDD 啟動腳本
# 進入測試撰寫模式
# ============================================

TEST_MODE_FILE=".tdd-test-mode"
RED_VERIFIED_FILE=".tdd-red-verified"

# 取得腳本所在目錄的父目錄（專案根目錄）
SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
ROOT_DIR="$(dirname "$SCRIPT_DIR")"
cd "$ROOT_DIR"

# 清除之前的狀態
rm -f "$RED_VERIFIED_FILE"

# 創建測試模式標記
touch "$TEST_MODE_FILE"

echo "🧪 TDD 模式已啟動"
echo ""
echo "   當前階段: Test Writing (可編輯測試文件)"
echo "   標記文件: $TEST_MODE_FILE"
echo ""
echo "   下一步:"
echo "   1. 撰寫測試程式碼"
echo "   2. 執行 ./scripts/tdd-red.sh 驗證測試失敗"
