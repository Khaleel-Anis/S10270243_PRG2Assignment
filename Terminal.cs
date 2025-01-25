using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG_2_Assignment
{
    internal class Terminal
    {
        public string TerminalName { get; }
        public Dictionary<string, Airline> Airlines { get; private set; }
        public Dictionary<string, Flight> Flights { get; private set; }
        public Dictionary<string, BoardingGate> BoardingGates { get; private set; }
        public Dictionary<string, double> GateFees { get; private set; }

        public Terminal(string terminalName)
        {
            TerminalName = terminalName;
            Airlines = new Dictionary<string, Airline>();
            Flights = new Dictionary<string, Flight>();
            BoardingGates = new Dictionary<string, BoardingGate>();
            GateFees = new Dictionary<string, double>();
        }

        public bool AddAirline(Airline airline)
        {
            if (!Airlines.ContainsKey(airline.Code))
            {
                Airlines.Add(airline.Code, airline);
                return true;
            }
            return false;
        }

        public bool AddBoardingGate(BoardingGate gate)
        {
            if (!BoardingGates.ContainsKey(gate.GateName))
            {
                BoardingGates.Add(gate.GateName, gate);
                return true;
            }
            return false;
        }

        public Airline GetAirlineFromFlight(Flight flight)
        {
            return Airlines.ContainsKey(flight.Airline.Code) ? Airlines[flight.Airline.Code] : null;
        }

        public void PrintAirlineFees()
        {
            foreach (var airline in Airlines.Values)
            {
                Console.WriteLine($"{airline.Name} Fees: ${airline.CalculateFees()}");
            }
        }
    }
}
