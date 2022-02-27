using LocLib;

namespace TSS.Lib.ViewModels
{
    // To keep track of how many assignments each tester supports a test event.
    public class TesterAssignmentDetails : ObservableObject
    {
        #region Properties

        #region Binding Properties

        #region Id

        private int _id;

        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        #endregion Id

        #region Name

        private string _name;

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        #endregion Name

        #region AssignmentCount

        private int _assignmentsCount;

        public int AssignmentsCount
        {
            get { return _assignmentsCount; }
            set { SetProperty(ref _assignmentsCount, value); }
        }

        #endregion AssignmentCount

        #endregion Binding Properties

        #endregion Properties
    }
}