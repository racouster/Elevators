using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using ElevatorLib.States;

[assembly: InternalsVisibleTo("Elevator.Test")]
namespace ElevatorLib
{
    public partial class ElevatorSystemManager : IDisposable
    {
        public ImmutableList<ElevatorManager> Elevators { get; private set; }
        public ImmutableList<Floor> Floors { get; private set; }
        public int ElevatorCount { get; private set; }

        private static readonly System.Timers.Timer timer = new System.Timers.Timer();
        private DateTime _previousTime = DateTime.Now;
        private event EventHandler<CustomEventArgs> OnUpdate = new EventHandler<CustomEventArgs>((object? sender, CustomEventArgs e) => { });

        private int MinimumFloor { get; } = -3;
        private int MaximumFloor { get; } = 10;

        public ElevatorSystemManager(int elevatorCount)
        {
            ElevatorCount = elevatorCount;

            Elevators = Enumerable.Range(0, ElevatorCount)
                .Select<int, ElevatorManager>(_ => new ElevatorManager())
                .ToImmutableList();

            Floors = Enumerable.Range(MinimumFloor, MaximumFloor - MinimumFloor + 1)
                .Select<int, Floor>(f => new Floor(f, 0))
                .ToImmutableList();
        }
        public ElevatorSystemManager(int elevatorCount, int minFloor, int maxFloor)
        {
            MinimumFloor = minFloor;
            MaximumFloor = maxFloor;

            ElevatorCount = elevatorCount;

            Elevators = Enumerable.Range(0, ElevatorCount)
                .Select<int, ElevatorManager>(_ => new ElevatorManager(0,minFloor, maxFloor))
                .ToImmutableList();

            Floors = Enumerable.Range(minFloor, maxFloor - minFloor + 1)
                .Select<int, Floor>(f => new Floor(f, 0))
                .ToImmutableList();
        }

        public void Start()
        {
            timer.Interval = 1000;
            timer.Elapsed += AdvanceState;
            timer.AutoReset = true;
            // Start the timer
            timer.Enabled = true;
        }

        public void Stop()
        {
            timer.Enabled = false;
            timer.Elapsed -= AdvanceState;
        }

        public void RequestElevator(int floor)
        {
            var idleElevator = Elevators
                    .Where(e => typeof(IdleState).IsAssignableFrom(e._currentState.GetType()))
                    // Get the closest elevator
                    .OrderBy(e => Math.Abs(e.CurrentFloor - floor))
                    .FirstOrDefault();

            // TODO: If no idle elevators, get the closest elevator that is moving in the same direction

            if (idleElevator is not null)
            {
                idleElevator.ChooseFloor(floor);
            }

            //TODO: Add queue for elevators that are busy
        }

        public IEnumerable<IElevator> GetElevatorSystemState()
        {
            var elevatorSnapshot = new List<IElevator>();
            return Elevators.Select(e => new Elevator() { CurrentFloor = e.CurrentFloor, StatusMessage = e.StatusMessage });
        }

        public IEnumerable<Floor> GetFloors()
        {
            return this.Floors;
        }

        public void Update(TimeSpan timeDelta)
        {
            // TODO: Will be used in draw to determine how much to move the elevator
            double gameTimeElapsed = timeDelta.TotalMilliseconds / 1000;

            foreach (var elevator in Elevators)
            {
                elevator.Update();
            }
        }

        private void AdvanceState(Object? sender, System.Timers.ElapsedEventArgs e)
        {
            var elapsedTime = e.SignalTime - _previousTime;
            _previousTime = e.SignalTime;

            if (elapsedTime.TotalMilliseconds > 0)
            {
                Update(elapsedTime);
                var elevators = GetElevatorSystemState();
                OnUpdate(this, new CustomEventArgs(elevators));
            }
        }

        public void RegisterUpdateCallback(EventHandler<CustomEventArgs> callback)
        {
            OnUpdate += callback;
        }

        public void Dispose()
        {
            Stop();
        }

        ~ElevatorSystemManager()
        {
            Stop();
        }
    }
}
