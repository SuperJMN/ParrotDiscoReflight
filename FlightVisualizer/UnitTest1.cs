using System;
using System.IO;
using FlightVisualizer.Core;
using Xunit;

namespace FlightVisualizer
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
