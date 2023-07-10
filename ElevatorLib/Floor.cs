namespace ElevatorLib
{
    public class Floor : IFloor
    {
        public int Id { get; }
        public string StatusMessage { get; private set; }

        public int Occupants { get; private set; } = 0;
        public int OccupantLimit { get; private set; } = 100;

        public int NewOccupants { get; private set; } = 0;

        public Floor()
        {
        }

        public Floor(int id, int occupants) : this()
        {
            Id = id;
            Occupants = occupants;
            OccupantLimit = 10;
        }

        public Floor(int id, int occupants, int occupantLimit) : this(id, occupants)
        {
            OccupantLimit = occupantLimit;
        }

        public int SetOccupants(int value)
        {
            // Could be count or weight
            Occupants = Math.Min(value, OccupantLimit);
            return OccupantLimit - Occupants;
        }

        public int AddOccupants(int change)
        {
            NewOccupants += Math.Abs(change);
            NewOccupants = Math.Min(NewOccupants, OccupantLimit - Occupants);
            return NewOccupants;
        }
        
        public int RemoveOccupants(int change)
        {
            Occupants = Math.Min(OccupantLimit, Occupants + NewOccupants);
            NewOccupants = 0;
            Occupants -= Math.Abs(change);
            Occupants = Math.Max(0, Occupants);
            return Occupants;
        }

        public void SetStatusMessage(string newMessage)
        {
            this.StatusMessage = newMessage;
        }
    }
}