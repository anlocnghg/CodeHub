using LocLib.Extensions;
using LocLib.IData;
using LocLib.WinUI.Services;
using LocLib.WinUI.ViewModels;

using System;
using System.Threading.Tasks;

using TSS.Lib.Common.Models;

namespace TSS.Lib.ViewModels
{
    public class NewTesterViewModel : ViewModelBase
    {
        private readonly IDataCRUD<Tester> _dataService;

        public NewTesterViewModel(ICommonServices commonServices, IDataCRUD<Tester> dataService) : base(commonServices) => _dataService = dataService;

        #region Properties

        #region Name

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (!value.IsNullOrWhiteSpace())
                {
                    CanSave = true;

                    SetProperty(ref _name, value);
                }
                else
                {
                    CanSave = false;
                }
            }
        }

        #endregion Name

        #region IsPrimary

        private bool _isPrimary;

        public bool IsPrimary
        {
            get { return _isPrimary; }
            set { SetProperty(ref _isPrimary, value); }
        }

        #endregion IsPrimary

        #region IsTestTeam

        private bool _isTestTeam;

        public bool IsTestTeam
        {
            get { return _isTestTeam; }
            set { SetProperty(ref _isTestTeam, value); }
        }

        #endregion IsTestTeam

        #region IsGovee

        private bool _isGovee;

        public bool IsGovee
        {
            get { return _isGovee; }
            set { SetProperty(ref _isGovee, value); }
        }

        #endregion IsGovee

        #region CanSave

        private bool _canSave;

        public bool CanSave
        {
            get { return _canSave; }
            set { SetProperty(ref _canSave, value); }
        }

        #endregion CanSave

        #endregion Properties

        #region Methods

        public async Task AddAsync()
        {
            int id = await _dataService.AddAsync(new Tester { Name = Name, IsPrimary = IsPrimary, IsTestTeam = IsTestTeam, IsGovee = IsGovee });

            ShellViewModel.Current.Status = $"New Tester ({Name}) with Id {id} was created.";

            NavigationService.Navigate<TestersViewModel>();
            //MessagingService.Send<NewTesterViewModel, int>(this, MessagingTokens.NewTesterAdded, id);
        }

        #endregion Methods
    }
}