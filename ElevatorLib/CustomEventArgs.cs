namespace ElevatorLib
{
    public class CustomEventArgs : EventArgs
    {
        public CustomEventArgs(IEnumerable<IElevator> elevators)
        {
            Elevators = elevators;
        }

        public IEnumerable<IElevator> Elevators { get; set; }
    }
}
