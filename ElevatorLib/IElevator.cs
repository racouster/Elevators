﻿namespace ElevatorLib
{
    public interface IElevator
    {
        public int CurrentFloor { get; set; }
        public string StatusMessage { get; set; }
    }
}