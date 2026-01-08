using Aegis.Services;
using System.Windows;

namespace Aegis
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ThemeService.ApplyTheme(SettingsService.Current.Theme);
        }
    }
}
