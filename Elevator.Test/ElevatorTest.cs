using ElevatorLib;
using FluentAssertions;

namespace Elevator.Test
{
    public class ElevatorTest
    {
        [Fact]
        public void InitialState()
        {
            // Arrange
            ElevatorManager elevator = new();

            // Act
            elevator.Update();

            // Assert
            Assert.Equal(elevator._currentState.GetType(), elevator.IdleState.GetType());
        }

        [Fact]
        public void InitialStateToIdleState()
        {
            // Arrange
            ElevatorManager elevator = new();
            var idleState = new IdleState();

            // Act
            elevator.ChangeState(idleState);
            elevator.Update();

            // Assert
            Assert.Equal(elevator._currentState.GetType(), elevator.IdleState.GetType());
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(-2)]
        public void MovementShouldEndOnCorrectFloor(int targetFloor)
        {
            // Arrange
            var startingFloor = 0;
            ElevatorManager elevator = new(startingFloor, minimumFloor: -2, maximumFloor: 2);

            var floorDiff = Math.Abs(startingFloor - targetFloor);

            // Act
            elevator.ChooseFloor(targetFloor);

            for (int i = 0; i < floorDiff; i++)
            {
                elevator.Update();
            }

            // Assert
            elevator.CurrentFloor.Should().Be(targetFloor);
        }

        [Theory]
        [InlineData(-3)]
        [InlineData(3)]
        public void MovementShouldNotExceedLimits(int targetFloor)
        {
            // Arrange
            var startingFloor = 0;
            var minimumFloor = -1 * Math.Abs(targetFloor) + 1;
            var maximumFloor = Math.Abs(targetFloor) - 1;
            ElevatorManager elevator = new(startingFloor, minimumFloor, maximumFloor);
            var floorDiff = Math.Abs(startingFloor - targetFloor);

            // Act
            elevator.ChooseFloor(targetFloor);

            for (int i = 0; i < floorDiff; i++)
            {
                elevator.Update();
            }

            // Assert
            elevator.CurrentFloor.Should().BeOneOf(elevator.MinimumFloor, elevator.MaximumFloor);
        }
    }
}