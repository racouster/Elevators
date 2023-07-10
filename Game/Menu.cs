internal class Menu : IRenderable
{
    private int SelectedIndex = 0;
    private string Prompt;
    private Dictionary<string, Action> Options;
    public int Top { get; } = 0;
    public int Left { get; } = 0;
    public int Height { get; private set; } = 0;

    public Menu(
        int top,
        int left,
        string prompt,
        Dictionary<string, Action> options)
    {
        Prompt = prompt;
        Options = options;
        Top = top;
        Left = left;
    }

    public void Render(ref char[] screenBuffer, int width)
    {
        // TODO: min screen max
        var maxOptionLength = Math.Max(Options?.Max(x => x.Key.Length) ?? 0, Prompt.Length);
        Array.Copy(Prompt.ToCharArray(), 0, screenBuffer, Top * width + Left, Prompt.Length);
        Height = Options.Count + 1;

        var i = 0;
        foreach(var(option, action) in Options)
        {
            var optArray = option.ToCharArray();
            var offset = (1 + Top + i) * width + Left;
            Array.Copy(optArray, 0, screenBuffer, offset, optArray.Length);
            i++;
        }
    }
}
