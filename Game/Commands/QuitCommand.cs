namespace GameSystem.Commands;

class QuitCommand : ICommand
{
    private readonly IGame Game;

    public QuitCommand(IGame game)
    {
        Game = game;
    }

    public void Execute()
    {
        Game.Quit();
    }

    public void Revert()
    {
        // TODO: Doesn't make sense to add this to a queue...
        throw new InvalidOperationException("Cannot revert a quit command.");
    }
}
