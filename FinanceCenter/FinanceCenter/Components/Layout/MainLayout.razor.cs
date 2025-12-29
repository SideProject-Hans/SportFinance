using Microsoft.AspNetCore.Components;

namespace FinanceCenter.Components.Layout
{
    /// <summary>
    /// MainLayout 組件的 code-behind 類別
    /// </summary>
    public partial class MainLayout : LayoutComponentBase
    {
        private bool _drawerOpen = false;

        private void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }
    }
}