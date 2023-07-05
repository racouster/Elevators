namespace ElevatorLib
{
    public class Floor
    {
        public int Id { get; }
        public string StatusMessage { get; private set; }

        public int Occupants { get; private set; } = 0;
        public int OccupantLimit { get; private set; } = 0;

        public Floor()
        {
            StatusMessage = "Ground floor.";
        }

        public Floor(int occupants) : this()
        {
            Occupants = occupants;
            OccupantLimit = occupants;
        }

        public Floor(int occupants, int occupantLimit) : this(occupants)
        {
            OccupantLimit = occupants;
        }

        public int AddOccupants(int change)
        {
            var availableSpace = OccupantLimit - Occupants;
            int remainder = availableSpace - change;

            Occupants += availableSpace - change;

            return remainder;
        }

        public int RemoveOccupants(int change)
        {
            if (this.Occupants >= this.OccupantLimit)
            {
                this.Occupants -= change;
                return this.Occupants;
            }

            return 0;
        }

        public void SetStatusMessage(string newMessage)
        {
            this.StatusMessage = newMessage;
        }
    }
}