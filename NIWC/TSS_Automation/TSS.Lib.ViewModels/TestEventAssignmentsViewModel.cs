using LocLib;
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
    public partial class TestEventAssignmentsViewModel : ViewModelBase
    {
        private IDataCRUD<Tester> _testerDataService;

        public TestEventAssignmentsViewModel(ICommonServices commonServices, IDataCRUD<Tester> dataService) : base(commonServices)
        {
            _testerDataService = dataService;

            MessagingService.Subscribe<TestEventViewModel, Tuple<string, List<Slot>>>(this, MessagingTokens.TestEventAssignmentRequested, async (sender, payload) =>
            {
                EventName = payload.Item1;
                Slots = payload.Item2;

                await GenerateAssignmentsAsync();

                MessagingService.Unsubscribe(this, MessagingTokens.TestEventAssignmentRequested);
            });

        }

        #region Properties

        #region EventName

        private string _eventName;

        public string EventName
        {
            get => _eventName;
            set => SetProperty(ref _eventName, value);
        }

        #endregion EventName
        // This property is to keep track of how many assignments that each tester is assigned to support so far.
        public ObservableCollection<TesterAssignmentDetails> TesterAssignments { get; private set; } = new();

        public List<Slot> Slots { get; set; } // Get from parameter

        private List<Tester> Testers { get; set; } // Retrieve from data provider/service

        #endregion Properties

        #region Methods

        public async Task GenerateAssignmentsAsync()
        {
            foreach (var slot in Slots)
            {
                slot.Tester_1 = null;
                slot.Tester_2 = null;
            }

            TesterAssignments.Clear();

            Testers = await _testerDataService.GetAllAsListAsync();

            foreach (var tester in Testers)
            {
                TesterAssignments.Add(new TesterAssignmentDetails { Id = tester.Id, Name = tester.Name, AssignmentCount = 0 });
            }

            await FillInSlotsAsync();
        }

        #region Demo to show the progress only

        private async Task FillInSlotsAsync()
        {
            ShellViewModel.Current.Status = "Working in progress... please be patient";
            ShellViewModel.Current.SetProgress(true);

            await FillInFirstTestersAsync();
            await FillInSecondTesterAsync();

            ShellViewModel.Current.Status = string.Empty;
            ShellViewModel.Current.SetProgress(false, 0);
        }

        private async Task FillInSecondTesterAsync()
        {
            List<Tester> allTesters = Testers;
            while (!TSSHelper.HasAllSecondTestersFiledIn(Slots))
            {
                foreach (var slot in Slots)
                {
                    Tester tester_2 = allTesters.GetRandomElementUsingSeedGuid();
                    if (slot.Tester_1 == null)
                    {
                        throw new Exception("Tester #1 has not been assigned");
                    }
                    else
                    {
                        if (slot.Tester_1.Equals(tester_2))
                        {
                            continue;
                        }
                        else
                        {
                            if (slot.Tester_2 == null)
                            {
                                slot.Tester_2 = tester_2;
                                await Task.Delay(500);
                            }
                        }
                    }

                    #region Update Assignment Count

                    foreach (var testerAssignmentDetails in TesterAssignments)
                    {
                        if (testerAssignmentDetails.Id == tester_2.Id)
                        {
                            testerAssignmentDetails.AssignmentCount++;
                        }
                    }

                    #endregion Update Assignment Count
                }
            }
        }

        private async Task FillInFirstTestersAsync()
        {
            // First Tester must be Primary one
            List<Tester> primaryTesters = Testers.Where(t => t.IsPrimary).ToList();

            int averageAssignmentCounts = Slots.Count / primaryTesters.Count;


            while (!TSSHelper.HasAllPrimaryTestersFiledIn(Slots))
            {
                foreach (var slot in Slots)
                {
                    // To do: count how many times this primary tester has been assigned so far
                    // To do: prioritize the assignments to Test-Team members

                    Tester tester_1 = primaryTesters.GetRandomElementUsingSeedGuid();
                    slot.Tester_1 = tester_1;
                    await Task.Delay(500);

                    #region Update Assignment Count

                    foreach (var testerAssignmentDetails in TesterAssignments)
                    {
                        if (testerAssignmentDetails.Id == tester_1.Id)
                        {
                            testerAssignmentDetails.AssignmentCount++;
                        }
                    }

                    #endregion Update Assignment Count
                }
            }
        }

        #endregion Demo to show the progress only

        #endregion Methods
    }
}
