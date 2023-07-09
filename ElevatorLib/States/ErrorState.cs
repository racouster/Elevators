namespace ElevatorLib.States
{
    public class ErrorState : ElevatorState
    {
        public override void EnterState(ElevatorManager elevator)
        {
            elevator.SetStatusMessage($"Is broken.");
        }
        public override void UpdateState(ElevatorManager elevator)
        {
            elevator.SetStatusMessage($"@#$^%&$.");
        }

        public override void LeaveState(ElevatorManager elevator)
        {
            elevator.SetStatusMessage($"Leaving {this.GetType().Name} state...");
        }
    }
}