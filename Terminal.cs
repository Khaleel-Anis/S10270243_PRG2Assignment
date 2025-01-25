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

                // Ensure the flight is added to the respective airline
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

        public Airline GetAirlineFromFlight(Flight flight)
        {
            return Airlines.ContainsKey(flight.Airline.Code) ? Airlines[flight.Airline.Code] : null;
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
                Console.WriteLine($"{airline.Name} Fees: ${airlineFee}");
            }

            Console.WriteLine("\n=====================================");
            Console.WriteLine($"Total Fees Collected by Terminal 5: ${totalTerminalFees}");
            Console.WriteLine("=====================================");
        }
    }
}
