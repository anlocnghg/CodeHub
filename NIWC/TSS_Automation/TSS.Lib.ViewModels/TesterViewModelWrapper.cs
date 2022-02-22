using LocLib.Extensions;
using LocLib.IData;
using LocLib.WinUI.Services;
using LocLib.WinUI.ViewModels;

using System.Threading.Tasks;

using TSS.Lib.Common.Models;

namespace TSS.Lib.ViewModels
{
    public class TesterViewModelWrapper : ViewModelBase
    {
        private readonly IDataCRUD<Tester> _dataService;

        public TesterViewModelWrapper(ICommonServices commonServices, Tester tester, IDataCRUD<Tester> dataService) : base(commonServices)
        {
            // Current = tester; // We don't want this. We want to emulate persistent layer (XML/SQL/file)
            ItShelf = new Tester
            {
                Id = tester.Id,
                Name = tester.Name,
                IsPrimary = tester.IsPrimary,
                IsTestTeam = tester.IsTestTeam,
                IsGovee = tester.IsGovee
            };

            {
                Id = ItShelf.Id;
                Name = ItShelf.Name;
                IsPrimary = ItShelf.IsPrimary;
                IsTestTeam = ItShelf.IsTestTeam;
                IsGovee = ItShelf.IsGovee;
            }

            _dataService = dataService;
        }

        #region Properties

        public Tester ItShelf { get; private set; }

        #region Id

        public int Id
        {
            get { return ItShelf.Id; }
            set
            {
                if (ItShelf.Id != value)
                {
                    ItShelf.Id = value;
                    RaisePropertyChanged();
                }
            }
        }

        #endregion Id

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

        public async Task SaveAsync()
        {
            if (CanSave)
            {
                await _dataService.UpdateAsync(new Tester { Id = Id, Name = Name, IsPrimary = IsPrimary, IsTestTeam = IsTestTeam, IsGovee = IsGovee });
            }
        }

        #endregion Methods
    }
}