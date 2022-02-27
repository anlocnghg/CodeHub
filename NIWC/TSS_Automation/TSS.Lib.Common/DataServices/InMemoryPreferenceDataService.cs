using LocLib.IData;

using TSS.Lib.Common.Models;

namespace TSS.Lib.Common.DataServices
{
    public class InMemoryPreferenceDataService : IDataCRUD<TesterPreference>
    {
        private readonly IDataCRUD<Tester> _testerDataService;
        private List<TesterPreference> _testerPreferences;

        public InMemoryPreferenceDataService(IDataCRUD<Tester> testerDataService)
        {
            _testerDataService = testerDataService;
            _testerPreferences = new List<TesterPreference>();

            Task.Run(async () =>
            {
                var allTesters = await _testerDataService.GetAllAsIEnumerableAsync();
                foreach (var tester in allTesters)
                {
                    TesterPreference testerPreference = new TesterPreference(tester);

                    _testerPreferences.Add(testerPreference);
                }
            });
        }

        public async Task<List<TesterPreference>> GetAllAsListAsync(string filter = null)
        {
            #region In case an existing Tester was deleted or a new one was added.

            var allTesters = await _testerDataService.GetAllAsIEnumerableAsync();

            #region In case a Tester was deleted.

            List<TesterPreference> toBeRemoved = new();
            foreach (var testerPreference in _testerPreferences)
            {
                if (!allTesters.Contains(testerPreference.Tester))
                {
                    toBeRemoved.Add(testerPreference);
                }
            }
            _testerPreferences = _testerPreferences.Except(toBeRemoved).ToList();

            #endregion In case a Tester was deleted.

            #region In case a new Tester was added.

            List<TesterPreference> toBeAdded = new();
            foreach (var tester in allTesters)
            {
                if (!_testerPreferences.Any(p => p.Tester == tester))
                {
                    toBeAdded.Add(new TesterPreference(tester));
                }
            }
            _testerPreferences.AddRange(toBeAdded);

            #endregion In case a new Tester was added.

            #endregion In case an existing Tester was deleted or a new one was added.

            return await Task.FromResult(_testerPreferences);
        }

        public async Task<IEnumerable<TesterPreference>> GetAllAsIEnumerableAsync(string filter = null) => await GetAllAsListAsync();

        public Task<int> UpdateAsync(TesterPreference testerPreference)
        {
            TesterPreference toBeUpdated = _testerPreferences.Single(p => p.Tester == testerPreference.Tester);

            {
                toBeUpdated.MaxAssignmentsCount = testerPreference.MaxAssignmentsCount;
                toBeUpdated.PreferredDays = testerPreference.PreferredDays;
                toBeUpdated.PreferredShifts = testerPreference.PreferredShifts;
                toBeUpdated.CoTesterId = testerPreference.CoTesterId;
                toBeUpdated.CoTesterName = testerPreference.CoTesterName;
                toBeUpdated.NotAvailableDates = testerPreference.NotAvailableDates;
            }

            return Task.FromResult(testerPreference.Tester.Id);
        }

        #region No Need

        public Task<int> DeleteAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TesterPreference> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddAsync(TesterPreference entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(TesterPreference entity)
        {
            throw new NotImplementedException();
        }

        #endregion No Need
    }
}