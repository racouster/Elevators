namespace ElevatorLib.States
{
    public class ErrorState : ElevatorState
    {
        public override void OnEnterState(ElevatorManager elevator)
        {
            elevator.SetStatusMessage($"Elevator {elevator.Id}: Is broken.");
        }
        public override void UpdateState(ElevatorManager elevator)
        {
            elevator.SetStatusMessage($"Elevator {elevator.Id}: @#$^%&$.");
        }

        public override void OnLeaveState(ElevatorManager elevator)
        {
            elevator.SetStatusMessage($"{elevator.Id}: Leaving {this.GetType().Name} state...");
        }
    }
}