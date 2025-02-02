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
    internal class Flight
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

        public double CalculateFees()
        {
            double fee = 300; // Base fee
            if (Destination == "SIN") fee += 500; // Arrival fee
            if (Origin == "SIN") fee += 800; // Departure fee

            // Apply Special Request Fees
            if (this is CFFTFlight) fee += 150;
            if (this is DDJBFlight) fee += 300;
            if (this is LWTTFlight) fee += 500;

            // Apply Discounts
            if (ExpectedTime.Hour < 11 || ExpectedTime.Hour >= 21) fee -= 110; // Early/Late Discount
            if (Origin == "DXB" || Origin == "BKK" || Origin == "NRT") fee -= 25; // Specific Airport Discount
            if (!(this is CFFTFlight || this is DDJBFlight || this is LWTTFlight)) fee -= 50; // No Special Request Discount

            return fee > 0 ? fee : 0; // Ensure fee is not negative
        }

        public override string ToString()
        {
            return string.Format("{0,-12} {1,-20} {2,-20} {3,-20} {4,-12} {5,-12}",
                FlightNumber, Origin, Destination,
                ExpectedTime.ToString("d/M/yyyy"), ExpectedTime.ToString("h:mm tt"), Status);
        }
    }
}