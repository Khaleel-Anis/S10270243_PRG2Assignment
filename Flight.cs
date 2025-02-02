//==========================================================
// Student Number : S10270243
// Student Number : S10269256
// Student Name : Khaleel Anis
// Partner Name : Hia Wei Dai
//==========================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG_2_Assignment
{
    internal abstract class Flight
    {
        public string FlightNumber { get; }
        public string Origin { get; }
        public string Destination { get; }
        public DateTime ExpectedTime { get; }
        public string Status { get; }

        protected Flight(string flightNumber, string origin, string destination, DateTime expectedTime, string status)
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expectedTime;
            Status = status;
        }

        public abstract double CalculateFees();

        public override string ToString()
        {
            return string.Format("{0,-12} {1,-20} {2,-20} {3,-20} {4,-12}",
                FlightNumber, Origin, Destination,
                ExpectedTime.ToString("d/M/yyyy"), ExpectedTime.ToString("h:mm tt"));
        }
    }
}