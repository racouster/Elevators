namespace ElevatorLib.States
{
    public class IdleState : ElevatorState
    {
        public override void EnterState(ElevatorManager elevator)
        {
            elevator.SetStatusMessage($"Started idling [{elevator.CurrentFloorNumber}]");
        }

        public override void UpdateState(ElevatorManager elevator)
        {
            elevator.SetStatusMessage($"Idle on [{elevator.CurrentFloorNumber}]");

            if (elevator.PreviousState.GetType() == typeof(MovingState))
            {
                elevator.SetStatusMessage($"Initiate open doors [{elevator.CurrentFloorNumber}]");
                elevator.ChangeState(elevator.DoorsOpenState);
            }
            else if (elevator.PreviousState.GetType() == typeof(DoorsOpenState))
            {
                elevator.SetStatusMessage($"Initiate close doors  [{elevator.CurrentFloorNumber}]");
                elevator.ChangeState(elevator.DoorsClosedState);
            }
        }

        public override bool CanProceedTo(ElevatorState targetState)
        {
            return
                targetState.GetType() == typeof(DoorsOpenState)
                || targetState.GetType() == typeof(IdleState)
                || targetState.GetType() == typeof(DoorsClosedState)
                || targetState.GetType() == typeof(MovingState);
        }

        public override void LeaveState(ElevatorManager elevator)
        {
            elevator.SetStatusMessage($"Leaving {this.GetType().Name} state...");
        }
    }
}