
internal class ControlPanel : IControlPanel
{
    private readonly Stack<ICommand> Commands = new Stack<ICommand>();
    public void SetCommand(ICommand command)
    {
        Commands.Push(command);
    }

    public void ExecuteCommands()
    {
        while(Commands.Count > 0)
            Commands.Pop().Execute();
    }
}
