using ElevatorLib;
using System.Reflection;

namespace ElevatorApi.Services
{
    public class CustomEventArgs : EventArgs
    {
        public CustomEventArgs(IEnumerable<IElevator> elevators)
        {
            Elevators = elevators;
        }

        public IEnumerable<IElevator> Elevators { get; set; }
    }

    public class ElevatorService : IElevatorService
    {
        private DateTime _previousTime = DateTime.Now;
        private static readonly System.Timers.Timer timer = new System.Timers.Timer();
        private EventHandler<CustomEventArgs> _updateCallbacks = new EventHandler<CustomEventArgs>((object? sender, CustomEventArgs e) => { });

    public ElevatorService()
        {
            _elevatorSystem = new ElevatorSystemManager(3);
            _elevatorSystem.Start();
            timer.Interval = 1000;
            timer.Elapsed += AdvanceState;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        public ElevatorSystemManager _elevatorSystem { get; }

        public void RequestElevator(int floor)
        {
            _elevatorSystem.RequestElevator(floor);
        }

        public IEnumerable<IElevator> GetElevatorSystemState()
        {
            return _elevatorSystem.GetElevatorSystemState();
        }

        public IEnumerable<IElevator> RegisterUpdateCallback(Func<object, bool> callback)
        {

            return _elevatorSystem.GetElevatorSystemState();
        }

        private void AdvanceState(Object source, System.Timers.ElapsedEventArgs e)
        {
            var elapsedTime = e.SignalTime - _previousTime;
            _previousTime = e.SignalTime;
            if (elapsedTime.TotalMilliseconds > 0)
            {
                _elevatorSystem.Update(elapsedTime);
                var elebators = _elevatorSystem.GetElevatorSystemState();
                _updateCallbacks(this, new CustomEventArgs(elebators));
            }
        }

        public void RegisterUpdateCallback(EventHandler<CustomEventArgs> callback)
        {
            _updateCallbacks += callback;
        }
    }
}
