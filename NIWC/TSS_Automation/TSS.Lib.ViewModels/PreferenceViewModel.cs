using LocLib.IData;
using LocLib.WinUI.Services;
using LocLib.WinUI.ViewModels;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using TSS.Lib.Common.Models;

namespace TSS.Lib.ViewModels
{
    public class PreferenceViewModel : ViewModelBase
    {
        private readonly IDataCRUD<Tester> _dataService;
        private readonly IDataCRUD<TesterPreference> _testerPreferenceDataService;

        public PreferenceViewModel(ICommonServices commonServices, IDataCRUD<Tester> dataService, IDataCRUD<TesterPreference> testerPreferenceDataService) : base(commonServices)
        {
            _dataService = dataService;
            _testerPreferenceDataService = testerPreferenceDataService;
        }

        #region Properties

        public ObservableCollection<TesterPreference> TesterPreferences { get; set; } = new();
        public ObservableCollection<Tester> Testers { get; set; } = new();

        #region SelectedTester

        private Tester _selectedTester;

        public Tester SelectedTester
        {
            get { return _selectedTester; }
            set { SetProperty(ref _selectedTester, value); }
        }

        #endregion SelectedTester

        #region MaxAssignments

        private int _maxAssignments = 5;

        public int MaxAssignments
        {
            get { return _maxAssignments; }
            set { SetProperty(ref _maxAssignments, value); }
        }

        #endregion MaxAssignments

        public List<DayOfWeek> DayOfWeeks { get; set; } = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday };

        public List<DayOfWeek> SelectedDayOfWeeks { get; set; }

        public List<WhatShift> Shifts { get; set; } = new() { WhatShift.FirstShift, WhatShift.SecondShift, WhatShift.ThirdShift };

        public List<WhatShift> SelectedShifts { get; set; }

        public ObservableCollection<Tester> PreferredTesters { get; set; } = new();

        #region SelectedPreferredTester

        private Tester _selectedPreferredTester;

        public Tester SelectedPreferredTester
        {
            get { return _selectedPreferredTester; }
            set { SetProperty(ref _selectedPreferredTester, value); }
        }

        #endregion SelectedPreferredTester

        private List<DateTimeOffset> _selectedOffDates;

        public List<DateTimeOffset> SelectedOffDates
        {
            get { return _selectedOffDates; }
            set { SetProperty(ref _selectedOffDates, value); }
        }

        #endregion Properties

        public async Task SetPreferencesForSelectedTester()
        {
            if (SelectedTester != null)
            {
                List<DateTime> offDates = new List<DateTime>();
                if (SelectedOffDates != null)
                {
                    foreach (var date in SelectedOffDates)
                    {
                        DateTime offDate = date.DateTime;
                        offDates.Add(offDate);
                    }
                }

                TesterPreference toBeUpdated = new TesterPreference(SelectedTester)
                {
                    MaxAssignmentsCount = MaxAssignments,
                    PreferredDays = SelectedDayOfWeeks,
                    PreferredShifts = SelectedShifts,
                    NotAvailableDates = offDates.Count > 0 ? offDates : null
                };

                if (SelectedPreferredTester != null)
                {
                    toBeUpdated.CoTesterId = SelectedPreferredTester.Id;
                    toBeUpdated.CoTesterName = SelectedPreferredTester.Name;
                }

                await _testerPreferenceDataService.UpdateAsync(toBeUpdated);

                await LoadPreferencesAsync();
            }
        }

        public async Task LoadTestersAsync()
        {
            Testers?.Clear();
            var allTesters = await _dataService.GetAllAsListAsync();
            foreach (var tester in allTesters)
            {
                Testers.Add(tester);
            }
        }

        public async Task WhenSelectedTesterChangedAsync()
        {
            if (SelectedTester != null)
            {
                PreferredTesters?.Clear();
                var allTesters = await _dataService.GetAllAsListAsync();
                foreach (var tester in allTesters)
                {
                    if (tester != SelectedTester)
                        PreferredTesters.Add(tester);
                }
            }
        }

        public async Task LoadPreferencesAsync()
        {
            TesterPreferences?.Clear();
            var allTesterPreferences = await _testerPreferenceDataService.GetAllAsListAsync();
            foreach (var testerPreference in allTesterPreferences)
            {
                TesterPreferences.Add(testerPreference);
            }
        }
    }
}