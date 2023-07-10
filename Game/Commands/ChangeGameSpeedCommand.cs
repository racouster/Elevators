namespace GameSystem.Commands;

class ChangeGameSpeedCommand : ICommand
{
    private readonly IGame Game;
    private bool ShouldIncrease = true;

    public ChangeGameSpeedCommand(IGame game, bool shouldIncrease)
    {
        Game = game;
        ShouldIncrease = shouldIncrease;
    }

    public void Execute()
    {
        if (ShouldIncrease)
            Game.IncreaseGameSpeed();
        else
            Game.DecreaseGameSpeed();
    }

    public void Revert()
    {
        if (ShouldIncrease)
            Game.DecreaseGameSpeed();
        else
            Game.IncreaseGameSpeed();
    }
}
