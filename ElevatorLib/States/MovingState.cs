namespace ElevatorLib.States
{
    public class MovingState : ElevatorState
    {

        public override void EnterState(ElevatorManager elevator)
        {
            elevator.SetStatusMessage($"Started Moving to floor {elevator.TargetFloor}");
        }

        public override void UpdateState(ElevatorManager elevator)
        {
            if (elevator.CurrentFloorNumber == elevator.TargetFloor)
            {
                elevator.SetStatusMessage($"Arrived at floor {elevator.CurrentFloorNumber}.");
                elevator.ChangeState(elevator.IdleState);
            }
            else
            {
                elevator.SetStatusMessage($"Moving from {elevator.CurrentFloorNumber} to floor {elevator.TargetFloor}.");
                if (elevator.CurrentFloorNumber > elevator.TargetFloor)
                {
                    elevator.MoveDown();
                }
                else
                {
                    elevator.MoveUp();
                }
            }
        }

        public override void LeaveState(ElevatorManager elevator)
        {
            elevator.SetStatusMessage($"Leaving {this.GetType().Name} state...");
        }

        public override bool CanProceedTo(ElevatorState targetState)
        {
            return targetState.GetType() == typeof(IdleState);
        }
    }
}