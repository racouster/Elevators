namespace ElevatorLib.States
{
    public class OpeningState : ElevatorState
    {
        public override void EnterState(ElevatorManager elevator)
        {
            elevator.SetStatusMessage($"Elevator {elevator.Id}: Door Opening.");
        }

        public override void UpdateState(ElevatorManager elevator)
        {
            elevator.SetStatusMessage($"Elevator {elevator.Id}: Door Open.");
        }

        public override void OnFloorSelected(ElevatorManager elevator, int floor)
        {
            elevator.SetStatusMessage($"Selected floor: {floor}.");
        }
    }
}