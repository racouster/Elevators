namespace ElevatorLib.States
{
    public class DoorsClosedState : ElevatorState
    {
        public override void OnEnterState(ElevatorManager elevator)
        {
            elevator.SetStatusMessage($"Elevator {elevator.Id}: Door Closing.");
        }

        public override void UpdateState(ElevatorManager elevator)
        {
            elevator.SetStatusMessage($"Elevator {elevator.Id}: Door Closed.");

            if (elevator.CurrentFloor != elevator.TargetFloor)
                elevator.ChangeState(elevator.MovingState);
            else
                elevator.ChangeState(elevator.IdleState);
        }

        public override void OnLeaveState(ElevatorManager elevator)
        {
            elevator.SetStatusMessage($"{elevator.Id}: Leaving {this.GetType().Name} state...");
        }
    }
}