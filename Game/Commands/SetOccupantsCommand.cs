namespace GameSystem.Commands;

class SetOccupantsCommand : ICommand
{
    private readonly IGame Game;
    private readonly int TargetFloor;
    private int OccupantCount;
    private int Remainder;

    public SetOccupantsCommand(IGame game, int targetFloor, int occupantCount)
    {
        Game = game;
        TargetFloor = targetFloor;
        OccupantCount = occupantCount;
        Remainder = occupantCount;
    }

    public void Execute()
    {
        Remainder = Game.SetFloorOccupants(TargetFloor, OccupantCount);
    }

    public void Revert()
    {
        // This will not work, Add returns a remainder, not idempotent
        Game.RemoveFloorOccupants(TargetFloor, OccupantCount - Remainder);
    }
}
