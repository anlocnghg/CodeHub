using LocLib;
using LocLib.Extensions;

namespace TSS.Lib.Common.Models
{
    /// <summary>
    /// This is to collect the MDA's input for each day schedule.
    /// </summary>
    public class EachTestDate : ObservableObject
    {
        public DateTime Date { get; set; }

        #region Has1stShift

        private bool _has1stShift;

        public bool Has1stShift { get => _has1stShift; set => SetProperty(ref _has1stShift, value); }

        #endregion Has1stShift

        #region Has2ndShift

        private bool _has2ndShift;

        public bool Has2ndShift { get => _has2ndShift; set => SetProperty(ref _has2ndShift, value); }

        #endregion Has2ndShift

        #region Has3rdShift

        private bool _has3rdShift;
        public bool Has3rdShift { get => _has3rdShift; set => SetProperty(ref _has3rdShift, value); }

        #endregion Has3rdShift

        #region Is weekend or holiday or RDO

        public bool IsWeekend => Date.IsWeekend();

        public bool IsHoliday => Date.IsHoliday();

        public bool IsRDO { get; set; }

        public bool IsWeekendOrHoliday => IsWeekend || IsHoliday;

        #endregion Is weekend or holiday or RDO

        public override string ToString() => Date.ToString("ddd, dd MMM yyyy");
    }
}