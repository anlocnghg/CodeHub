
using Xunit;

namespace TSS.Lib.Common.Test
{
    public class Test_Templates
    {
        #region Templates

        #region Facts

        [Fact]
        public void PassingTest()
        {
            Assert.Equal(4, Add(2, 2));
        }

        private int Add(int x, int y)
        {
            return x + y;
        }

        #endregion Facts

        #region Theory

        [Theory]
        [InlineData(3)]
        [InlineData(5)]
        public void MyFirstTheory(int value)
        {
            Assert.True(IsOdd(value));
        }

        private bool IsOdd(int value)
        {
            return value % 2 == 1;
        }

        #endregion Theory

        #endregion Templates
    }
}
