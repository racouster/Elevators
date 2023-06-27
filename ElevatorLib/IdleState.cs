namespace ElevatorLib
{
    public class IdleState : ElevatorState
    {
        public override void EnterState(ElevatorManager elevator)
        {
            Console.WriteLine($"Elevator {elevator.Id}: Is now idle.");
        }

        public override void UpdateState(ElevatorManager elevator)
        {
            Console.WriteLine($"{elevator.Id}: {elevator.CurrentFloor} zzz.");
        }

        public override void OnFloorSelected(ElevatorManager elevator, int targetFloor)
        {
            elevator.ChooseFloor(targetFloor);
            elevator.ChangeState(elevator.MovingState);
        }
    }
}