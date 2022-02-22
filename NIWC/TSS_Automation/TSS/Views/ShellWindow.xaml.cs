using LocLib.WinUI;
using LocLib.WinUI.AppData;
using LocLib.WinUI.Services;
using LocLib.WinUI.ViewModels;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using TSS.Lib.ViewModels;

namespace TSS.Views
{
    public sealed partial class ShellWindow : Window
    {
        public ShellViewModel ViewModel { get; private set; }

        public ShellWindow()
        {
            this.InitializeComponent();

            ViewModel = DependencyContainer.GetService<ShellViewModel>();
            InitNavViewMenuItems();
            InitNavViewFooterMenuItems();
            ViewModel.NavigationService.Init(mainFrame);
            mainFrame.Navigated += MainFrame_Navigated;

            rootGrid.Loaded += RootGrid_Loaded;
            navView.Loaded += NavView_Loaded;
        }

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            if (AppSettings.LocalSettingsContainer.Values.ContainsKey(AppKeyTokens.PaneOpenOrClose))
                navView.IsPaneOpen = (bool)AppSettings.LocalSettingsContainer.Values[AppKeyTokens.PaneOpenOrClose];
        }

        private void RootGrid_Loaded(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.NavigationService.Navigate<TestEventViewModel>())
            {
                throw new InvalidOperationException($"Unsuccessfully navigated to {typeof(TestEventViewModel)}.");
            }
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            var targettype = NavigationService.GetViewModel(e.SourcePageType);
            if (targettype == null) return;

            if (ViewModel.NavViewMenuItems.Any(i => i.Tag as Type == targettype))
            {
                ViewModel.SelectedMenuItem = ViewModel.NavViewMenuItems.Where(i => i.Tag as Type == targettype).FirstOrDefault();
            }
            else if (ViewModel.NavViewFooterMenuItems.Any(i => i.Tag as Type == targettype))
            {
                ViewModel.SelectedMenuItem = ViewModel.NavViewFooterMenuItems.Where(i => i.Tag as Type == targettype).FirstOrDefault();
            }
            else
            {
                ViewModel.SelectedMenuItem = null;
            }
        }

        private void InitNavViewMenuItems()
        {
            IEnumerable<NavigationViewItem> CreateMenuItems()
            {
                //yield return WinUIHelper.CreateNavViewMenuItem("Dashboard", Symbol.Home, typeof(HomeViewModel));
                yield return WinUIHelper.CreateNavViewMenuItem("Test Event", 0xE787, typeof(TestEventViewModel));
                yield return WinUIHelper.CreateNavViewMenuItem("Manage Testers", 0xE716, typeof(TestersViewModel));
            }

            ViewModel.NavViewMenuItems = new ObservableCollection<NavigationViewItem>();
            foreach (var item in CreateMenuItems())
            {
                ViewModel.NavViewMenuItems.Add(item);
            }
        }

        private void InitNavViewFooterMenuItems()
        {
            IEnumerable<NavigationViewItem> CreateMenuItems()
            {
                //yield return WinUIHelper.CreateNavViewMenuItem("Preferences", 0xEF58, typeof(PreferenceViewModel));
                yield return WinUIHelper.CreateNavViewMenuItem("Preferences", Symbol.Setting, typeof(PreferenceViewModel));
                yield return WinUIHelper.CreateNavViewMenuItem("About", 0xEE57, typeof(AboutViewModel));
            }

            ViewModel.NavViewFooterMenuItems = new ObservableCollection<NavigationViewItem>();

            foreach (var item in CreateMenuItems())
            {
                ViewModel.NavViewFooterMenuItems.Add(item);
            }
        }

        private void OnMenuItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var item = args.InvokedItemContainer as NavigationViewItem;
            if (item == null || item.Tag as Type == NavigationService.GetViewModel(mainFrame.CurrentSourcePageType)) return;

            ViewModel.NavigationService.Navigate(item.Tag as Type); // No parameter.
        }

        private void OnPaneOpenedOrClosedChanged(NavigationView sender, object args)
            => AppSettings.LocalSettingsContainer.Values[AppKeyTokens.PaneOpenOrClose] = navView.IsPaneOpen;
    }
}