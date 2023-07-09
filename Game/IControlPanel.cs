internal interface IControlPanel
{
    void ExecuteCommands();
    void SetCommand(ICommand command);
}