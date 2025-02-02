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
        private string terminalName;
        private Dictionary<string, Airline> airlines = new Dictionary<string, Airline>();
        private Dictionary<string, Flight> flights = new Dictionary<string, Flight>();
        private Dictionary<string, BoardingGate> boardingGates = new Dictionary<string, BoardingGate>();
        private Dictionary<string, double> gateFees;

        public string TerminalName { get { return terminalName; } }
        public Dictionary<string, Airline> Airlines { get { return airlines; } }
        public Dictionary<string, BoardingGate> BoardingGates { get { return boardingGates; } }

        public Terminal(string terminalName)
        {
            this.terminalName = terminalName;
            airlines = new Dictionary<string, Airline>();
            flights = new Dictionary<string, Flight>();
            boardingGates = new Dictionary<string, BoardingGate>();
            gateFees = new Dictionary<string, double>();
        }

        public bool AddAirline(Airline airline)
        {
            if (!airlines.ContainsKey(airline.Code))
            {
                airlines.Add(airline.Code, airline);
                return true;
            }
            return false;
        }

        public bool AddBoardingGate(BoardingGate gate)
        {
            if (!boardingGates.ContainsKey(gate.GateName))
            {
                boardingGates.Add(gate.GateName, gate);
                return true;
            }
            return false;
        }

        public Airline GetAirlineFromFlight(Flight flight)
        {
            foreach (var airline in airlines.Values)
            {
                if (airline.Flights.ContainsKey(flight.FlightNumber))
                {
                    return airline;
                }
            }
            return null;
        }

        public void PrintAirlineFees()
        {
            Console.WriteLine("=============================================");
            Console.WriteLine("Airline Fees for " + terminalName);
            Console.WriteLine("=============================================");
            Console.WriteLine(string.Format("{0,-20} {1,10}", "Airline", "Total Fees ($)"));
            Console.WriteLine("---------------------------------------------");
            foreach (var airline in airlines.Values)
            {
                double totalFee = 0;
                foreach (var flight in airline.Flights.Values)
                {
                    totalFee += flight.CalculateFees();
                }
                Console.WriteLine(string.Format("{0,-20} {1,10:F2}", airline.Name, totalFee));
            }
            Console.WriteLine("=============================================");
        }

        public override string ToString()
        {
            return $"Terminal: {terminalName}\nTotal Airlines: {airlines.Count}\nTotal Boarding Gates: {boardingGates.Count}";
        }
    }
}