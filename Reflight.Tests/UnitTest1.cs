using System.IO;
using Reflight.Core;
using Xunit;

namespace Reflight.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            using (var stream = File.OpenRead("Test.json"))
            {
                var data = FlightDataReader.Read(stream);            
            }
        }
    }
}
