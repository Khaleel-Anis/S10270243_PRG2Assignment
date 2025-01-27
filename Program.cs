//==========================================================
// Student Number : S10270243
// Student Number : S10269256
// Student Name : Khaleel Anis
// Partner Name : Hia Wei Dai
//==========================================================

using System;
using System.Collections.Generic;
using System.IO;


namespace PRG_2_Assignment
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Terminal terminal = new Terminal("Changi Airport Terminal 5");

            // Load data from CSV files
            LoadAirlines(terminal);
            LoadBoardingGates(terminal);
            LoadFlights(terminal);

            while (true)
            {
                Console.WriteLine("\n=============================================");
                Console.WriteLine("Welcome to Changi Airport Terminal 5");
                Console.WriteLine("=============================================");
                Console.WriteLine("1. List All Flights");
                Console.WriteLine("2. List Boarding Gates");
                Console.WriteLine("0. Exit");
                Console.Write("\nPlease select your option: ");

                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        ListAllFlights(terminal);
                        break;
                    case "2":
                        terminal.PrintBoardingGates();
                        break;
                    case "0":
                        Console.WriteLine("\nGoodbye!");
                        return;
                    default:
                        Console.WriteLine("\nInvalid option. Please try again.");
                        break;
                }
            }
        }

        static void LoadAirlines(Terminal terminal)
        {
            string filePath = "airlines.csv";
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Error: airlines.csv not found.");
                return;
            }

            using (StreamReader reader = new StreamReader(filePath))
            {
                bool isFirstLine = true;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        continue;
                    }
                    string[] parts = line.Split(',');
                    if (parts.Length == 2)
                    {
                        terminal.AddAirline(new Airline(parts[1].Trim(), parts[0].Trim()));
                    }
                }
            }
            Console.WriteLine("Airlines loaded successfully.");
        }

        static void LoadBoardingGates(Terminal terminal)
        {
            string filePath = "boardinggates.csv";
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Error: boardinggates.csv not found.");
                return;
            }

            using (StreamReader reader = new StreamReader(filePath))
            {
                bool isFirstLine = true;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        continue;
                    }
                    string[] parts = line.Split(',');
                    if (parts.Length == 4)
                    {
                        terminal.AddBoardingGate(new BoardingGate(parts[0].Trim(),
                            bool.Parse(parts[1].Trim()), bool.Parse(parts[2].Trim()), bool.Parse(parts[3].Trim())));
                    }
                }
            }
            Console.WriteLine("Boarding gates loaded successfully.");
        }

        static void ListAllFlights(Terminal terminal)
        {
            Console.WriteLine("\n=============================================");
            Console.WriteLine("List of Flights for Changi Airport Terminal 5");
            Console.WriteLine("=============================================");
            Console.WriteLine(string.Format("{0,-15} {1,-22} {2,-25} {3,-25} {4,-15}",
                "Flight Number", "Airline Name", "Origin", "Destination", "Expected"));
            Console.WriteLine(string.Format("{0,-15} {1,-22} {2,-25} {3,-25} {4,-15}",
                "Departure/Arrival Time", "", "", "", ""));

            if (terminal.Flights.Count == 0)
            {
                Console.WriteLine("No flights available.");
                return;
            }

            foreach (var flight in terminal.Flights.Values)
            {
                Console.WriteLine(string.Format("{0,-15} {1,-22} {2,-25} {3,-25} {4,-15}",
                    flight.FlightNumber, flight.Airline.Name, flight.Origin, flight.Destination,
                    flight.ExpectedTime.ToString("d/M/yyyy")));
                Console.WriteLine(string.Format("{0,-15} {1,-22} {2,-25} {3,-25} {4,-15}",
                    flight.ExpectedTime.ToString("h:mm:00 tt"), "", "", "", ""));
            }
        }

        static void LoadFlights(Terminal terminal)
        {
            string filePath = "flights.csv";
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Error: flights.csv not found.");
                return;
            }

            using (StreamReader reader = new StreamReader(filePath))
            {
                bool isFirstLine = true;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        continue;
                    }
                    string[] parts = line.Split(',');
                    if (parts.Length >= 4)
                    {
                        string flightNumber = parts[0].Trim();
                        string origin = parts[1].Trim();
                        string destination = parts[2].Trim();
                        string timeString = parts[3].Trim();
                        string specialRequest = parts.Length >= 5 ? parts[4].Trim() : "";

                        string dateTimeString = $"18/01/2025 {timeString}";
                        string[] dateFormats = { "dd/MM/yyyy h:mm tt", "dd/MM/yyyy hh:mm tt" };

                        if (DateTime.TryParseExact(dateTimeString, dateFormats, null, System.Globalization.DateTimeStyles.None, out DateTime expectedTime))
                        {
                            string airlineCode = flightNumber.Split(' ')[0];
                            Airline airline = terminal.Airlines.ContainsKey(airlineCode) ? terminal.Airlines[airlineCode] : new Airline(airlineCode, "Unknown Airline");

                            Flight flight = specialRequest.ToUpper() switch
                            {
                                "CFFT" => new CFFTFlight(flightNumber, airline, origin, destination, expectedTime),
                                "DDJB" => new DDJBFlight(flightNumber, airline, origin, destination, expectedTime),
                                "LWTT" => new LWTTFlight(flightNumber, airline, origin, destination, expectedTime),
                                _ => new NORMFlight(flightNumber, airline, origin, destination, expectedTime)
                            };
                            terminal.AddFlight(flight);
                        }
                        else
                        {
                            Console.WriteLine($"Error: Invalid time format for flight {flightNumber} - '{timeString}'");
                        }
                    }
                }
            }
            Console.WriteLine("Flights loaded successfully.");
        }
    }
}
