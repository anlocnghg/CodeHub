using LocLib.IData;
using LocLib.WinUI.Services;
using LocLib.WinUI.ViewModels;

using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using TSS.Lib.Common.Models;

namespace TSS.Lib.ViewModels
{
    public class TestersViewModel : ViewModelBase
    {
        private readonly IDataCRUD<Tester> _dataService;

        public TestersViewModel(ICommonServices commonServices, IDataCRUD<Tester> dataService) : base(commonServices)
        {
            _dataService = dataService;

            //MessagingService.Subscribe<NewTesterViewModel, int>(this, MessagingTokens.NewTesterAdded, async (sender, payload) =>
            //{
            //    await OnNewTesterAdd(sender, payload);
            //});
        }

        private async Task OnNewTesterAdd(NewTesterViewModel sender, int id)
        {
            await Task.CompletedTask;
            //MessagingService.Unsubscribe<NewTesterViewModel, int>(this, MessagingTokens.NewTesterAdded);
        }

        #region Properties

        public ObservableCollection<TesterViewModelWrapper> TesterWrappers { get; } = new();

        #region SelectedTester

        private TesterViewModelWrapper _selectedTesterWrapper;

        public TesterViewModelWrapper SelectedTesterWrapper
        {
            get => _selectedTesterWrapper;
            set
            {
                if (IsEditMode)
                {
                    SwitchEditMode();
                }

                SetProperty(ref _selectedTesterWrapper, value);
            }
        }

        #endregion SelectedTester

        #region TesterCount

        private string _testerCountStr;

        public string TesterCountStr
        {
            get { return _testerCountStr; }
            set { SetProperty(ref _testerCountStr, value); }
        }

        #endregion TesterCount

        #region IsEditMode

        private bool _isEditMode;

        public bool IsEditMode
        {
            get { return _isEditMode; }
            set { SetProperty(ref _isEditMode, value); }
        }

        #endregion IsEditMode

        #endregion Properties

        #region Methods

        public async Task LoadTestersAsync()
        {
            TesterWrappers.Clear();

            var testers = await _dataService.GetAllAsIEnumerableAsync();

            foreach (var tester in testers)
            {
                TesterWrappers.Add(new TesterViewModelWrapper(CommonServices, tester, _dataService));
            }

            TesterCountStr = $"Testers ({TesterWrappers.Count})";
        }

        public async Task CancelAsync()
        {
            int selectedId = 0;
            if (SelectedTesterWrapper != null) selectedId = SelectedTesterWrapper.Id;
            await LoadTestersAsync();
            SelectedTesterWrapper = TesterWrappers.SingleOrDefault(t => t.Id == selectedId);

            if (IsEditMode)
            {
                SwitchEditMode();
            }
        }

        public async Task DeleteAsync()
        {
            if (SelectedTesterWrapper != null)
            {
                Tester testerToDelete = await _dataService.GetAsync(SelectedTesterWrapper.Id);
                if (testerToDelete != null)
                {
                    await _dataService.DeleteAsync(testerToDelete);
                    await LoadTestersAsync();
                }
            }
        }

        public async Task SaveAsync()
        {
            if (SelectedTesterWrapper.CanSave)
            {
                await SelectedTesterWrapper.SaveAsync();

                int id = SelectedTesterWrapper.Id;
                await LoadTestersAsync();
                SelectedTesterWrapper = TesterWrappers.Single(t => t.Id == id);

                SwitchEditMode();
            }
        }

        public void SwitchEditMode() => IsEditMode = !IsEditMode;

        public void GoToAddNewTesterPage() => NavigationService.Navigate<NewTesterViewModel>();

        #endregion Methods
    }
}