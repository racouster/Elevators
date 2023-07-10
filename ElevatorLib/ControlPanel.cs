
internal class ControlPanel
{
    private readonly Stack<ICommand> Commands = new Stack<ICommand>();
    public void SetCommand(ICommand command)
    {
        Commands.Append(command);
    }

    public void ExecuteCommands()
    {
        foreach(var command in Commands)
            command.Execute();
    }
}
