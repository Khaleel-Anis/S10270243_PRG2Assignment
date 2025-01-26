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

                if (Airlines.ContainsKey(flight.Airline.Code))
                {
                    Airlines[flight.Airline.Code].AddFlight(flight);
                }
                else
                {
                    Console.WriteLine($"Error: Airline {flight.Airline.Code} not found.");
                    return false;
                }

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

        public void PrintAirlineFees()
        {
            double totalTerminalFees = 0;

            Console.WriteLine("\n=====================================");
            Console.WriteLine("Airline Fees Summary for Terminal 5");
            Console.WriteLine("=====================================");

            foreach (var airline in Airlines.Values)
            {
                double airlineFee = airline.CalculateFees();
                totalTerminalFees += airlineFee;
                Console.WriteLine(string.Format("{0,-20} Fees: ${1,-10:N2}", airline.Name, airlineFee));
            }

            Console.WriteLine("\n=====================================");
            Console.WriteLine(string.Format("Total Fees Collected by Terminal 5: ${0,-10:N2}", totalTerminalFees));
            Console.WriteLine("=====================================");
        }

        public override string ToString()
        {
            string gateDetails = "\n=============================================";
            gateDetails += "\nList of Boarding Gates for Changi Airport Terminal 5";
            gateDetails += "\n=============================================";
            gateDetails += string.Format("\n{0,-10} {1,-10} {2,-10} {3,-10}", "Gate Name", "DDJB", "CFFT", "LWTT");
            
            foreach (var gate in BoardingGates.Values)
            {
                gateDetails += "\n" + gate.ToString();
            }

            return gateDetails;
        }
    }
}