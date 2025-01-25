using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG_2_Assignment
{
    internal class Airline
    {
        public string Name { get; }
        public string Code { get; }
        public Dictionary<string, Flight> Flights { get; private set; }

        public Airline(string code, string name)
        {
            Code = code;
            Name = name;
            Flights = new Dictionary<string, Flight>();
        }

        public bool AddFlight(Flight flight)
        {
            if (!Flights.ContainsKey(flight.FlightNumber))
            {
                Flights.Add(flight.FlightNumber, flight);
                return true;
            }
            return false;
        }

        public bool RemoveFlight(Flight flight)
        {
            return Flights.Remove(flight.FlightNumber);
        }

        public double CalculateFees()
        {
            double total = 0;
            foreach (var flight in Flights.Values)
            {
                total += flight.CalculateFees();
            }
            return total;
        }
    }
}
