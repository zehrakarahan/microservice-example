using MyUnitTest;
using Xunit;

namespace UnitTestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void TestMethod1()
        {
            Calculator calculator = new Calculator();
            var result = calculator.Sum(2,3);
            Assert.Equal<int>(5,result);
        }
    }
}
