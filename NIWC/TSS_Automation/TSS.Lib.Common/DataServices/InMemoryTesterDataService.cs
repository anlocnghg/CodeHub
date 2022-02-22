using LocLib.IData;

using TSS.Lib.Common.Models;

namespace TSS.Lib.Common.DataServices
{
    public class InMemoryTesterDataService : IDataCRUD<Tester>
    {
        private List<Tester> _testers;
        private static int id = 1;

        public InMemoryTesterDataService()
        {
            //int id = 1;
            _testers = new List<Tester>
            {
                new Tester {Id = id++, Name = "Erwin Taganas", IsPrimary = true, IsTestTeam = true, IsGovee = true},
                new Tester {Id = id++, Name = "Paul Liwanag", IsPrimary = true, IsTestTeam = true, IsGovee = false},
                new Tester {Id = id++, Name = "Rosa Lisa Silipino", IsPrimary = true, IsTestTeam = true, IsGovee = false},
                new Tester {Id = id++, Name = "Megan Moore", IsPrimary = true, IsTestTeam = false, IsGovee = true},
                new Tester {Id = id++, Name = "Bianca Makdisi", IsPrimary = true, IsTestTeam = false, IsGovee = true},
                new Tester {Id = id++, Name = "Loc Nguyen", IsPrimary = false, IsTestTeam = false, IsGovee = true},
            };
        }

        public async Task<List<Tester>> GetAllAsListAsync(string filter = null)
            => await Task.FromResult(_testers);

        public async Task<IEnumerable<Tester>> GetAllAsIEnumerableAsync(string filter = null)
            => await Task.FromResult(_testers);

        public async Task<int> AddAsync(Tester newTester)
            => await Task.Run(() =>
            {
                newTester.Id = id++;
                _testers.Add(newTester);
                return newTester.Id;
            });

        public async Task<Tester> GetAsync(int id)
            => await Task.Run(() => _testers.Single(t => t.Id == id));

        public async Task<int> UpdateAsync(Tester updatedTester)
        {
            return await Task.Run(() =>
            {
                Tester tester = _testers.Single(t => t.Id == updatedTester.Id);

                {
                    tester.Name = updatedTester.Name;
                    tester.IsPrimary = updatedTester.IsPrimary;
                    tester.IsTestTeam = updatedTester.IsTestTeam;
                    tester.IsGovee = updatedTester.IsGovee;
                }
                return tester.Id;
            });
        }

        public async Task<int> DeleteAsync(Tester testerToDelete)
        {
            return await Task.Run(() =>
            {
                Tester tester = _testers.SingleOrDefault(t => t.Id == testerToDelete.Id);
                _testers.Remove(tester);
                return tester.Id;
            });
        }

        public async Task<int> DeleteAllAsync()
        {
            return await Task.Run(() =>
            {
                int total = _testers.Count;
                _testers.Clear();
                return total;
            });
        }
    }
}