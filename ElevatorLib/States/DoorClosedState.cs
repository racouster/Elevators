namespace ElevatorLib.States
{
    public class DoorsClosedState : ElevatorState
    {
        public override void EnterState(ElevatorManager elevator)
        {
            // Load occupants
            elevator.SetStatusMessage($"Loading occupants: {elevator.CurrentFloor.Occupants}");

            var startingFloorOccupants = elevator.CurrentFloor.Occupants;
            // People enter elevator
            var elevatorOccupantCount = elevator.AddOccupants(startingFloorOccupants);
            var floorOccupantCount = elevator.CurrentFloor.RemoveOccupants(startingFloorOccupants);

            elevator.SetStatusMessage($"Floor occupants: {floorOccupantCount}");


            elevator.SetStatusMessage($"Door Closing.");
        }

        public override void UpdateState(ElevatorManager elevator)
        {
            if (elevator.PreviousState.GetType() == typeof(IdleState))
            {
                elevator.ChangeState(elevator.IdleState);
            }

            elevator.SetStatusMessage($"Door Closed.");
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