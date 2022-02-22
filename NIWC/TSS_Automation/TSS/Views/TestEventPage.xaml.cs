using LocLib.WinUI.ViewModels;

using Microsoft.UI.Xaml.Controls;

using System;

using TSS.Lib.ViewModels;

namespace TSS.Views
{
    public sealed partial class TestEventPage : Page
    {
        public TestEventViewModel ViewModel { get; }

        public TestEventPage()
        {
            this.InitializeComponent();

            // Setup RDO CalendarDatePicker
            {
                myRDOCalendarDatePicker.MaxDate = DateTimeOffset.Now;
                myRDOCalendarDatePicker.MinDate = DateTimeOffset.Now.AddMonths(-1);

                myRDOCalendarDatePicker.DateFormat = "{dayofweek.abbreviated}, {month.abbreviated} {day.integer}, {year.full}";
            }

            ViewModel = DependencyContainer.GetService<TestEventViewModel>();
        }

        private void myRDOCalendarDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            if (myRDOCalendarDatePicker.Date == null)
            {
                ShellViewModel.Current.Status = $"Invalid selected RDO or it was selected twice.";

                if (ViewModel.KnownRDO != myRDOCalendarDatePicker.MinDate)
                {
                    ViewModel.KnownRDO = myRDOCalendarDatePicker.MinDate;
                }
                else if (ViewModel.KnownRDO != myRDOCalendarDatePicker.MaxDate)
                {
                    ViewModel.KnownRDO = myRDOCalendarDatePicker.MaxDate;
                }
            }
        }
    }
}