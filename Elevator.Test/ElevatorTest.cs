using ElevatorLib;

namespace Elevator.Test
{
    public class ElevatorTest
    {
        [Fact]
        public void InitialState()
        {
            // Arrange
            ElevatorManager elevator = new ();
            var idleState = new IdleState();
            // Act
            elevator.ChangeState(idleState);
            elevator.Update();

            // Assert
            Assert.Equal(elevator._currentState.GetType(), elevator.IdleState.GetType());
        }
    }
}