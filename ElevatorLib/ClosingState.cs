namespace ElevatorLib
{
    public class ClosingState : ElevatorState
    {
        public override void EnterState(ElevatorManager elevator)
        {
            Console.WriteLine($"Elevator {elevator.Id}: Door Closed.");
        }

        public override void UpdateState(ElevatorManager elevator)
        {
            Console.WriteLine($"Elevator {elevator.Id}: Closed.");
        }

        public override void OnFloorSelected(ElevatorManager elevator, int floor)
        {
            Console.WriteLine($"Selected floor: {floor}.");
        }
    }
}