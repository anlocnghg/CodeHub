using TSS.Lib.Common.Models;

namespace TSS.Lib.Common
{
    public static class TSSHelper
    {
        //public static TesterPreference GetATesterPreferenceThatMatchesFrom(List<Slot> slots, List<TesterPreference> testerPreferences)
        //{
        //}

        public static IEnumerable<DateTime> DateTimesBetween(DateTime start, DateTime end)
        {
            for (var day = start.Date; day <= end; day = day.AddDays(1))
                yield return day;
        }

        public static bool HasAllPrimaryTestersFiledIn(IEnumerable<Slot> slots)
        {
            foreach (var slot in slots)
            {
                if (slot.Tester_1 == null) return false;
            }
            return true;
        }

        public static bool HasAllSecondTestersFiledIn(IEnumerable<Slot> slots)
        {
            foreach (var slot in slots)
            {
                if (slot.Tester_2 == null) return false;
            }
            return true;
        }

        public static bool SlotContainsTester(Slot slot, Tester tester)
        {
            return tester.Equals(slot.Tester_1) || tester.Equals(slot.Tester_2);
        }

        public static int HowManyTimesAssignedSoFar(IEnumerable<Slot> slots, Tester tester)
        {
            int count = 0;

            foreach (var slot in slots)
            {
                if (SlotContainsTester(slot, tester)) count++;
            }

            return count;
        }
    }
}