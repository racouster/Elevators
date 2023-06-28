namespace ElevatorLib
{
    public class ErrorState : ElevatorState
    {
        public override void EnterState(ElevatorManager elevator)
        {
            elevator.SetStatusMessage($"Elevator {elevator.Id}: Is broken.");
        }
        public override void UpdateState(ElevatorManager elevator)
        {
            elevator.SetStatusMessage($"Elevator {elevator.Id}: @#$^%&$.");
        }
        public override void OnFloorSelected(ElevatorManager elevator, int floor)
        {
            elevator.SetStatusMessage($"Selected floor: {floor}.");
        }
    }
}