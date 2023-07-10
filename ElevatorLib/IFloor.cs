namespace ElevatorLib
{
    public interface IFloor
    {
        int Id { get; }
        int OccupantLimit { get; }
        int Occupants { get; }
        string StatusMessage { get; }

        int SetOccupants(int change);
        int AddOccupants(int change);
        int RemoveOccupants(int change);
        void SetStatusMessage(string newMessage);
    }
}