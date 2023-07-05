namespace ElevatorLib.States
{
    public partial class DoorsOpenState : IdleState
    {

        public override void OnEnterState(ElevatorManager elevator)
        {
            elevator.SetStatusMessage($"Elevator {elevator.Id}: Door Opening.");

        }

        public override void UpdateState(ElevatorManager elevator)
        {
            elevator.SetStatusMessage($"Elevator {elevator.Id}: Door Open.");
        }

        public override void OnLeaveState(ElevatorManager elevator)
        {
            elevator.SetStatusMessage($"Elevator {elevator.Id}: Door will soon close.");
        }
    }
}