using System.Diagnostics;
using System.Runtime.CompilerServices;
using ElevatorLib.States;

[assembly: InternalsVisibleTo("Elevator.Test")]
namespace ElevatorLib
{
    public partial class ElevatorSystemManager : IElevatorSystemManager
    {
        // TODO: Use concurrent collections or state persistence layer.
        public List<IElevator> Elevators { get; private set; }
        public List<IFloor> Floors { get; private set; }
        public int ElevatorCount { get; private set; }

        // TODO: Replace with a timed background service?
        // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-7.0&tabs=visual-studio#timed-background-tasks
        private static readonly System.Timers.Timer timer = new System.Timers.Timer();
        private DateTime PreviousTime = DateTime.Now;
        public int WaitTime { get; }
        private event EventHandler<CustomEventArgs> OnUpdate = new EventHandler<CustomEventArgs>((object? sender, CustomEventArgs e) => { });

        public int MinimumFloor { get; private set; }
        public int MaximumFloor { get; private set; }
        public int OccupantLimit { get; private set; }

        public ElevatorSystemManager(int elevatorCount, int minFloor, int maxFloor, int occupantLimit = 10, int waitTime = 1000)
        {
            WaitTime = waitTime;

            MinimumFloor = minFloor;
            MaximumFloor = maxFloor;

            ElevatorCount = elevatorCount;

            Elevators = Enumerable.Range(0, ElevatorCount)
                .Select(_ => new ElevatorManager(0, occupantLimit, minFloor, maxFloor) as IElevator)
                .ToList();

            Floors = Enumerable.Range(MinimumFloor, MaximumFloor - MinimumFloor + 1)
                .Select((floorNum, i) => new Floor(floorNum, Math.Abs(i - 3), 20))
                .ToList<IFloor>();
        }

        public void Start()
        {
            timer.Interval = WaitTime;
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

        public int CallElevator(int floor)
        {
            var idleElevator = Elevators
                    .Where(e => typeof(IdleState).IsAssignableFrom(e.CurrentState.GetType()))
                    //TODO: Check horizontal distance too
                    // Get the closest elevator (vertical distance), which button called it?
                    .OrderBy(e => Math.Abs(e.CurrentFloorNumber - floor))
                    .FirstOrDefault();
            
            int previousFloor = idleElevator?.CurrentFloorNumber ?? 0;

            // TODO: If no idle elevators, get the closest elevator that is moving in the same direction
            if (idleElevator is not null)
            {
                idleElevator.ChooseFloor(floor);
            }

            //TODO: Add queue for elevators that are busy
            return previousFloor;
        }

        public IEnumerable<IElevator> GetElevatorSystemState()
        {
            var elevatorSnapshot = new List<IElevator>();
            return Elevators;
        }

        public IEnumerable<IFloor> GetFloors()
        {
            return Floors;
        }

        public int AddElevatorOccupants(IElevator elevator, int count)
        {
            return elevator.RemoveOccupants(count);
        }

        public int RemoveElevatorOccupants(IElevator elevator, int count)
        {
            return elevator.RemoveOccupants(count);
        }

        public int SetFloorOccupants(int floorNumber, int count)
        {
            return Floors?.Find(f => f.Id == floorNumber)?.SetOccupants(count) ?? 0;
        }

        public int AddFloorOccupants(int floorNumber, int count)
        {
            return Floors?.Find(f => f.Id == floorNumber-MinimumFloor)?.AddOccupants(count) ?? 0;
        }

        public int RemoveFloorOccupants(int floorNumber, int count)
        {
            return Floors?.Find(f => f.Id == floorNumber)?.RemoveOccupants(count) ?? 0;
        }

        public void AddElevator()
        {
            Elevators.Add(new ElevatorManager(0, 10, MinimumFloor, MaximumFloor));
        }

        public void RemoveElevator()
        {
            Elevators = Elevators.Take(Elevators.Count() - 1).ToList();
        }

        public IEnumerable<IElevator> Update(TimeSpan timeDelta)
        {
            // TODO: Yagni but Will be used in draw to determine how much to move the elevator
            double gameTimeElapsed = timeDelta.TotalMilliseconds / 1000;

            foreach (var elevator in Elevators)
            {
                // TODO: Elevator movement speed.
                var floor = Floors.Find(f => f.Id == elevator.TargetFloor);
                elevator.Update(floor);
            }

            return Elevators;
        }

        private void AdvanceState(Object? sender, System.Timers.ElapsedEventArgs e)
        {
            var elapsedTime = e.SignalTime - PreviousTime;
            PreviousTime = e.SignalTime;

            if (elapsedTime.TotalMilliseconds > 0)
            {
                var elevators = Update(elapsedTime);
                OnUpdate(this, new CustomEventArgs(elevators, elapsedTime));
            }
        }

        public void RegisterUpdateCallback(EventHandler<CustomEventArgs> callback)
        {
            OnUpdate += callback;
        }
    }
}
