﻿using System;

namespace MPK.Connect.Model.Business
{
    public class StopTimeCore
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public TimeSpan DepartureTime { get; set; }
    }
}