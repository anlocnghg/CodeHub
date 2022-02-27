using LocLib;
using LocLib.IData;
using LocLib.WinUI.Services;
using LocLib.WinUI.ViewModels;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using TSS.Lib.Common;
using TSS.Lib.Common.Models;

namespace TSS.Lib.ViewModels
{
    public partial class TestEventAssignmentsViewModel : ViewModelBase
    {
        private IDataCRUD<Tester> _testerDataService;
        private IDataCRUD<TesterPreference> _testerPreferenceDataService;

        public TestEventAssignmentsViewModel(ICommonServices commonServices, IDataCRUD<Tester> dataService, IDataCRUD<TesterPreference> testerPreferenceDataService) : base(commonServices)
        {
            _testerDataService = dataService;
            _testerPreferenceDataService = testerPreferenceDataService;

            MessagingService.Subscribe<TestEventViewModel, Tuple<string, List<Slot>>>(this, MessagingTokens.TestEventAssignmentRequested, async (sender, payload) =>
            {
                EventName = payload.Item1;
                Slots = payload.Item2;

                await GenerateAssignmentsAsync();

                MessagingService.Unsubscribe(this, MessagingTokens.TestEventAssignmentRequested);
            });
        }

        #region Properties

        #region EventName

        private string _eventName;

        public string EventName
        {
            get => _eventName;
            set => SetProperty(ref _eventName, value);
        }

        #endregion EventName

        // This property is to keep track of how many assignments that each tester is assigned to support so far.
        public ObservableCollection<TesterAssignmentDetails> AssignmentsDetails { get; private set; } = new();

        public List<Slot> Slots { get; set; } // Get from parameter

        private List<Tester> Testers { get; set; }

        private List<TesterPreference> Preferences { get; set; }

        #endregion Properties

        #region Methods

        public async Task GenerateAssignmentsAsync()
        {
            await FillInSlotsAsync();

            #region Verifying, if not completed, give x times to do it again.

            int times = 0;
            if (!AllSlotsAreFilled())
            {
                while (times < 3)
                {
                    await FillInSlotsAsync();

                    if (AllSlotsAreFilled())
                    {
                        ShellViewModel.Current.ShellStatusBackground = "Green";
                        ShellViewModel.Current.Status = $"The test support assignment has been completed.";

                        break;
                    }
                    else
                    {
                        ShellViewModel.Current.ShellStatusBackground = "Orange";
                        ShellViewModel.Current.Status = $"Could not fully schedule the assignment.";
                    }

                    times++;
                }
            }
            else
            {
                ShellViewModel.Current.ShellStatusBackground = "Green";
                ShellViewModel.Current.Status = $"The test support assignment has been completed.";
            }

            #endregion Verifying, if not completed, give x times to do it again.
        }

        #region Fill in slots

        private async Task FillInSlotsAsync()
        {
            foreach (var slot in Slots)
            {
                slot.Tester_1 = null;
                slot.Tester_2 = null;
            }

            #region Assignments counts

            AssignmentsDetails.Clear();
            Testers = await _testerDataService.GetAllAsListAsync();
            foreach (var tester in Testers)
            {
                AssignmentsDetails.Add(new TesterAssignmentDetails { Id = tester.Id, Name = tester.Name, AssignmentsCount = 0 });
            }

            #endregion Assignments counts

            ShellViewModel.Current.ShellStatusBackground = "Blue";

            ShellViewModel.Current.Status = "Working in progress... please be patient";
            ShellViewModel.Current.SetProgress(true);

            await FillInTestersAsync();

            ShellViewModel.Current.Status = string.Empty;
            ShellViewModel.Current.SetProgress(false, 0);
        }

        private async Task FillInTestersAsync()
        {
            Preferences = await _testerPreferenceDataService.GetAllAsListAsync();

            var slotsWeeks = Slots
                .GroupBy(s => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(s.Date.Date, CalendarWeekRule.FirstDay, DayOfWeek.Monday))
                .Select(group => group.ToList())
                .ToList();

            foreach (var slotsInThisWeek in slotsWeeks)
            {
                foreach (Slot slot in slotsInThisWeek)
                {
                    #region Tester #1

                    #region Tester #1 filter

                    // Exclude testers with unavailable date falls into this slot's date
                    List<TesterPreference> tester1Candidates = Preferences.Where(p => (p.NotAvailableDates == null) || (p.NotAvailableDates != null && !p.NotAvailableDates.Any(d => d.Date == slot.Date.Date))).ToList();

                    // Make sure that Tester #1 must be primary
                    tester1Candidates = tester1Candidates.Where(p => p.Tester.IsPrimary).ToList();

                    // Consider only if the assignments count has not reached the max
                    tester1Candidates = tester1Candidates.Where(p => p.AssignmentsCount < p.MaxAssignmentsCount).ToList();

                    // Consider preferred days of week
                    tester1Candidates = tester1Candidates.Where(p => (p.PreferredDays == null) || (p.PreferredDays != null && p.PreferredDays.Any(d => d == slot.Date.DayOfWeek))).ToList();

                    // Consider preferred shifts
                    tester1Candidates = tester1Candidates.Where(p => (p.PreferredShifts == null) || (p.PreferredShifts.Any(s => s == slot.Shift))).ToList();

                    #endregion Tester #1 filter

                    if (tester1Candidates.Any())
                    {
                        TesterPreference tester1Preference = tester1Candidates.PickRandom();
                        slot.Tester_1 = tester1Preference.Tester;
                        tester1Preference.AssignmentsCount++;

                        UpdateDetailsCounts(slot.Tester_1.Id);

                        await Task.Delay(100);

                        #region Tester #2

                        #region Tester #2 filter

                        // Exclude testers with unavailable date falls into this slot's date
                        List<TesterPreference> tester2Candidates = Preferences.Where(p => (p.NotAvailableDates == null) || (p.NotAvailableDates != null && !p.NotAvailableDates.Any(d => d.Date == slot.Date.Date))).ToList();

                        // Consider only if the assignments count has not reached the max
                        tester2Candidates = tester2Candidates.Where(p => p.AssignmentsCount < p.MaxAssignmentsCount).ToList();

                        // Consider preferred days of week
                        tester2Candidates = tester2Candidates.Where(p => (p.PreferredDays == null) || (p.PreferredDays != null && p.PreferredDays.Any(d => d == slot.Date.DayOfWeek))).ToList();

                        // Consider preferred shifts
                        tester2Candidates = tester2Candidates.Where(p => (p.PreferredShifts == null) || (p.PreferredShifts.Any(s => s == slot.Shift))).ToList();

                        // Tester #2 must not be the same with tester #1
                        tester2Candidates = tester2Candidates.Where(p => p.Tester != slot.Tester_1).ToList();

                        #endregion Tester #2 filter

                        if (tester2Candidates.Any())
                        {
                            // Consider co-tester (two-way and one-way)
                            TesterPreference preferredCoTesterOfTester1 = tester2Candidates.SingleOrDefault(p => p.Tester.Id == tester1Preference.CoTesterId);

                            if (preferredCoTesterOfTester1 != null)
                            {
                                // This means in all candidates for Tester#2 there exists a co-tester that Tester#1 prefers.
                                // Now, go figure out if that one prefers the Tester#1 (two-way) or not (one-way).
                                if (preferredCoTesterOfTester1.CoTesterId == tester1Preference.Tester.Id)
                                {
                                    // Two-way
                                }
                                else
                                {
                                    // One-way: Tester #1 prefers this co-tester, but this co-tester doesn't prefer Tester #1.
                                }

                                // Doesn't change: we set this as Tester #2 anyway. Best if two-way, but at least we get one-way.
                                slot.Tester_2 = preferredCoTesterOfTester1.Tester;
                                preferredCoTesterOfTester1.AssignmentsCount++;
                            }
                            else
                            {
                                TesterPreference tester2Preference = tester2Candidates.PickRandom();
                                slot.Tester_2 = tester2Preference.Tester;
                                tester2Preference.AssignmentsCount++;
                            }

                            UpdateDetailsCounts(slot.Tester_2.Id);
                            await Task.Delay(100);
                        }
                        else
                        {
                            // Log: not able to assign Tester #2 based on input/preferences
                        }

                        #endregion Tester #2
                    }
                    else
                    {
                        // Log: not able to assign Tester #1 based on input/preferences
                    }

                    #endregion Tester #1
                }

                // Reset assignments count after each week
                foreach (var preference in Preferences)
                {
                    preference.AssignmentsCount = 0;
                }
            }
        }

        #endregion Fill in slots

        private bool AllSlotsAreFilled()
        {
            foreach (Slot slot in Slots)
            {
                if (slot.Tester_1 == null || slot.Tester_2 == null)
                {
                    return false;
                }
            }

            return true;
        }

        private void UpdateDetailsCounts(int id)
        {
            foreach (TesterAssignmentDetails testerAssignmentDetails in AssignmentsDetails)
            {
                if (testerAssignmentDetails.Id == id)
                {
                    testerAssignmentDetails.AssignmentsCount++;
                }
            }
        }

        #endregion Methods
    }
}