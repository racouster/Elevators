using ElevatorLib;
using ElevatorLib.States;
using FluentAssertions;

namespace Elevator.Test
{
    public class MenuTest
    {
        [Fact]
        public void ElevatorManager_InitialState_IsIdle()
        {
            // Arrange
            //IMenu elevator = new();

            // Act
            //elevator.Update();

            //// Assert
            //Assert.Equal(elevator._currentState.GetType(), elevator.IdleState.GetType());
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
            ElevatorManager elevator = new(startingFloor, minimumFloor: -2, maximumFloor: 2);

            var floorDiff = Math.Abs(startingFloor - targetFloor);
            var moreUpdatesThanNeeded = floorDiff + 5;
            
            // Act
            elevator.ChooseFloor(targetFloor);

            for (int i = 0; i < moreUpdatesThanNeeded; i++)
            {
                var floor = new Floor(targetFloor, 0, 10);
                elevator.Update(floor);
            }

            // Assert
            elevator.CurrentFloorNumber.Should().Be(targetFloor);
        }
    }
}