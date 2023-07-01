namespace ElevatorLib.States
{
    public class IdleState : ElevatorState
    {
        public override void EnterState(ElevatorManager elevator)
        {
            elevator.SetStatusMessage($"Elevator {elevator.Id}: Is now idle on floor: {elevator.CurrentFloor}.");
        }

        public override void UpdateState(ElevatorManager elevator)
        {
            elevator.SetStatusMessage($"{elevator.Id}: Current Floor: {elevator.CurrentFloor}.");
        }

        public override void OnFloorSelected(ElevatorManager elevator, int targetFloor)
        {
            elevator.SetStatusMessage($"Selected floor: {targetFloor}.");
            // TODO: Add in-transit floor selection, add to queue
            elevator.ChooseFloor(targetFloor);
            elevator.ChangeState(elevator.MovingState);
        }
    }
}