﻿using System;

namespace S10270243_PRG2Assignment
{
    // ✅ Implement IComparable<Flight>
    public abstract class Flight : IComparable<Flight>
    {
        public string FlightNumber { get; }
        public string Origin { get; }
        public string Destination { get; }
        public DateTime ExpectedTime { get; }
        public string Status { get; set; } // Now mutable as per UML

        protected Flight(string flightNumber, string origin, string destination, DateTime expectedTime, string status)
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expectedTime;
            Status = status;
        }

        public virtual double CalculateFees()
        {
            double fee = 300; // Base boarding gate fee

            // Arrival / Departure Fees
            if (Destination == "SIN") fee += 500; // Arriving flight fee
            if (Origin == "SIN") fee += 800; // Departing flight fee

            // Special Request Fees
            if (this is CFFTFlight) fee += 150;
            if (this is DDJBFlight) fee += 300;
            if (this is LWTTFlight) fee += 500;

            // Apply Flight-Based Discounts
            if (ExpectedTime.Hour < 11 || ExpectedTime.Hour >= 21)
                fee -= 110; // Early/Late Discount

            if (Origin == "DXB" || Origin == "BKK" || Origin == "NRT")
                fee -= 25; // Specific Origin Discount

            if (!(this is CFFTFlight || this is DDJBFlight || this is LWTTFlight))
                fee -= 50; // No Special Request Discount

            // Ensure Fee is Non-Negative
            return Math.Max(fee, 0);
        }

        // ✅ Implement CompareTo() for sorting
        public int CompareTo(Flight other)
        {
            if (other == null) return 1;
            return this.ExpectedTime.CompareTo(other.ExpectedTime);
        }

        public override string ToString()
        {
            return string.Format("{0,-12} {1,-20} {2,-20} {3,-20} {4,-12} {5,-12}",
                FlightNumber, Origin, Destination,
                ExpectedTime.ToString("d/M/yyyy"), ExpectedTime.ToString("h:mm tt"), Status);
        }
    }
}
