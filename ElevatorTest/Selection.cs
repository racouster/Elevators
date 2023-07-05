internal class Selection
{
    public ActionType @Action = Selection.ActionType.DoNothing;
    public string Value = "";

    public Selection()
    {
    }

    public Selection(ActionType action, string inputValue)
    {
        @Action = action;
        Value = inputValue;
    }

    public enum ActionType
    {
        DoNothing = 0,
        SubmitInput,
        UpdateInput,
        ChooseFloor,
        SetOccupants,
        IncreaseSpeed,
        DecreaseSpeed,
        Quit,
    }
}