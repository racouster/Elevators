namespace ElevatorLib.States
{
    public class MovingState : ElevatorState
    {

        public override void EnterState(ElevatorManager elevator)
        {
            elevator.SetStatusMessage($"Elevator {elevator.Id}: Started Moving to floor {elevator.TargetFloor}");
        }

        public override void UpdateState(ElevatorManager elevator)
        {
            if (elevator.CurrentFloor == elevator.TargetFloor)
            {
                elevator.SetStatusMessage($"Elevator {elevator.Id}: Arrived at floor {elevator.CurrentFloor}.");
                elevator.ChangeState(elevator.IdleState);
            }
            else
            {
                elevator.SetStatusMessage($"Elevator {elevator.Id}:Moving from {elevator.CurrentFloor} to floor {elevator.TargetFloor}.");
                if (elevator.CurrentFloor > elevator.TargetFloor)
                {
                    elevator.MoveDown();
                }
                else
                {
                    elevator.MoveUp();
                }
            }
        }

        public override void OnFloorSelected(ElevatorManager elevator, int floor)
        {
            elevator.SetStatusMessage($"Elevator {elevator.Id}: Selected floor: {floor}.");
        }
    }
}