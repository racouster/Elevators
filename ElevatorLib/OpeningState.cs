namespace ElevatorLib
{
    public class OpeningState : ElevatorState
    {
        public override void EnterState(ElevatorManager elevator)
        {
            Console.WriteLine($"Elevator {elevator.Id}: Door Opened.");
        }

        public override void UpdateState(ElevatorManager elevator)
        {
            Console.WriteLine($"Elevator {elevator.Id}: Open.");
        }

        public override void OnFloorSelected(ElevatorManager elevator, int floor)
        {
            Console.WriteLine($"Selected floor: {floor}.");
        }
    }
}