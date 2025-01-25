using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG_2_Assignment
{
    internal class Flight
    {
        public string FlightNumber { get; }
        public Airline Airline { get; }
        public string Origin { get; }
        public string Destination { get; }
        public DateTime ExpectedTime { get; }
        public string Status { get; protected set; }

        protected Flight(string flightNumber, Airline airline, string origin, string destination, DateTime expectedTime)
        {
            FlightNumber = flightNumber;
            Airline = airline;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expectedTime;
            Status = "Scheduled";
        }

        public abstract double CalculateFees();
    }
}
