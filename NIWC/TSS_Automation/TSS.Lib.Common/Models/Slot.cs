using LocLib;

namespace TSS.Lib.Common.Models
{
    /// <summary>
    /// Each Test Assignment Slot contains two testers (at least one must be Primary), and other info such as date and shift. Think of it as a 'slot' to fill in testers.
    /// </summary>
    public class Slot : ObservableObject
    {
        #region Properties

        public DateTime Date { get; set; }

        public WhatShift Shift { get; set; }

        #region Tester_1

        private Tester _tester_1;

        public Tester Tester_1
        {
            get { return _tester_1; }
            set
            {
                SetProperty(ref _tester_1, value);
                Name_1 = Tester_1 == null ? null : Tester_1.Name;

                CheckIfTwoTestersAreTheSame();
            }
        }

        #endregion Tester_1

        #region Name_1

        private string _name_1;

        public string Name_1
        {
            get { return _name_1; }
            set { SetProperty(ref _name_1, value); }
        }

        #endregion Name_1

        #region Tester_2

        private Tester _tester_2;

        public Tester Tester_2
        {
            get { return _tester_2; }
            set
            {
                SetProperty(ref _tester_2, value);
                Name_2 = Tester_2 == null ? null : Tester_2.Name;

                CheckIfTwoTestersAreTheSame();
            }
        }

        #endregion Tester_2

        #region Name_2

        private string _name_2;

        public string Name_2
        {
            get { return _name_2; }
            set { SetProperty(ref _name_2, value); }
        }

        #endregion Name_2

        #endregion Properties

        #region Methods

        private void CheckIfTwoTestersAreTheSame()
        {
            if (Tester_1 != null && Tester_2 != null && Tester_1.Equals(Tester_2))
            {
                throw new ArgumentException($"Two Testers in an assignment slot must be different.");
            }
        }

        #endregion Methods

        #region Re-define equality

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Slot slot = (Slot)obj;
                if (slot.Date != Date || slot.Shift != Shift)
                {
                    return false;
                }
                else
                {
                    var twoTesters = new List<Tester>() { slot.Tester_1, slot.Tester_2 };
                    return twoTesters.Exists(t => t.Equals(Tester_1))
                        && twoTesters.Exists(t => t.Equals(Tester_2));
                }
            }
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                // Pick two different prime numbers
                int hash = 13;

                // To do: check nullity!
                hash = hash * 7 + Date.GetHashCode();
                hash = hash * 7 + Shift.GetHashCode();

                hash = hash * 7 + Tester_1.Id.GetHashCode();
                hash = hash * 7 + Tester_1.Name.GetHashCode();
                hash = hash * 7 + Tester_1.IsPrimary.GetHashCode();
                hash = hash * 7 + Tester_1.IsTestTeam.GetHashCode();
                hash = hash * 7 + Tester_1.IsGovee.GetHashCode();

                hash = hash * 7 + Tester_2.Id.GetHashCode();
                hash = hash * 7 + Tester_2.Name.GetHashCode();
                hash = hash * 7 + Tester_2.IsPrimary.GetHashCode();
                hash = hash * 7 + Tester_2.IsTestTeam.GetHashCode();
                hash = hash * 7 + Tester_2.IsGovee.GetHashCode();
                return hash;
            }
        }

        #endregion Re-define equality
    }
}
