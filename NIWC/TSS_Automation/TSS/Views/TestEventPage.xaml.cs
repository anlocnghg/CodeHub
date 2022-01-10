using Microsoft.UI.Xaml.Controls;

using TSS.Lib.ViewModels;

namespace TSS.Views
{
    public sealed partial class TestEventPage : Page
    {
        public TestEventViewModel ViewModel { get; }
        public TestEventPage()
        {
            this.InitializeComponent();

            ViewModel = DependencyContainer.GetService<TestEventViewModel>();
        }
    }
}
