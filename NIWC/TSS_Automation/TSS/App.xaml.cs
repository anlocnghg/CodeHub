using Microsoft.UI.Xaml;

using TSS.Views;

namespace TSS
{
    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
        }
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            DependencyContainer.RegisterAndConfigureDependencies();

            m_window = new ShellWindow();
            m_window.Title = "TSS Scheduler";
            m_window.Activate();
        }

        private Window m_window;
    }
}
