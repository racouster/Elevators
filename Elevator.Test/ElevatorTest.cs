using ElevatorLib;

namespace Elevator.Test
{
    public class ElevatorTest
    {
        [Fact]
        public void InitialState()
        {
            // Arrange
            ElevatorManager elevator = new ElevatorManager();
            
            // Act
            elevator.ChangeState(elevator.IdleState);

            // Assert
            Assert.Equal(elevator.IdleState, );
        }
    }
}