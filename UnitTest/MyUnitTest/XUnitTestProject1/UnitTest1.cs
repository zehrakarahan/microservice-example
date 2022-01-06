using MyUnitTest;
using NSubstitute;
using System;
using Xunit;

namespace XUnitTestProject1
{
    public class UnitTest1: ContainerTest
    {
       

        [Fact]
        public void TestMethod1()
        {
            var _calculator = Instance<ICalculatorService>();
            var result = _calculator.Sum(2, 3);
            Assert.Equal<int>(0, result);
        }

        [Fact]
        public void SetUp()
        {
            ObjectFactory.Initialize(
                x =>
                {
                    x.For<IValidator>().Use<Validator>();
                    x.For<IRepository>().Use<Repository>()
                        .Ctor<string>().Is(TestProfile.SqlServerDB)
                        .WithProperty("UpdateBy")
                        .EqualTo(TestProfile.UpdateByAdmin); ;
                });
        }


    }
}
