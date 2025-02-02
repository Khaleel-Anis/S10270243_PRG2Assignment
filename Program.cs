﻿//==========================================================
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
            //
            while (true)
            {
                Console.WriteLine("\n=============================================");
                Console.WriteLine("Welcome to Changi Airport Terminal 5");
                Console.WriteLine("=============================================");
                Console.WriteLine("1. List All Flights");
                Console.WriteLine("2. List Boarding Gates");
                Console.WriteLine("4. Create Flight");
                Console.WriteLine("7. Display Flight Schedule");
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
                    case "4":
                        CreateFlight(terminal);
                        break;
                    case "7":
                        DisplayFlightSchedule(terminal);
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

        static void CreateFlight(Terminal terminal)
        {
            while (true)
            {
                // Get flight number
                Console.Write("Enter Flight Number: ");
                string flightNumber = Console.ReadLine().Trim();
                if (terminal.Flights.ContainsKey(flightNumber))
                {
                    Console.WriteLine("Error: Flight number already exists.");
                    return;
                }

                // Get origin and destination
                Console.Write("Enter Origin: ");
                string origin = Console.ReadLine().Trim();
                Console.Write("Enter Destination: ");
                string destination = Console.ReadLine().Trim();

                // Get expected departure/arrival time
                // month needs 0 in front if month is 01
                Console.Write("Enter Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
                string dateTimeString = Console.ReadLine().Trim();
                if (!DateTime.TryParseExact(dateTimeString, "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime expectedTime))
                {
                    Console.WriteLine("Error: Invalid date format.");
                    return;
                }

                // Get special request code
                Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
                string specialRequest = Console.ReadLine().Trim().ToUpper();

                // Determine flight type based on special request
                Flight flight;
                switch (specialRequest)
                {
                    case "CFFT":
                        flight = new CFFTFlight(flightNumber, new Airline("N/A", "Unknown Airline"), origin, destination, expectedTime);
                        break;
                    case "DDJB":
                        flight = new DDJBFlight(flightNumber, new Airline("N/A", "Unknown Airline"), origin, destination, expectedTime);
                        break;
                    case "LWTT":
                        flight = new LWTTFlight(flightNumber, new Airline("N/A", "Unknown Airline"), origin, destination, expectedTime);
                        break;
                    default:
                        flight = new NORMFlight(flightNumber, new Airline("N/A", "Unknown Airline"), origin, destination, expectedTime);
                        break;
                }

                // Add flight to terminal
                terminal.AddFlight(flight);
                Console.WriteLine($"Flight {flightNumber} has been added!");

                // Ask if user wants to add another flight
                Console.Write("Would you like to add another flight? (Y/N): ");
                string response = Console.ReadLine().Trim().ToUpper();
                if (response != "Y")
                {
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
            Console.WriteLine(string.Format("{0,-12} {1,-20} {2,-20} {3,-20} {4,-12} {5,-12}",
                "Flight No.", "Airline", "Origin", "Destination", "Expected Time", "Status"));

            if (terminal.Flights.Count == 0)
            {
                Console.WriteLine("No flights available.");
                return;
            }

            foreach (var flight in terminal.Flights.Values)
            {
                Airline airline = terminal.GetAirlineFromFlight(flight);
                string airlineName = airline != null ? airline.Name : "Unknown Airline";

                Console.WriteLine(string.Format("{0,-12} {1,-20} {2,-20} {3,-20} {4,-12} {5,-12}",
                    flight.FlightNumber, airlineName, flight.Origin, flight.Destination,
                    flight.ExpectedTime.ToString("d/M/yyyy h:mm tt"), flight.Status));
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

        static void DisplayFlightSchedule(Terminal terminal)
        {
            Console.WriteLine("\n=============================================");
            Console.WriteLine("Flight Schedule for Changi Airport Terminal 5");
            Console.WriteLine("=============================================");
            Console.WriteLine("Flight Number  Airline Name         Origin                 Destination            Expected");
            Console.WriteLine("Departure/Arrival Time   Status      Boarding Gate");

            if (terminal.Flights.Count == 0)
            {
                Console.WriteLine("No scheduled flights available.");
                return;
            }

            // Sort flights by expected departure/arrival time
            List<Flight> sortedFlights = new List<Flight>(terminal.Flights.Values);
            sortedFlights.Sort((f1, f2) => f1.ExpectedTime.CompareTo(f2.ExpectedTime));

            foreach (var flight in sortedFlights)
            {
                string gate = flight.AssignedGate != null ? flight.AssignedGate.GateName : "Unassigned";
                Console.WriteLine(string.Format("{0,-15} {1,-22} {2,-25} {3,-25} {4,-15}",
                    flight.FlightNumber, flight.Airline.Name, flight.Origin, flight.Destination,
                    flight.ExpectedTime.ToString("d/M/yyyy")));
                Console.WriteLine(string.Format("{0,-25} {1,-12} {2,-15}",
                    flight.ExpectedTime.ToString("h:mm:00 tt"), "Scheduled", gate));
            }
        }
    }
}
