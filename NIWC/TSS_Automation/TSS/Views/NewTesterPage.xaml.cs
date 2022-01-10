using Microsoft.UI.Xaml.Controls;

using TSS.Lib.ViewModels;

namespace TSS.Views
{
    public sealed partial class NewTesterPage : Page
    {
        public NewTesterViewModel ViewModel { get; }
        public NewTesterPage()
        {
            this.InitializeComponent();

            ViewModel = DependencyContainer.GetService<NewTesterViewModel>();
        }
    }
}
