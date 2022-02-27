using LocLib.Extensions;
using LocLib.WinUI.Services;
using LocLib.WinUI.ViewModels;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using TSS.Lib.Common;
using TSS.Lib.Common.Models;

namespace TSS.Lib.ViewModels
{
    public class TestEventViewModel : ViewModelBase
    {
        public TestEventViewModel(ICommonServices commonServices) : base(commonServices) => Init();

        private void Init()
        {
            StartDate = DateTime.Now.AddDays(1);
            EndDate = DateTime.Now.AddDays(7);

            #region Known RDO

            DateTime aKnowRFO = new DateTime(2021, 12, 3);
            foreach (DateTime date in TSSHelper.DateTimesBetween(DateTime.Now.AddMonths(-1), DateTime.Now))
            {
                if ((date.Date - aKnowRFO.Date).Days % 14 == 0)
                {
                    KnownRDO = new DateTimeOffset(new DateTime(date.Year, date.Month, date.Day));
                }
            }

            #endregion Known RDO
        }

        #region EventName

        private string _eventName;

        public string EventName
        {
            get => _eventName;
            set
            {
                SetProperty(ref _eventName, value);
                RaisePropertyChanged(nameof(IsReadyToGenerateAssignments));
            }
        }

        #endregion EventName

        #region StartDate

        private DateTimeOffset _startDate;

        public DateTimeOffset StartDate
        {
            get => _startDate;
            set
            {
                SetProperty(ref _startDate, value);
                RaisePropertyChanged(nameof(AreTestDatesValid));
                RaisePropertyChanged(nameof(IsReadyToGenerateAssignments));
            }
        }

        #endregion StartDate

        #region EndDate

        private DateTimeOffset _endDate;

        public DateTimeOffset EndDate
        {
            get => _endDate;
            set
            {
                SetProperty(ref _endDate, value);
                RaisePropertyChanged(nameof(AreTestDatesValid));
                RaisePropertyChanged(nameof(IsReadyToGenerateAssignments));
            }
        }

        #endregion EndDate

        #region KnownRDO

        private DateTimeOffset _knownRDO;

        public DateTimeOffset KnownRDO
        {
            get => _knownRDO;
            set
            {
                SetProperty(ref _knownRDO, value);
                SetIsRDOForTestDates();
            }
        }

        #endregion KnownRDO

        #region AreTestDatesValid

        public bool AreTestDatesValid
        {
            get
            {
                // if (EventName.IsNullOrEmpty()) return false;
                if (
                    (StartDate.DateTime < DateTime.Now)
                    || (EndDate.DateTime < DateTime.Now)
                    || (StartDate.DateTime > EndDate.DateTime)
                    || (EndDate.DateTime > StartDate.DateTime.AddMonths(6))
                    )
                {
                    ShellViewModel.Current.ShellStatusBackground = "Red";
                    ShellViewModel.Current.Status = $"Start Date must be later than today and End Date must be later than Start Date (but not more than 6 months).";
                    return false;
                }

                if (StartDate.DateTime >= DateTime.Now
                    && EndDate.DateTime >= StartDate.DateTime
                    && EndDate.DateTime <= StartDate.DateTime.AddMonths(6)
                    )
                {
                    ShellViewModel.Current.ShellStatusBackground = "Gray";
                    ShellViewModel.Current.Status = string.Empty;
                    return true;
                }

                return false;
            }
        }

        public bool IsReadyToGenerateAssignments => (!EventName.IsNullOrWhiteSpace() && AreTestDatesValid);

        #endregion AreTestDatesValid

        #region TestDates

        public ObservableCollection<EachTestDate> TestDates { get; set; } = new();

        public void SetIsRDOForTestDates()
        {
            foreach (var testDate in TestDates)
            {
                SetIsRDOForEachTestDate(testDate);
            }
        }

        private void SetIsRDOForEachTestDate(EachTestDate testDate)
        {
            testDate.IsRDO = false;

            int days = (testDate.Date - KnownRDO.DateTime).Days;
            bool isRDO = days % 14 == 0;
            testDate.IsRDO = isRDO;
        }

        public async Task OnScheduleChangedAsync()
        {
            TestDates?.Clear();
            if (AreTestDatesValid)
            {
                foreach (var day in TSSHelper.DateTimesBetween(StartDate.Date, EndDate.Date))
                {
                    EachTestDate newTestDate = new EachTestDate { Date = day };
                    SetIsRDOForEachTestDate(newTestDate);

                    TestDates.Add(newTestDate);
                    await Task.Delay(25);
                }
            }
        }

        #endregion TestDates

        public void GenerateAssignments()
        {
            List<Slot> slots = new();

            foreach (var eachTestDate in TestDates)
            {
                if (eachTestDate.Has1stShift)
                {
                    slots.Add(new Slot { Date = eachTestDate.Date, Shift = WhatShift.FirstShift });
                }

                if (eachTestDate.Has2ndShift)
                {
                    slots.Add(new Slot { Date = eachTestDate.Date, Shift = WhatShift.SecondShift });
                }

                if (eachTestDate.Has3rdShift)
                {
                    slots.Add(new Slot { Date = eachTestDate.Date, Shift = WhatShift.ThirdShift });
                }
            }

            if (slots.Count > 0)
            {
                ShellViewModel.Current.ShellStatusBackground = "Gray";
                ShellViewModel.Current.Status = string.Empty;

                // NavigationService.Navigate<TestEventAssignmentsViewModel>(Tuple.Create(EventName, slots));
                // Could have included parameters with the navigation, but let's use Messaging Service instead.

                NavigationService.Navigate<TestEventAssignmentsViewModel>();

                MessagingService.Send<TestEventViewModel, Tuple<string, List<Slot>>>(this, MessagingTokens.TestEventAssignmentRequested, Tuple.Create(EventName, slots));
            }
            else
            {
                ShellViewModel.Current.ShellStatusBackground = "Orange";
                ShellViewModel.Current.Status = "No input, please pick at least a shift.";
            }
        }
    }
}