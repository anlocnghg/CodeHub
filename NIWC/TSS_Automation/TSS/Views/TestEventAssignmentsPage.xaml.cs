using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

using TSS.Lib.ViewModels;

namespace TSS.Views
{
    public sealed partial class TestEventAssignmentsPage : Page
    {
        public TestEventAssignmentsViewModel ViewModel { get; private set; }

        public TestEventAssignmentsPage()
        {
            this.InitializeComponent();

            ViewModel = DependencyContainer.GetService<TestEventAssignmentsViewModel>();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Now continue with our code...

            /*
             * Could have used the parameters passing in the navigation,
             * but let's use the Messaging Service instead.
             *

            var passedParameters = e.Parameter as Tuple<string, List<Slot>>;
            if (passedParameters != null)
            {
                ViewModel.EventName = passedParameters.Item1;
                ViewModel.Slots = passedParameters.Item2;

                await ViewModel.GenerateAssignmentsAsync();
            }
            else
            {
                throw new ArgumentException($"Expected two parameters: (1) string event name, and (2) list of slots.");
            }
            */
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Our code to prepare leaving...
            base.OnNavigatedFrom(e);
        }
    }
}