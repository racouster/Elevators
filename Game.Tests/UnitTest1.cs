using FluentAssertions;

namespace GameSystem.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            var a = new Game("SimuvatorTest", 1);
            await a.StartAsync();
            a.CallElevator(1);
            a.ElevatorCount.Should().Be(1);
        }
    }
}