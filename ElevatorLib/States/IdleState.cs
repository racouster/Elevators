namespace ElevatorLib.States
{
    public class IdleState : ElevatorState
    {
        public override void OnEnterState(ElevatorManager elevator)
        {
            elevator.SetStatusMessage($"Elevator {elevator.Id}: Is about to idle on floor: {elevator.CurrentFloor}.");
            if(elevator._previousState == elevator.MovingState)
            {
                elevator.ChangeState(elevator.DoorsOpenState);
            }
        }

        public override void UpdateState(ElevatorManager elevator)
        {
            elevator.SetStatusMessage($"{elevator.Id}: Idle on Floor: {elevator.CurrentFloor}.");
        }

        public override bool CanProceedTo(ElevatorManager elevator)
        {
            return elevator._currentState.GetType() == elevator.IdleState.GetType()
                || elevator._currentState.GetType() == elevator.DoorsClosedState.GetType();
        }

        public override void OnLeaveState(ElevatorManager elevator)
        {
            elevator.SetStatusMessage($"{elevator.Id}: Leaving {this.GetType().Name} state...");
        }
    }
}