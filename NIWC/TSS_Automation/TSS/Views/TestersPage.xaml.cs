using Microsoft.UI.Xaml.Controls;

using TSS.Lib.ViewModels;

namespace TSS.Views
{
    public sealed partial class TestersPage : Page
    {
        public TestersViewModel ViewModel { get; private set; }

        public TestersPage()
        {
            this.InitializeComponent();

            ViewModel = DependencyContainer.GetService<TestersViewModel>();
        }
    }
}