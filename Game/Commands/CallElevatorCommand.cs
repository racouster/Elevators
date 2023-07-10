namespace GameSystem.Commands;

class CallElevatorCommand : ICommand
{
    private readonly IGame Game;
    private readonly int TargetFloor;
    private int PreviousFloor;

    public CallElevatorCommand(IGame elevatorSystem, int targetFloor)
    {
        Game = elevatorSystem;
        TargetFloor = targetFloor;
    }

    public void Execute()
    {
        PreviousFloor = Game.CallElevator(TargetFloor);
    }

    public void Revert()
    {
        Game.CallElevator(PreviousFloor);
    }
}
