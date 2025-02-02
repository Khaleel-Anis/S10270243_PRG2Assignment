//==========================================================
// Student Number : S10270243
// Student Number : S10269256
// Student Name : Khaleel Anis
// Partner Name : Hia Wei Dai
//==========================================================

using System;
using System.Collections.Generic;
using System.IO;


namespace S10270243_PRG2Assignment
{
    class Program
    {
        static Terminal terminal = new Terminal("Changi Terminal 5");

        static void Main(string[] args)
        {
            Console.WriteLine("Loading Airlines...");
            LoadAirlines();
            Console.WriteLine($"{terminal.Airlines.Count} Airlines Loaded!");

            Console.WriteLine("Loading Boarding Gates...");
            LoadBoardingGates();
            Console.WriteLine($"{terminal.BoardingGates.Count} Boarding Gates Loaded!");

            Console.WriteLine("Loading Flights...");
            LoadFlights();
            int totalFlights = 0;
            foreach (var airline in terminal.Airlines.Values)
            {
                totalFlights += airline.Flights.Count;
            }
            Console.WriteLine($"{totalFlights} Flights Loaded!");

            Console.WriteLine("\n=============================================");
            Console.WriteLine("Welcome to Changi Airport Terminal 5 System!");
            Console.WriteLine("=============================================");

            while (true)
            {
                Console.WriteLine("\n1. List All Flights");
                Console.WriteLine("2. List Boarding Gates");
                Console.WriteLine("0. Exit");
                Console.Write("Please select an option: ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ListAllFlights();
                        break;
                    case "2":
                        ListAllBoardingGates();
                        break;
                    case "0":
                        Console.WriteLine("Exiting program...");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        static void ListAllFlights()
        {
            Console.WriteLine("\n=============================================");
            Console.WriteLine("List of Flights for Changi Airport Terminal 5");
            Console.WriteLine("=============================================");
            Console.WriteLine(string.Format("{0,-15} {1,-22} {2,-25} {3,-25} {4,-15}",
                "Flight Number", "Airline Name", "Origin", "Destination", "Expected"));
            Console.WriteLine(string.Format("{0,-15} {1,-22} {2,-25} {3,-25} {4,-15}",
                "Departure/Arrival Time", "", "", "", ""));

            foreach (var airline in terminal.Airlines.Values)
            {
                foreach (var flight in airline.Flights.Values)
                {
                    Console.WriteLine(string.Format("{0,-12} {1,-20} {2,-20} {3,-20}",
                        flight.FlightNumber, airline.Name, flight.Origin, flight.Destination));
                    Console.WriteLine($"Expected Departure/Arrival Time: {flight.ExpectedTime.ToString("d/M/yyyy h:mm:ss tt")}");
                    Console.WriteLine(); // New line for spacing
                }
            }
        }


        static void ListAllBoardingGates()
        {
            Console.WriteLine("\n=============================================");
            Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
            Console.WriteLine("=============================================");
            Console.WriteLine(string.Format("{0,-10} {1,-10} {2,-10} {3,-10} {4,-15}",
                "Gate", "DDJB", "CFFT", "LWTT", "Assigned Flight"));

            foreach (var gate in terminal.BoardingGates.Values)
            {
                string assignedFlight = gate.AssignedFlight != null ? gate.AssignedFlight.FlightNumber : "None";
                Console.WriteLine(string.Format("{0,-10} {1,-10} {2,-10} {3,-10} {4,-15}",
                    gate.GateName, gate.SupportsDDJB ? "Yes" : "No",
                    gate.SupportsCFFT ? "Yes" : "No",
                    gate.SupportsLWTT ? "Yes" : "No",
                    assignedFlight));
            }
        }

        static void LoadAirlines()
        {
            string filePath = "airlines.csv";
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Error: Airlines data file not found!");
                return;
            }

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] data = line.Split(',');
                    if (data.Length == 2)
                    {
                        Airline airline = new Airline(data[1], data[0]); // Code, Name
                        terminal.AddAirline(airline);
                    }
                }
            }
        }

        static void LoadBoardingGates()
        {
            string filePath = "boardinggates.csv";
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Error: Boarding gates data file not found!");
                return;
            }

            using (StreamReader reader = new StreamReader(filePath))
            {
                reader.ReadLine(); // ✅ Skip header row

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] data = line.Split(',');
                    if (data.Length == 4)
                    {
                        string gateName = data[0].Trim();
                        bool supportsDDJB = data[1].Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
                        bool supportsCFFT = data[2].Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
                        bool supportsLWTT = data[3].Trim().Equals("True", StringComparison.OrdinalIgnoreCase);

                        BoardingGate gate = new BoardingGate(gateName, supportsCFFT, supportsDDJB, supportsLWTT);
                        terminal.AddBoardingGate(gate);
                    }
                }
            }
        }

        static void LoadFlights()
        {
            string filePath = "flights.csv";
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Error: Flights data file not found!");
                return;
            }

            using (StreamReader reader = new StreamReader(filePath))
            {
                reader.ReadLine();

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] data = line.Split(',');
                    if (data.Length >= 4)
                    {
                        string flightNumber = data[0].Trim();
                        string origin = data[1].Trim();
                        string destination = data[2].Trim();
                        string timeOnly = data[3].Trim();
                        string specialRequest = data.Length == 5 ? data[4].Trim() : "None";

                        string fullDateTimeString = $"18/1/2025 {timeOnly}";
                        DateTime expectedTime = DateTime.ParseExact(fullDateTimeString, "d/M/yyyy h:mm tt", null);

                        if (string.IsNullOrWhiteSpace(specialRequest))
                        {
                            specialRequest = "None";
                        }

                        Flight flight;
                        if (specialRequest == "CFFT") flight = new CFFTFlight(flightNumber, origin, destination, expectedTime);
                        else if (specialRequest == "DDJB") flight = new DDJBFlight(flightNumber, origin, destination, expectedTime);
                        else if (specialRequest == "LWTT") flight = new LWTTFlight(flightNumber, origin, destination, expectedTime);
                        else flight = new NORMFlight(flightNumber, origin, destination, expectedTime);

                        // ✅ Assign flight to correct airline
                        string airlineCode = flightNumber.Split(' ')[0]; // Extract "SQ" from "SQ 115"
                        if (terminal.Airlines.ContainsKey(airlineCode))
                        {
                            terminal.Airlines[airlineCode].AddFlight(flight);
                        }
                        else
                        {
                            Console.WriteLine($"Warning: No airline found for flight {flightNumber}.");
                        }
                    }
                }
            }
        }
    }
}
