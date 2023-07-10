namespace ElevatorLib.States
{
    public partial class DoorsOpenState : ElevatorState
    {

        public override void EnterState(ElevatorManager elevator)
        {
            elevator.SetStatusMessage($"Door Opening.");
        }

        public override void UpdateState(ElevatorManager elevator)
        {
            elevator.SetStatusMessage($"Door Open.");

            if (elevator.PreviousState.GetType() == typeof(IdleState))
            {
                elevator.ChangeState(elevator.IdleState);
            }
        }

        public override void LeaveState(ElevatorManager elevator)
        {
            // Load occupants
            elevator.SetStatusMessage($"Unloading occupants: {elevator.Occupants}");

            // People enter floor
            var newFloorCount  = elevator.CurrentFloor.AddOccupants(elevator.Occupants);
            elevator.SetStatusMessage($"Floor population: {newFloorCount}, {elevator.Occupants} remain in elevator");
            // People leave elevator
            var stillInElevator = elevator.RemoveOccupants(elevator.Occupants);
            elevator.SetStatusMessage($"Elevator lost: {elevator.Occupants}, {stillInElevator} remain in elevator");

            elevator.SetStatusMessage($"Door will soon close.");
        }

        public override bool CanProceedTo(ElevatorState targetState)
        {
            return targetState.GetType() == typeof(IdleState);
        }
    }
}