using System;
using System.IO;
using Xunit;
using ThingsToRemember.Services;
using Xunit.Abstractions;

namespace ThingsToTest
{
    public class Units : IClassFixture<TestsFixture>
    {
        private readonly ITestOutputHelper _testOutputHelper;
        public static    MockDataStore     Database => new();
        public Units(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Test1()
        {
            
        }
    }

    public class TestsFixture : IDisposable
    {
        public TestsFixture()
        {
            //Initialize things here
        }

        public void Dispose()
        {
            
            //Teardown things here
        }
    }
}
