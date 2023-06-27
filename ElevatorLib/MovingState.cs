namespace ElevatorLib
{
    public class MovingState : ElevatorState
    {

        public override void EnterState(ElevatorManager elevator)
        {
            Console.WriteLine($"Elevator {elevator.Id}: Moving...");
        }

        public override void UpdateState(ElevatorManager elevator)
        {
            if (elevator.CurrentFloor == elevator.TargetFloor)
            {
                elevator.ChangeState(elevator.IdleState);
            }
            else
            {
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
            Console.WriteLine($"Selected floor: {floor}.");
        }
    }
}