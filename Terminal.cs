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
    internal class Terminal
    {
        public string TerminalName { get; }
        public Dictionary<string, Airline> Airlines { get; private set; }
        public Dictionary<string, Flight> Flights { get; private set; }
        public Dictionary<string, BoardingGate> BoardingGates { get; private set; }

        public Terminal(string terminalName)
        {
            TerminalName = terminalName;
            Airlines = new Dictionary<string, Airline>();
            Flights = new Dictionary<string, Flight>();
            BoardingGates = new Dictionary<string, BoardingGate>();
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

        public bool AddFlight(Flight flight)
        {
            if (!Flights.ContainsKey(flight.FlightNumber))
            {
                Flights.Add(flight.FlightNumber, flight);
                return true;
            }
            return false;
        }

        public void PrintBoardingGates()
        {
            Console.WriteLine("=============================================");
            Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
            Console.WriteLine("=============================================");
            Console.WriteLine(string.Format("{0,-10} {1,-10} {2,-10} {3,-10}", "Gate Name", "DDJB", "CFFT", "LWTT"));

            foreach (var gate in BoardingGates.Values)
            {
                Console.WriteLine(gate.ToString());
            }
        }

        public bool AssignBoardingGate(string flightNumber, string gateName)
        {
            if (Flights.ContainsKey(flightNumber) && BoardingGates.ContainsKey(gateName))
            {
                Flight flight = Flights[flightNumber];
                BoardingGate gate = BoardingGates[gateName];

                if (gate.AssignedFlight == null)
                {
                    flight.AssignGate(gate);
                    return true;
                }
                else
                {
                    Console.WriteLine("Error: Gate already occupied.");
                }
            }
            else
            {
                Console.WriteLine("Error: Flight or gate not found.");
            }
            return false;
        }

        public override string ToString()
        {
            string gateDetails = "\n=============================================";
            gateDetails += "\nList of Boarding Gates for Changi Airport Terminal 5";
            gateDetails += "\n=============================================";
            gateDetails += string.Format("\n{0,-10} {1,-10} {2,-10} {3,-10} {4,-15}", "Gate Name", "DDJB", "CFFT", "LWTT", "Assigned Flight");

            foreach (var gate in BoardingGates.Values)
            {
                string assignedFlight = gate.AssignedFlight != null ? gate.AssignedFlight.FlightNumber : "None";
                gateDetails += string.Format("\n{0,-10} {1,-10} {2,-10} {3,-10} {4,-15}",
                                             gate.GateName, gate.SupportsDDJB, gate.SupportsCFFT, gate.SupportsLWTT, assignedFlight);
            }

            return gateDetails;
        }
    }
}
