using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace FinanceCenter.Components.Layout
{
    /// <summary>
    /// MainLayout 組件的 code-behind 類別
    /// </summary>
    public partial class MainLayout : LayoutComponentBase, IDisposable
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;

        private bool _drawerOpen = false;

        protected override void OnInitialized()
        {
            NavigationManager.LocationChanged += OnLocationChanged;
        }

        private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
        {
            _drawerOpen = false;
            StateHasChanged();
        }

        private void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }

        public void Dispose()
        {
            NavigationManager.LocationChanged -= OnLocationChanged;
        }
    }
}