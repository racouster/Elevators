namespace ElevatorLib
{
    public class ErrorState : ElevatorState
    {
        public override void EnterState(ElevatorManager elevator)
        {
            Console.WriteLine($"Elevator {elevator.Id}: Is broken.");
        }
        public override void UpdateState(ElevatorManager elevator)
        {
            Console.WriteLine($"Elevator {elevator.Id}: @#$^%&$.");
        }
        public override void OnFloorSelected(ElevatorManager elevator, int floor)
        {
            Console.WriteLine($"Selected floor: {floor}.");
        }
    }
}