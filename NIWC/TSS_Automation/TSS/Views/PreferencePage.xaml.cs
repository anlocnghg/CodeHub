using Microsoft.UI.Xaml.Controls;

using System;
using System.Collections.Generic;
using System.Linq;

using TSS.Lib.Common.Models;
using TSS.Lib.ViewModels;

namespace TSS.Views
{
    public sealed partial class PreferencePage : Page
    {
        public PreferenceViewModel ViewModel { get; private set; }

        public PreferencePage()
        {
            this.InitializeComponent();

            ViewModel = DependencyContainer.GetService<PreferenceViewModel>();
        }

        private void SelectedDayOfWeeks_Changed(object sender, SelectionChangedEventArgs e)
            => ViewModel.SelectedDayOfWeeks = myDayOfWeeksListView.SelectedItems.Cast<DayOfWeek>().ToList();

        private void SelectedShifts_Changed(object sender, SelectionChangedEventArgs e)
            => ViewModel.SelectedShifts = myShiftsListView.SelectedItems.Cast<WhatShift>().ToList();

        private void SelectedDates_Changed(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            ViewModel.SelectedOffDates = new List<DateTimeOffset>();
            foreach (var date in myCalendarView.SelectedDates)
            {
                ViewModel.SelectedOffDates.Add(date);
            }
        }
    }
}