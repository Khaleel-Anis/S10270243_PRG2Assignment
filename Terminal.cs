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

namespace S10270243_PRG2Assignment
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
        public void ListAllFlights()
        {
            Console.WriteLine("=============================================");
            Console.WriteLine($"List of Flights for {terminalName}");
            Console.WriteLine("=============================================");
            Console.WriteLine("{0,-12} {1,-20} {2,-20} {3,-30} {4,-12}",
                "Flight No", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time");

            foreach (var flight in flights.Values)
            {
                Airline airline = GetAirlineFromFlight(flight); // Retrieve the airline
                string airlineName = airline != null ? airline.Name : "Unknown"; // Ensure airline exists

                Console.WriteLine("{0,-12} {1,-20} {2,-20} {3,-20} {4,-12}",
                    flight.FlightNumber,
                    airlineName,
                    flight.Origin,
                    flight.Destination,
                    flight.ExpectedTime.ToString("d/M/yyyy h:mm:ss tt")); // Matches sample output format
            }
            Console.WriteLine();
        }

        public void AssignBoardingGate()
        {
            Console.Write("Enter Flight Number: ");
            string flightNumber = Console.ReadLine()?.Trim().ToUpper();

            if (!flights.ContainsKey(flightNumber))
            {
                Console.WriteLine("Error: Flight not found.");
                return;
            }

            Flight flight = flights[flightNumber];

            // Display Flight Details
            Console.WriteLine("\nFlight Details:");
            Console.WriteLine($"Flight Number: {flight.FlightNumber}");
            Console.WriteLine($"Origin: {flight.Origin}");
            Console.WriteLine($"Destination: {flight.Destination}");
            Console.WriteLine($"Expected Time: {flight.ExpectedTime:dd/M/yyyy h:mm:ss tt}");
            Console.WriteLine($"Status: {flight.Status}");
            Console.WriteLine($"Special Request: {(flight is CFFTFlight ? "CFFT" : flight is DDJBFlight ? "DDJB" : flight is LWTTFlight ? "LWTT" : "None")}");

            // Prompt for Boarding Gate
            string gateName;
            do
            {
                Console.Write("\nEnter Boarding Gate: ");
                gateName = Console.ReadLine()?.Trim().ToUpper();

                if (!boardingGates.ContainsKey(gateName))
                {
                    Console.WriteLine("Error: Boarding Gate not found.");
                    continue;
                }

                if (boardingGates[gateName].AssignedFlight != null) // Now using the public getter
                {
                    Console.WriteLine("Error: Boarding Gate already assigned. Choose another.");
                    continue;
                }

                boardingGates[gateName].AssignFlight(flight);
                break;

            } while (true);

            // Prompt user to update flight status
            Console.Write("Would you like to update the flight status? (Y/N): ");
            string updateStatus = Console.ReadLine()?.Trim().ToUpper();

            if (updateStatus == "Y")
            {
                Console.Write("Enter new status (Delayed/Boarding/On Time): ");
                string newStatus = Console.ReadLine()?.Trim();

                if (newStatus == "Delayed" || newStatus == "Boarding" || newStatus == "On Time")
                {
                    flight.Status = newStatus;
                }
                else
                {
                    Console.WriteLine("Invalid status. Defaulting to 'On Time'.");
                    flight.Status = "On Time";
                }
            }
            else
            {
                flight.Status = "On Time";
            }

            // Success Message
            Console.WriteLine($"\nSuccessfully assigned Boarding Gate {gateName} to Flight {flightNumber}");
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