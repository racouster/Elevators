namespace ElevatorLib
{
    public class ClosingState : ElevatorState
    {
        public override void EnterState(ElevatorManager elevator)
        {
            elevator.SetStatusMessage($"Elevator {elevator.Id}: Door Closing.");
        }

        public override void UpdateState(ElevatorManager elevator)
        {
            elevator.SetStatusMessage($"Elevator {elevator.Id}: Door Closed.");
        }

        public override void OnFloorSelected(ElevatorManager elevator, int floor)
        {
            elevator.SetStatusMessage($"Selected floor: {floor}.");
        }
    }
}