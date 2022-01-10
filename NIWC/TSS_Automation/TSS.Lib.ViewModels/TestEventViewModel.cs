using LocLib.Extensions;
using LocLib.IData;
using LocLib.WinUI.Services;
using LocLib.WinUI.ViewModels;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
        }

        #region EventName

        private string _eventName;

        public string EventName
        {
            get => _eventName;
            set => SetProperty(ref _eventName, value);
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
                OnScheduleChanged();
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
                OnScheduleChanged();
            }
        }

        #endregion EndDate

        #region AreTestDatesValid

        public bool AreTestDatesValid => !(StartDate.DateTime <= DateTime.Now
            || EndDate.DateTime <= DateTime.Now
            || StartDate >= EndDate);

        #endregion AreTestDatesValid

        #region TestDates

        public ObservableCollection<EachTestDate> TestDates { get; set; } = new();

        public void OnScheduleChanged()
        {
            TestDates?.Clear();

            if (!AreTestDatesValid)
            {
                ShellViewModel.Current.Status = $"Start Date must be before End Date, and both must be later than today date.";
            }
            else
            {
                foreach (var day in TSSHelper.AllDatesBetween(StartDate.DateTime, EndDate.DateTime))
                {
                    TestDates.Add(new EachTestDate { Date = day });
                }

                RaisePropertyChanged(nameof(TestDates));
                ShellViewModel.Current.Status = string.Empty;
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

                // NO else
                if (eachTestDate.Has2ndShift)
                {
                    slots.Add(new Slot { Date = eachTestDate.Date, Shift = WhatShift.SecondShift });
                }

                // NO else
                if (eachTestDate.Has3rdShift)
                {
                    slots.Add(new Slot { Date = eachTestDate.Date, Shift = WhatShift.ThirdShift });
                }
            }

            if (slots.Count > 0)
            {
                ShellViewModel.Current.Status = string.Empty;

                //NavigationService.Navigate<TestEventAssignmentsViewModel>(Tuple.Create(EventName, slots));
                // Could have included parameters with the navigation, but let's use Messaging Service instead. 

                NavigationService.Navigate<TestEventAssignmentsViewModel>();

                MessagingService.Send<TestEventViewModel, Tuple<string, List<Slot>>>(this, MessagingTokens.TestEventAssignmentRequested, Tuple.Create(EventName, slots));
            }
            else
            {
                ShellViewModel.Current.Status = "No input, please pick at least a shift.";
            }
        }
    }
}
