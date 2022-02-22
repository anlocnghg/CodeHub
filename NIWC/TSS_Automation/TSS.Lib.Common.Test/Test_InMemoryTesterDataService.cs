/*
 * Shared Context between Tests via Constructor and Dispose
 * Ref: https://xunit.net/docs/shared-context
 *
 */

using LocLib.IData;

using System;
using System.Linq;
using System.Threading.Tasks;

using TSS.Lib.Common.DataServices;
using TSS.Lib.Common.Models;

using Xunit;

namespace TSS.Lib.Common.Test
{
    public class Test_InMemoryTesterDataService : IDisposable
    {
        #region Setup Context

        private IDataCRUD<Tester> _testerDataService;

        public Test_InMemoryTesterDataService() => _testerDataService = new InMemoryTesterDataService();

        public void Dispose() => _testerDataService = null;

        #endregion Setup Context

        [Fact]
        public async Task GetAllAsListAsync_Test()
        {
            var allTesters = await _testerDataService.GetAllAsListAsync();
            Assert.Equal(6, allTesters.Count);
            Assert.True(allTesters.Exists(t => t.Name == "Loc Nguyen" && !t.IsPrimary & t.IsGovee & !t.IsTestTeam));
            Assert.False(allTesters.Exists(t => t.Name == "John Doe"));

            Assert.Contains(1, allTesters.Select(t => t.Id));
            Assert.Contains(2, allTesters.Select(t => t.Id));
            Assert.Contains(3, allTesters.Select(t => t.Id));
            Assert.Contains(4, allTesters.Select(t => t.Id));
        }

        [Fact]
        public async Task DeleteAsync_Test()
        {
            var testers = await _testerDataService.GetAllAsListAsync();
            Assert.Equal(6, testers.Count);

            //Tester testerToDelete = testers.SingleOrDefault(t => t.Id == 1); // Note static Id.

            Tester testerToDelete = testers.FirstOrDefault(t => t.Name.Contains("Nguyen"));
            var idDeleted = await _testerDataService.DeleteAsync(testerToDelete);

            Assert.DoesNotContain(testerToDelete, await _testerDataService.GetAllAsListAsync());
            Assert.Equal(5, testers.Count);

            //await _testerDataService.DeleteAsync(await _testerDataService.GetAsync(2));
            //Assert.Equal(4, testers.Count);
        }
    }
}