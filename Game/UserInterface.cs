using GameSystem.Commands;
using System.Text;

namespace GameSystem;

internal class UserInterface
{
    private Menu Menu;
    private string? UserInput = "0";
    private List<string> Messages = new() { };
    private int Width;
    private int Height;
    private int BufferLength;
    private short BufferIndex = 0;
    private char[][] Buffers = new char[2][];
    private int SelectedFloor = 0;
    private int margin = 4;
    private IControlPanel ControlPanel;
    private IGame Game;

    public UserInterface(
        IGame game,
        string title
        )
    {
        ControlPanel = new ControlPanel();
        Game = game;

        Console.Title = title;
        Console.OutputEncoding = Encoding.UTF8;
        //Console.CursorVisible = false;

        // TODO: Can we detect window size changes?
        Width = Console.WindowWidth;
        Height = Console.WindowHeight;
        BufferLength = Width * Height;

        InititializeBuffer();

        Menu = CreateMenu(margin, (int)(0.625*Width));
    }

    public void Run()
    {
        // Look for keyboard input
        var input = GetInput();
        HandleInput(input);

        //Switch between buffers every cycle
        bool useBuffer0 = ++BufferIndex % 2 == 0;
        
        if (BufferIndex % 10 == 0)
        {
            ControlPanel.ExecuteCommands();
        }

        // Render buffer we don't draw on.
        #pragma warning disable 4014
        Task.Run(() =>
        {
            Render(Buffers[useBuffer0 ? 1 : 0]);
        }).ConfigureAwait(false);
        #pragma warning restore 4014

        // TODO: screen and player
        Paint(Buffers[useBuffer0 ? 0 : 1]);
    }

    private Menu CreateMenu(int top, int left)
    {
        return new Menu(
            top: top,
            left: left,
            prompt: "Controls:",
            options: new Dictionary<string, Action> {
                {"[⬆,⬇] Select Higher Floor", ()=>{} },
                {"[Space] Call Elevator.", ()=>{} },
                {"[0..9,Enter,Backspace] Set occupant count.", ()=>{} },
                {"[PgUp, PgDown] Inc. Dec. game speed.", ()=>{} },
                {"[Esc] Quit.", () => {} },
            });
    }

    private void InititializeBuffer()
    {
        // Make 2 display buffers
        Buffers = new char[2][] {
                Enumerable.Repeat(' ', BufferLength).ToArray(),
                Enumerable.Repeat(' ', BufferLength).ToArray()
            };

        // Draw the border once, no need to re-render
        DrawBorder(ref Buffers[0]);

        // Copy buffer 0 to buffer 1
        Buffers[1] = Buffers[0].ToArray();
    }

    // Collects key input updates final user input value
    // and assigns an action if required
    EventType? GetInput()
    {
        // TODO: Debounce
        if (Console.KeyAvailable)
        {
            var key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    return EventType.UpFloor;
                case ConsoleKey.DownArrow:
                    return EventType.DownFloor;

                case ConsoleKey.Escape:
                    return EventType.Quit;

                case ConsoleKey.Enter:
                    return EventType.SetOccupants;

                case ConsoleKey.Spacebar:
                    return EventType.CallElevator;

                case ConsoleKey.Add:
                    return EventType.AddElevator;
                case ConsoleKey.Subtract:
                    return EventType.RemoveElevator;

                case ConsoleKey.PageUp:
                    return EventType.IncreaseSpeed;
                case ConsoleKey.PageDown:
                    return EventType.DecreaseSpeed;
                case ConsoleKey.Backspace:
                    UserInput = string.IsNullOrEmpty(UserInput) ? "" : UserInput?[..^1];
                    break;
                default:
                    {
                        UserInput = $"{UserInput}{key.KeyChar}";
                        break;
                    }
            }
        }

        return null;
    }

    // Decides which action to take based on user input
    private void HandleInput(EventType? input)
    {
        switch (input)
        {
            case EventType.SetOccupants:
                if (int.TryParse(UserInput, out int count))
                {
                    ControlPanel.SetCommand(
                        new SetOccupantsCommand(Game, SelectedFloor, count));
                    Messages.Add($"Floor {SelectedFloor} has occupants: {count}");
                } else
                {
                    Messages.Add($"'{UserInput}' isn't a valid number.");
                }
                break;

            case EventType.CallElevator:
                Messages.Add($"Moving to floor {SelectedFloor}");
                ControlPanel.SetCommand(new CallElevatorCommand(
                    Game, 
                    SelectedFloor));
                break;

            case EventType.UpFloor:
                SelectedFloor = Math.Clamp(SelectedFloor + 1, Game.MinimumFloor, Game.MaximumFloor);
                var floorPop = Game.GetPopulation(SelectedFloor);
                Messages.Add($"Selected floor: {SelectedFloor}, population {floorPop}");
                break;
            case EventType.DownFloor:
                SelectedFloor = Math.Clamp(SelectedFloor - 1, Game.MinimumFloor, Game.MaximumFloor);
                var floorPopD = Game.GetPopulation(SelectedFloor);
                Messages.Add($"Selected floor: {SelectedFloor}, population {floorPopD}");
                break;

            case EventType.IncreaseSpeed:
                ControlPanel.SetCommand(
                        new ChangeGameSpeedCommand(Game, true));
                Messages.Add($"Increasing game speed.");
                break;
            case EventType.DecreaseSpeed:
                ControlPanel.SetCommand(
                        new ChangeGameSpeedCommand(Game, false));
                Messages.Add($"Decreasing game speed.");
                break;

            case EventType.Quit:
                ControlPanel.SetCommand(
                        new QuitCommand(Game));
                Messages.Add("Exiting...");
                break;
            default:
                break;
        }
    }

    void Paint(char[] screenBuffer)
    {
        int buildingTop = 3;
        int buildingLeft = 3;
        int gutter = 3;

        DrawBorder(ref screenBuffer); 
        DrawFloors(ref screenBuffer, Game.MinimumFloor, Game.MaximumFloor, Game.ElevatorCount, buildingTop, buildingLeft, gutter);
        DrawElevators(ref screenBuffer, Game.GetElevators(), buildingTop, buildingLeft, gutter);
        DrawPeople(ref screenBuffer, Game.GetFloors(), Game.MinimumFloor, Game.MaximumFloor, Game.ElevatorCount, buildingTop, buildingLeft, gutter);
        DrawMainMenu(screenBuffer, Menu);
        var menuBottom = margin + Menu.Height;
        DrawSelectedFloor(ref screenBuffer, SelectedFloor, menuBottom, (int)(0.625 * Width), gutter);
        DrawUserInput(ref screenBuffer, UserInput, menuBottom, (int)(0.625 * Width), gutter);
        

        DrawMessageBox(Messages.ToArray(), ref screenBuffer);
    }
    async Task Render(char[] screenBuffer)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.CursorTop = 0;
        Console.CursorLeft = 0;
        await Console.Out.WriteAsync(screenBuffer);
    }

    private void DrawUserInput(ref char[] screenBuffer, string? userInput, int top, int left, int gutter)
    {
        var annoyingCaret = DateTime.Now.Ticks % 2 == 0 ? "_" : " ";
        var populationInput = $"Change population to: {userInput, 4}{annoyingCaret}".ToCharArray();
        var offset = (top +1) * Width + left;
        Array.Copy(populationInput, 0, screenBuffer, offset, populationInput.Length);
    }

    private void DrawSelectedFloor(ref char[] screenBuffer, int selectedFloor, int top, int left, int gutter)
    {
        var floorNumber = $"Selected Floor: {selectedFloor, 10}".ToCharArray();
        var offset = (top + 2) * Width + left;
        Array.Copy(floorNumber, 0, screenBuffer, offset, floorNumber.Length);
    }

    private void DrawMainMenu(char[] screenBuffer, IRenderable menu)
    {
        menu.Render(ref screenBuffer, Width);
    }

    private void DrawFloors(ref char[] screenBuffer, int minimumFloor, int maximumFloor, int elevatorCount, int top = 1, int left = 1, int gutter = 3)
    {
        var buildingWidth = elevatorCount * gutter + 2;

        for (int floorNum = maximumFloor; floorNum >= minimumFloor; floorNum--)
        {
            string floorPostfix = floorNum.ToString();
            string fix = new string('_', buildingWidth - floorPostfix.Length);
            string prefix2 = (floorNum == SelectedFloor ? ">" : " ");
            var floor = (prefix2 + fix + floorPostfix).ToCharArray();

            Array.Copy(floor, 0, screenBuffer, (top + maximumFloor - floorNum + margin) * Width + left, floor.Length);
        }
    }

    private void DrawPeople(
        ref char[] screenBuffer,
        List<IFloor> floors,
        int minimumFloor,
        int maximumFloor,
        int elevatorCount,
        int top = 1,
        int left = 1,
        int gutter = 3
        )
    {
        var title = "Population".ToCharArray();
        Array.Copy(title, 0, screenBuffer, (top + maximumFloor - maximumFloor-1 + margin) * Width + left + elevatorCount * 3 + 5, title.Length);

        for (int floorNum = maximumFloor; floorNum >= minimumFloor; floorNum--)
        {
            var floor = floors.FirstOrDefault(f => f.Id == floorNum);
            var floorOccupantCount = floor.Occupants;
            var persons = $"{new string('¥', floorOccupantCount),11}".ToCharArray();

            Array.Copy(persons, 0, screenBuffer, (top + maximumFloor - floorNum + margin) * Width + left + elevatorCount*3 +5, persons.Length);
        }
    }

    private void DrawBorder(ref char[] screenBuffer)
    {
        char borderChar = '+';

        // Draw a border around the screen
        var line = new String(borderChar, Width).ToCharArray();
        line.CopyTo(screenBuffer, 0);

        for (int i = 1; i < Height; i++)
        {
            // Left
            screenBuffer[i * Width] = borderChar;
            // Right
            screenBuffer[i * Width + Width - 1] = borderChar;
        }

        line.CopyTo(screenBuffer, BufferLength - Width);

        char conrnerChar = '*';
        screenBuffer[0] = conrnerChar;
        screenBuffer[Width - 1] = conrnerChar;
        screenBuffer[Height * Width - Width] = conrnerChar;
        screenBuffer[Height * Width - 1] = conrnerChar;
    }

    void DrawElevators(ref char[] screenBuffer, IEnumerable<IElevator> elevators, int left = 1, int top = 1, int gutter = 2)
    {
        foreach (var (i, elevator) in elevators.Select((e, i) => (i, e)))
        {
            //Each elevator could have its own max and min
            var floorCount = elevator.MaximumFloor - elevator.MinimumFloor;

            // TODO: Change symbol when elevator contains occupants.
            //'⚀', 0x2680, 0x2681, 0x2682, 0x2683, 0x2684, 0x2685 
            //var symbol = 0x2680 + (int)Math.Floor(elevator.Occupants / 6.0);
            //var utf8Char  = Encoding.UTF8.GetChars(BitConverter.GetBytes(symbol));
            //screenBuffer[(floorCount - elevator.CurrentFloor + top + 1) * Width + left + 2 + (i * gutter)] = utf8Chars..;

            // Write elevator population count
            var elevatorPopulation = $"{elevator.Occupants,3}".ToCharArray();
            Array.Copy(elevatorPopulation, 0, screenBuffer, (top + 1) * Width + left + 2 + (i * gutter), elevatorPopulation.Length);

            var elevatorSymbol = elevator.TargetFloor > elevator.CurrentFloorNumber ? '▲' 
                : elevator.TargetFloor < elevator.CurrentFloorNumber ? '▼' 
                    : elevator.Occupants > 0 ? '#' : '█';
            screenBuffer[(floorCount - elevator.CurrentFloorNumber + top + 1) * Width + left + 2 + (i * gutter)] = elevatorSymbol;
        }
    }

    void DrawMessageBox(string[] messages, ref char[] screenBuffer, int messageDrawCount = 4)
    {
        if (messages == null || messages.Length == 0 || screenBuffer.Length < BufferLength)
        {
            return;
        }

        var boxTop = Height - 6;
        var boxLeft = 5;
        var boxWidth = messages.Max(m => m.Length) + 2;
        var messages2 = messages.TakeLast(messageDrawCount).Reverse().ToArray();

        for (int i = 0; i < messages2.Length; i++)
        {
            var topOffset = boxTop * Width - 1;
            var msgOffset = i * Width - 1;
            var targetIndex = topOffset + msgOffset + boxLeft;
            Array.Copy(messages2[i].PadRight(boxWidth, ' ').ToCharArray(), 0, screenBuffer, targetIndex, boxWidth);
        }
    }
}