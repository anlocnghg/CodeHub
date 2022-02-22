using TSS.Lib.Common.Models;

using Xunit;

namespace TSS.Lib.Common.Test
{
    public class Test_Tester_Model
    {
        [Fact]
        public void Tester_Model_Equality()
        {
            Tester t1 = new Tester() { Id = 1, Name = "John Doe" };
            Tester t2 = new Tester() { Id = 1, Name = "John Doe" };
            Tester t3 = t1;

            Assert.True(t1.Equals(t2));
            Assert.True(t3.Equals(t1));
            Assert.True(t2.Equals(t3));

            Tester t4 = new Tester() { Id = 1, Name = "Loc Nguyen" };
            Tester t5 = new Tester() { Id = 2, Name = "Nguyen Loc" };
            Assert.False(t4.Equals(t5));
            Assert.False(t1.Equals(t4));
            Assert.False(t2.Equals(t5));
        }
    }
}