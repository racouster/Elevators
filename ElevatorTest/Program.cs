using ElevatorLib;
using System.Collections.Immutable;
using System.Text;

var sleepTimes = new List<int>() { 256, 128, 64, 32, 16, 8, 4, 2, 1 };
var selectedSleepTime = 0;
var sleepTime = sleepTimes[selectedSleepTime];

var messages = new List<string>(10) { "" };
var currentInput = "";
var keepRunning = false;
var _previousGameTime = DateTime.Now;


Console.SetWindowSize(80, 30);
Console.SetBufferSize(80, 30);
Console.CursorVisible = false;

var _width = Console.WindowWidth;
var _height = Console.WindowHeight;

var _bufferLength = _width * _height;
var _buffer = new char[2][] { 
    Enumerable.Repeat(' ', _bufferLength).ToArray(), 
    Enumerable.Repeat(' ', _bufferLength).ToArray() };
int _bufferIndex = 0;

var line = new String('.', _width).ToCharArray();
line.CopyTo(_buffer[0], 0);
line.CopyTo(_buffer[1], 0);

for (int i = 1; i < _height-1; i++)
{
    _buffer[0][i * _width - 1] = '.';
    _buffer[1][i * _width - 1] = '.';

    _buffer[0][i * _width] = '.';
    _buffer[1][i * _width] = '.';
}
line.CopyTo(_buffer[0], _bufferLength-_width);
line.CopyTo(_buffer[1], _bufferLength-_width);

Console.Title = "Elevator";


Console.WriteLine("How many elevators would you like to simulate?");
var requestedElevatorCount = Console.ReadLine();

Console.Clear();

ElevatorSystemManager elevatorSystem;

if (int.TryParse(requestedElevatorCount, out int elevatorCount))
{
    keepRunning = true;
    messages.Add($"Starting {elevatorCount} elevators...");
    elevatorSystem = new ElevatorSystemManager(elevatorCount);
}
else
{
    messages.Add($"Starting 1 elevator...");
    elevatorSystem = new ElevatorSystemManager(1);
}

//Console.WriteLine("Input keys: `ESC` - Quit, `1 .. 9` - Choose a floor, `+` - Cycle processign speed");

elevatorSystem.Start();

// Main loop
while (keepRunning)
{
    // Borrowed from https://www.codementor.io/@dewetvanthomas/tutorial-game-loop-for-c-128ovxgrig
    // Calculate the time elapsed since the last game loop cycle
    TimeSpan GameTime = DateTime.Now - _previousGameTime;
    // Update the current previous game time
    _previousGameTime += GameTime;

    // Update Game at 3.90625 fps
    await Task.Delay(sleepTime);

    elevatorSystem.Update(GameTime);

    var input = GetInput();

    if (input?.Action == Selection.ActionType.Quit)
    {
        Console.WriteLine("Exiting...");
        elevatorSystem.Stop();
        keepRunning = false;
    }

    if (input?.Action == Selection.ActionType.ChooseFloor)
    {
        messages.Add("Please choose a floor:");
        //messages.Add("How many occupants?");
    }

    if (input?.Action == Selection.ActionType.SubmitInput)
    {
        if (int.TryParse(currentInput, out int requestedFloor))
        {
            elevatorSystem.RequestElevator(requestedFloor);
            messages.Add($"Requested floor {requestedFloor}");
        }
        currentInput = null;
        input = null;
    }

    if (input?.Action == Selection.ActionType.IncreaseSpeed)
    {
        if (selectedSleepTime >= sleepTimes.Count - 1)
        {
            selectedSleepTime = 0;
        }
        else
        {
            selectedSleepTime++;
        }

        sleepTime = sleepTimes[selectedSleepTime];
    }

    if (input?.Action == Selection.ActionType.DecreaseSpeed)
    {
        if (selectedSleepTime <= sleepTimes.Count - 1)
        {
            selectedSleepTime = 0;
        }
        else
        {
            selectedSleepTime--;
        }

        sleepTime = sleepTimes[selectedSleepTime];
    }

    if (input?.Action == Selection.ActionType.UpdateInput)
    {
        currentInput = input.Value;
    }


    bool useBuffer0 = ++_bufferIndex % 2 == 0;
    // TODO: Draw map, elevators, screen and player
    Paint(_buffer[useBuffer0 ? 0 : 1]);
}

// TODO: Create Visual representation of the elevator system
void Paint(char[] screenBuffer)
{
    var currentBuffer = DrawElevators(elevatorSystem.Elevators, screenBuffer);
    
    currentBuffer[0] = '*';
    currentBuffer[_width - 1] = '*';
    currentBuffer[_height * _width - 1] = '*';
    currentBuffer[_height * _width - _width] = '*';

    screenBuffer = DrawMessageBox(messages.ToArray(), currentBuffer, _width, _height);

    Console.ForegroundColor = ConsoleColor.Blue;
    Console.CursorTop = 0;
    Console.CursorLeft = 0;
    Console.Out.Write(screenBuffer);
}

char[] DrawElevators(ImmutableList<ElevatorManager> elevators, char[] template)
{
    var newBuffer = template.ToArray();
    const short margin = 2;

    foreach (var (i, elevator) in elevators.Select((e, i) => (i, e)))
    {
        var floorCount = elevator.MaximumFloor - elevator.MinimumFloor;
        newBuffer[margin+(floorCount - elevator.CurrentFloor) * _width + (i * 2)] = '█';
    }

    return newBuffer;
}

char[] DrawMessageBox(string[] messages, char[] template, int width, int height)
{
    if (messages == null || messages.Length == 0 || template.Length < width * height)
    {
        return template;
    }
    var newBuffer = template.ToArray();

    var boxTop = _height-6;
    var boxLeft = 5;

    var messages2 = messages.Reverse().Take(4).ToArray();

    for (int i = 0; i < messages2.Length; i++)
    {
        var topOffset = boxTop * _width-1;
        var msgOffset = i * _width-1;
        var targetIndex = topOffset + msgOffset + boxLeft;
        Array.Copy(messages2[i].ToCharArray(), 0, newBuffer, targetIndex, messages2[i].Length);
    }

    return newBuffer;
}

Selection? GetInput()
{
    if (Console.KeyAvailable)
    {
        var key = Console.ReadKey(true);

        if (key.Key == ConsoleKey.Enter)
        {
            return new Selection(Selection.ActionType.SubmitInput, currentInput);
        }

        if (key.Key == ConsoleKey.Escape)
        {
            return new Selection(Selection.ActionType.Quit, null);
        }

        if (key.Key == ConsoleKey.Backspace)
        {
            return new Selection(Selection.ActionType.UpdateInput, currentInput[..-1]);
        }

        if (key.Key == ConsoleKey.F)
        {
            return new Selection(Selection.ActionType.SetOccupants, "");
        }

        return new Selection(Selection.ActionType.UpdateInput, currentInput + key.KeyChar);
    }

    return null;
}