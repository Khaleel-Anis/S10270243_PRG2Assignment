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
    internal class Airline
    {
        private string name;
        private string code;
        private Dictionary<string, Flight> flights;

        public string Name { get { return name; } }
        public string Code { get { return code; } }
        public Dictionary<string, Flight> Flights { get { return flights; } }

        public Airline(string code, string name)
        {
            this.code = code;
            this.name = name;
            flights = new Dictionary<string, Flight>();
        }

        public bool AddFlight(Flight flight)
        {
            if (!flights.ContainsKey(flight.FlightNumber))
            {
                flights.Add(flight.FlightNumber, flight);
                return true;
            }
            return false;
        }

        public bool RemoveFlight(Flight flight)
        {
            return flights.Remove(flight.FlightNumber);
        }

        public double CalculateFees()
        {
            double totalFees = 0;
            int arrivingFlights = 0;
            int departingFlights = 0;

            foreach (var flight in flights.Values)
            {
                totalFees += flight.CalculateFees();

                if (flight.Destination == "SIN") arrivingFlights++;
                if (flight.Origin == "SIN") departingFlights++;
            }

            // Apply Bulk Discount ($350 per 3 flights)
            int totalFlights = arrivingFlights + departingFlights;
            totalFees -= (totalFlights / 3) * 350;

            // Apply 3% Discount if Airline has 5+ Flights
            if (totalFlights >= 5)
                totalFees *= 0.97;

            // Ensure Total Fee is Non-Negative
            return Math.Max(totalFees, 0);
        }


        public override string ToString()
        {
            string flightDetails = "\n=============================================";
            flightDetails += "\nList of Flights for " + name;
            flightDetails += "\n=============================================";
            flightDetails += string.Format("\n{0,-12} {1,-20} {2,-20} {3,-20} {4,-25} {5,-15}", "Flight No.", "Origin", "Destination", "Expected Time", "Status", "Gate");

            foreach (var flight in flights.Values)
            {
                flightDetails += "\n" + flight.ToString();
            }

            return flightDetails;
        }
    }
}