namespace TSS.Lib.Common.Models
{
    public class TesterPreference
    {
        public TesterPreference(Tester tester) => Tester = tester;

        #region Properties set by user

        public Tester Tester { get; private set; }

        // public Tester CoTester { get; set; }
        public int CoTesterId { get; set; } = -1;

        public string CoTesterName { get; set; }
        public int MaxAssignmentsCount { get; set; } = 5; // Per week
        public List<DayOfWeek> PreferredDays { get; set; } // = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };
        public List<WhatShift> PreferredShifts { get; set; } // = new List<WhatShift> { WhatShift.FirstShift, WhatShift.SecondShift, WhatShift.ThirdShift };
        public List<DateTime> NotAvailableDates { get; set; }

        #endregion Properties set by user

        #region Calculated properties

        public int AssignmentsCount { get; set; } = 0; // Since max one assignment per day, this is also number of days this tester has been assigned to support.

        #endregion Calculated properties
    }
}