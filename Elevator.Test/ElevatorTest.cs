using ElevatorLib;
using ElevatorLib.States;
using FluentAssertions;

namespace Elevator.Test
{
    public class ElevatorTest
    {
        [Fact]
        public void ElevatorManager_InitialState_IsIdle()
        {
            // Arrange
            ElevatorManager elevator = new(0, 10, -3, 10);
            Floor floor = new Floor(1,1,10);

            // Act
            elevator.Update(floor);

            // Assert
            Assert.Equal(elevator.CurrentState.GetType(), elevator.IdleState.GetType());
        }

        [Fact]
        public void ElevatorManager_ChangeToIdleState_DoesNothing()
        {
            // Arrange
            ElevatorManager elevator = new(0, 10, -3, 10);
            var floor = new Floor(1, 1, 10);
            var idleState = new IdleState();

            // Act
            elevator.ChangeState(idleState);
            elevator.Update(floor);

            // Assert
            Assert.Equal(elevator.CurrentState.GetType(), elevator.IdleState.GetType());
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(-2)]
        public void ChooseFloor_MovingUpOrDown_StopsOnTargetFloor(int targetFloor)
        {
            // Arrange
            var startingFloor = 0;
            ElevatorManager elevator = new(startingFloor, 10, minimumFloor: -2, maximumFloor: 2);
            Floor floor = new Floor(startingFloor, 1, 100);

            var floorDiff = Math.Abs(startingFloor - targetFloor);
            var moreUpdatesThanNeeded = floorDiff + 5;
            // Act
            elevator.ChooseFloor(targetFloor);

            for (int i = 0; i < moreUpdatesThanNeeded; i++)
            {
                elevator.Update(floor);
            }

            // Assert
            elevator.CurrentFloorNumber.Should().Be(targetFloor);
        }

        [Theory]
        [InlineData(-3)]
        [InlineData(3)]
        public void ChooseFloor_BeyondTarget_DoesNotExceedLimits(int targetFloor)
        {
            // Arrange
            var startingFloor = 0;
            var minimumFloor = -1 * Math.Abs(targetFloor) + 1;
            var maximumFloor = Math.Abs(targetFloor) - 1;
            
            ElevatorManager elevator = new(startingFloor, 10, minimumFloor, maximumFloor);
            Floor floor = new Floor(startingFloor, 1, 100);
            var floorDiff = Math.Abs(startingFloor - targetFloor);

            // Act
            elevator.ChooseFloor(targetFloor);

            for (int i = 0; i < floorDiff; i++)
            {
                elevator.Update(floor);
            }

            // Assert
            elevator.CurrentFloorNumber.Should().BeOneOf(elevator.MinimumFloor, elevator.MaximumFloor);
        }
    }
}