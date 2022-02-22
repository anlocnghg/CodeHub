using LocLib.IData;
using LocLib.Messaging.MessagingService;
using LocLib.WinUI.Services;
using LocLib.WinUI.ViewModels;

using Microsoft.Extensions.DependencyInjection;

using System;

using TSS.Lib.Common.DataServices;
using TSS.Lib.Common.Models;
using TSS.Lib.ViewModels;
using TSS.Views;

namespace TSS
{
    public static class DependencyContainer
    {
        private static IServiceProvider _serviceProvider = null;

        public static void RegisterAndConfigureDependencies()
        {
            IServiceCollection services = new ServiceCollection();

            RegisterAndConfigureCoreServices(services);
            RegisterViewModels(services);

            _serviceProvider = services.BuildServiceProvider();
        }

        private static void RegisterAndConfigureCoreServices(IServiceCollection services)
        {
            #region Core services: Navigation, Messaging Bus, Dialog, etc.

            #region Navigation

            services.AddSingleton<NavigationService>();

            {
                // Map (ViewModel, View) pairs for Navigation Service
                // NavigationService.Register<HomeViewModel, HomePage>();
                NavigationService.Register<TestersViewModel, TestersPage>();
                NavigationService.Register<TestEventViewModel, TestEventPage>();
                NavigationService.Register<TestEventAssignmentsViewModel, TestEventAssignmentsPage>();
                NavigationService.Register<PreferenceViewModel, PreferencePage>();
                NavigationService.Register<AboutViewModel, AboutPage>();
                NavigationService.Register<NewTesterViewModel, NewTesterPage>();
            }

            #endregion Navigation

            services.AddSingleton<IMessagingService, MessagingService>();

            services.AddSingleton<ICommonServices, CommonServices>();

            #endregion Core services: Navigation, Messaging Bus, Dialog, etc.

            #region Application-specific services

            services.AddSingleton<IDataCRUD<Tester>, InMemoryTesterDataService>();
            services.AddSingleton<IDataCRUD<TesterPreference>, InMemoryPreferenceDataService>();

            #endregion Application-specific services
        }

        private static void RegisterViewModels(IServiceCollection services)
        {
            services.AddTransient<ShellViewModel>();
            services.AddTransient<AboutViewModel>();
            services.AddTransient<PreferenceViewModel>();
            // services.AddTransient<HomeViewModel>();
            services.AddTransient<TestersViewModel>();
            services.AddTransient<TesterViewModelWrapper>();
            services.AddTransient<TestEventViewModel>();
            services.AddTransient<TestEventAssignmentsViewModel>();
            services.AddTransient<NewTesterViewModel>();
        }

        #region GetService<T>

        public static T GetService<T>() => GetService<T>(true);

        public static T GetService<T>(bool isRequired)
            => isRequired ? _serviceProvider.GetRequiredService<T>() : _serviceProvider.GetService<T>();

        #endregion GetService<T>
    }
}