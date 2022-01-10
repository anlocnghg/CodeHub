namespace TSS.Lib.Common.Models
{
    public class Tester
    {
        #region Properties

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsTestTeam { get; set; }
        public bool IsGovee { get; set; }

        #endregion Properties

        #region Re-define equality

        // Ref:
        // https://docs.microsoft.com/en-us/dotnet/api/system.object.equals?view=net-5.0
        // https://www.loganfranken.com/blog/692/overriding-equals-in-c-part-2/
        // https://stackoverflow.com/questions/371328/why-is-it-important-to-override-gethashcode-when-equals-method-is-overridden
        // https://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-overriding-gethashcode

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType())) return false;
            else
            {
                Tester t = (Tester)obj;
                return (t.Id == Id && t.Name == Name && t.IsPrimary == IsPrimary && t.IsTestTeam == IsTestTeam && t.IsGovee == IsGovee);
            }
        }

        public override int GetHashCode()
        {
            // It's best just to return Id if can be sure that it's unique!

            unchecked // Overflow is fine, just wrap
            {
                // Pick two different prime numbers
                int hash = 13;

                // To do: check nullity!
                hash = hash * 7 + Id.GetHashCode();
                hash = hash * 7 + Name.GetHashCode();
                hash = hash * 7 + IsPrimary.GetHashCode();
                hash = hash * 7 + IsTestTeam.GetHashCode();
                hash = hash * 7 + IsGovee.GetHashCode();
                return hash;
            }
        }

        //public static bool operator ==(Tester a, Tester b) => a.Equals(b);

        //public static bool operator !=(Tester a, Tester b) => !(a == b);

        #endregion Re-define equality

        public override string ToString()
        {
            return $"[{Name}]";
        }
    }
}
