namespace ElevatorLib
{
    public class CustomEventArgs : EventArgs
    {
        public CustomEventArgs(IEnumerable<IElevator> elevators, TimeSpan elapsedTime)
        {
            ElapsedTime = elapsedTime;
            Elevators = elevators;
        }

        public IEnumerable<IElevator> Elevators { get; set; }
        public TimeSpan ElapsedTime { get; set; }
    }
}
