using ElevatorLib;
using FluentAssertions;

namespace Elevator.Test
{
    public class ElevatorSystemTest
    {
        [Fact]
        public void ElevatorSystemManager_Start_EntersRunningState()
        {
            // Arrange
            ElevatorSystemManager elevatorSystem = new(1);
            
            // Act
            elevatorSystem.Start();
            
            // Assert
            Assert.True(elevatorSystem.Elevators.Count == 1);
        }

        [Fact]
        public void ElevatorSystemManager_Floors_IsSet()
        {
            // Arrange
            ElevatorSystemManager elevatorSystem = new(1, 1, 10);

            // Act
            elevatorSystem.Start();

            // Assert
            Assert.True(elevatorSystem.Floors.Count == 1);
        }

        [Fact]
        public void ElevatorSystemManager_WithElevators_HasCorrectNumber()
        {
            // Arrange
            // Act
            ElevatorSystemManager elevatorSystem = new(5);

            // Assert
            elevatorSystem.ElevatorCount.Should().Be(5);
        }


        [Theory]
        [InlineData(-1)]
        public void ChooseFloor_MovingUpOrDown_StopsOnTargetFloor(int targetFloor)
        {
            // Arrange
            ElevatorSystemManager elevatorSystem = new(1);
            elevatorSystem.Start();

            // Act
            elevatorSystem.RequestElevator(targetFloor);
            elevatorSystem.Update(TimeSpan.Zero);

            // Assert
            elevatorSystem.Elevators[0].CurrentFloor.Should().Be(targetFloor);
        }

        
    }
}