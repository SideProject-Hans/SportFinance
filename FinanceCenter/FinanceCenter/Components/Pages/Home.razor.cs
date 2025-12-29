using FinanceCenter.Data.Entities;
using FinanceCenter.Services;
using Microsoft.AspNetCore.Components;

namespace FinanceCenter.Components.Pages;

/// <summary>
/// 首頁元件
/// </summary>
public partial class Home
{
    [Inject]
    private FinanceService FinanceService { get; set; } = null!;

    /// <summary>
    /// 現金流資料清單
    /// </summary>
    private List<CashFlow> CashFlows { get; set; } = new();

    /// <summary>
    /// 元件初始化時載入資料
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        await LoadCashFlowsAsync();
    }

    /// <summary>
    /// 載入所有現金流資料
    /// </summary>
    private async Task LoadCashFlowsAsync()
    {
        CashFlows = await FinanceService.GetAllCashFlowsAsync();
    }

    /// <summary>
    /// 重新整理資料
    /// </summary>
    private async Task RefreshDataAsync()
    {
        await LoadCashFlowsAsync();
    }
}