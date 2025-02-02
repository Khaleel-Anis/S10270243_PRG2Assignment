//==========================================================
// Student Number : S10270243
// Student Number : S10269256
// Student Name : Khaleel Anis
// Partner Name : Hia Wei Dai
//==========================================================

using System;
using System.Collections.Generic;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;


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

            while (true)
            {
                Console.WriteLine("\n=============================================");
                Console.WriteLine("Welcome to Changi Airport Terminal 5 System!");
                Console.WriteLine("=============================================");
                Console.WriteLine("\n1. List All Flights");
                Console.WriteLine("2. List Boarding Gates");
                Console.WriteLine("3. Assign Boarding Gate");
                Console.WriteLine("6. Modify Flight Details");
                Console.WriteLine("7. Display Flight Schedule");
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
                    case "3":
                        AssignBoardingGate();
                        break;
                    case "6":
                        ModifyFlightDetails();
                        break;
                    case "7":
                        DisplayFlightSchedule();
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
            Console.WriteLine(string.Format("{0,-12} {1,-20} {2,-20} {3,-20} {4,-12}",
                "Flight Number", "Airline Name", "Origin", "Destination", "Expected"));
            Console.WriteLine("Departure/Arrival Time");

            foreach (var airline in terminal.Airlines.Values)
            {
                foreach (var flight in airline.Flights.Values)
                {
                    Console.WriteLine(string.Format("{0,-12} {1,-20} {2,-20} {3,-20} {4,-12}",
                        flight.FlightNumber, airline.Name, flight.Origin, flight.Destination,
                        flight.ExpectedTime.ToString("d/M/yyyy")));

                    Console.WriteLine(string.Format("{0,-12}",
                        flight.ExpectedTime.ToString("h:mm:ss tt")));
                }
            }
        }

        static void ListAllBoardingGates()
        {
            Console.WriteLine("\n=============================================");
            Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
            Console.WriteLine("=============================================");
            Console.WriteLine(string.Format("{0,-10} {1,-10} {2,-10} {3,-10}",
                "Gate Name", "DDJB", "CFFT", "LWTT"));

            foreach (var gate in terminal.BoardingGates.Values)
            {
                Console.WriteLine(string.Format("{0,-10} {1,-10} {2,-10} {3,-10}",
                    gate.GateName, gate.SupportsDDJB ? "True" : "False",
                    gate.SupportsCFFT ? "True" : "False",
                    gate.SupportsLWTT ? "True" : "False"));
            }
        }
        static void AssignBoardingGate()
        {
            Console.WriteLine("\n=============================================");
            Console.WriteLine("Assign a Boarding Gate to a Flight");
            Console.WriteLine("=============================================");

            Console.Write("Enter Flight Number: ");
            string flightNumber = Console.ReadLine()?.Trim().ToUpper();

            // Find the flight manually from all airlines
            Flight flight = null;
            Airline flightAirline = null;

            foreach (var airline in terminal.Airlines.Values)
            {
                if (airline.Flights.ContainsKey(flightNumber))
                {
                    flight = airline.Flights[flightNumber];
                    flightAirline = airline;
                    break;
                }
            }

            if (flight == null)
            {
                Console.WriteLine("Error: Flight not found.");
                return;
            }

            Console.Write("Enter Boarding Gate Name: ");
            string gateName = Console.ReadLine()?.Trim().ToUpper();

            if (!terminal.BoardingGates.ContainsKey(gateName))
            {
                Console.WriteLine("Error: Boarding Gate not found.");
                return;
            }

            BoardingGate gate = terminal.BoardingGates[gateName];

            // Check if the boarding gate is already assigned
            if (gate.AssignedFlight != null)
            {
                Console.WriteLine($"Error: Boarding Gate {gate.GateName} is already assigned to Flight {gate.AssignedFlight.FlightNumber}. Choose another.");
                return;
            }

            // Assign flight to gate
            gate.AssignFlight(flight);

            // Display assigned details (Matching Sample Output)
            Console.WriteLine("\n=============================================");
            Console.WriteLine($"Flight Number: {flight.FlightNumber}");
            Console.WriteLine($"Origin: {flight.Origin}");
            Console.WriteLine($"Destination: {flight.Destination}");
            Console.WriteLine($"Expected Time: {flight.ExpectedTime:dd/M/yyyy h:mm:ss tt}");
            Console.WriteLine($"Special Request Code: {(flight is CFFTFlight ? "CFFT" : flight is DDJBFlight ? "DDJB" : flight is LWTTFlight ? "LWTT" : "None")}");
            Console.WriteLine($"Boarding Gate Name: {gate.GateName}");
            Console.WriteLine($"Supports DDJB: {gate.SupportsDDJB}");
            Console.WriteLine($"Supports CFFT: {gate.SupportsCFFT}");
            Console.WriteLine($"Supports LWTT: {gate.SupportsLWTT}");

            // Prompt for status update
            Console.Write("\nWould you like to update the status of the flight? (Y/N): ");
            string statusChoice = Console.ReadLine()?.Trim().ToUpper();

            if (statusChoice == "Y")
            {
                Console.WriteLine("\n1. Delayed");
                Console.WriteLine("2. Boarding");
                Console.WriteLine("3. On Time");
                Console.Write("Please select the new status of the flight: ");
                string statusOption = Console.ReadLine();

                if (statusOption == "1") flight.Status = "Delayed";
                else if (statusOption == "2") flight.Status = "Boarding";
                else if (statusOption == "3") flight.Status = "On Time";
                else Console.WriteLine("Invalid option. Status unchanged.");
            }
            else
            {
                flight.Status = "On Time"; // Default to On Time if not changed
            }

            // **Final Output (Fixing Boarding Gate Header Alignment)**
            Console.WriteLine("\n=============================================");
            Console.WriteLine("Updated Flight Information");
            Console.WriteLine("=============================================");

            // Flight details header
            Console.WriteLine(string.Format("{0,-15} {1,-22} {2,-25} {3,-25} {4,-15}",
                "Flight Number", "Airline Name", "Origin", "Destination", "Expected"));

            // Status, Boarding Gate, Departure/Arrival Time headers (Correctly aligned)
            Console.WriteLine(string.Format("{0,-15} {1,-5} {2,-15}",
                "Departure/Arrival Time", "Status", "Boarding Gate"));

            Console.WriteLine(string.Format("{0,-15} {1,-22} {2,-25} {3,-25} {4,-15}",
                flight.FlightNumber, flightAirline.Name, flight.Origin, flight.Destination,
                flight.ExpectedTime.ToString("d/M/yyyy")));

            Console.WriteLine(string.Format("{0,-15} {1,-12} {2,-15}",
                flight.ExpectedTime.ToString("h:mm:ss tt"), flight.Status, gate.GateName));

            Console.WriteLine($"\nFlight {flight.FlightNumber} has been assigned to Boarding Gate {gate.GateName}!");
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
                reader.ReadLine(); // Skip header row

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

                        // Assign flight to correct airline
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

        static void ModifyFlightDetails()
        {
            Console.WriteLine("\n=============================================");
            Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
            Console.WriteLine("=============================================");
            Console.WriteLine(string.Format("{0,-12} {1,-20}", "Airline Code", "Airline Name"));

            foreach (var airline in terminal.Airlines.Values)
            {
                Console.WriteLine(string.Format("{0,-12} {1,-20}", airline.Code, airline.Name));
            }

            Console.Write("\nEnter Airline Code: ");
            string airlineCode = Console.ReadLine().ToUpper();

            if (!terminal.Airlines.ContainsKey(airlineCode))
            {
                Console.WriteLine("Error: Invalid Airline Code.");
                return;
            }

            Airline selectedAirline = terminal.Airlines[airlineCode];

            Console.WriteLine("\n=============================================");
            Console.WriteLine($"List of Flights for {selectedAirline.Name}");
            Console.WriteLine("=============================================");
            Console.WriteLine(string.Format("{0,-12} {1,-20} {2,-20} {3,-20} {4,-12}",
                "Flight Number", "Airline Name", "Origin", "Destination", "Expected"));
            Console.WriteLine("Departure/Arrival Time");

            foreach (var flight in selectedAirline.Flights.Values)
            {
                Console.WriteLine(string.Format("{0,-12} {1,-20} {2,-20} {3,-20} {4,-12}",
                    flight.FlightNumber, selectedAirline.Name, flight.Origin, flight.Destination,
                    flight.ExpectedTime.ToString("d/M/yyyy")));
                Console.WriteLine(string.Format("{0,-12}", flight.ExpectedTime.ToString("h:mm:ss tt")));
            }

            Console.Write("\nChoose an existing Flight to modify or delete: ");
            string flightNumber = Console.ReadLine().ToUpper();

            if (!selectedAirline.Flights.ContainsKey(flightNumber))
            {
                Console.WriteLine("Error: Flight not found.");
                return;
            }

            Flight selectedFlight = selectedAirline.Flights[flightNumber];

            Console.WriteLine("\n1. Modify Flight");
            Console.WriteLine("2. Delete Flight");
            Console.Write("Choose an option: ");

            string choice = Console.ReadLine();

            if (choice == "2")
            {
                selectedAirline.RemoveFlight(selectedFlight);
                Console.WriteLine($"Flight {flightNumber} has been deleted.");
                return;
            }
            else if (choice != "1")
            {
                Console.WriteLine("Invalid option.");
                return;
            }

            Console.WriteLine("\n1. Modify Basic Information");
            Console.WriteLine("2. Modify Status");
            Console.WriteLine("3. Modify Special Request Code");
            Console.WriteLine("4. Modify Boarding Gate");
            Console.Write("Choose an option: ");

            string modifyChoice = Console.ReadLine();

            switch (modifyChoice)
            {
                case "1":
                    Console.Write("Enter new Origin: ");
                    string newOrigin = Console.ReadLine();

                    Console.Write("Enter new Destination: ");
                    string newDestination = Console.ReadLine();

                    Console.Write("Enter new Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
                    if (DateTime.TryParseExact(Console.ReadLine(), "d/M/yyyy H:mm", null, System.Globalization.DateTimeStyles.None, out DateTime newTime))
                    {
                        // Replace the flight with an updated version
                        selectedAirline.RemoveFlight(selectedFlight);

                        Flight updatedFlight;
                        if (selectedFlight is CFFTFlight)
                            updatedFlight = new CFFTFlight(flightNumber, newOrigin, newDestination, newTime);
                        else if (selectedFlight is DDJBFlight)
                            updatedFlight = new DDJBFlight(flightNumber, newOrigin, newDestination, newTime);
                        else if (selectedFlight is LWTTFlight)
                            updatedFlight = new LWTTFlight(flightNumber, newOrigin, newDestination, newTime);
                        else
                            updatedFlight = new NORMFlight(flightNumber, newOrigin, newDestination, newTime);

                        selectedAirline.AddFlight(updatedFlight);
                        selectedFlight = updatedFlight; // Assign the new instance back to selectedFlight
                    }
                    else
                    {
                        Console.WriteLine("Invalid time format. No changes made.");
                    }
                    break;

                case "2":
                    Console.WriteLine("\n1. Delayed");
                    Console.WriteLine("2. Boarding");
                    Console.WriteLine("3. On Time");
                    Console.Write("Choose a new status: ");
                    string statusChoice = Console.ReadLine();

                    if (statusChoice == "1") selectedFlight.Status = "Delayed";
                    else if (statusChoice == "2") selectedFlight.Status = "Boarding";
                    else if (statusChoice == "3") selectedFlight.Status = "On Time";
                    else Console.WriteLine("Invalid option. No changes made.");
                    break;
            }

            Console.WriteLine("\nFlight updated!");
            Console.WriteLine($"Flight Number: {selectedFlight.FlightNumber}");
            Console.WriteLine($"Airline Name: {selectedAirline.Name}");
            Console.WriteLine($"Origin: {selectedFlight.Origin}");
            Console.WriteLine($"Destination: {selectedFlight.Destination}");
            Console.WriteLine($"Expected Departure/Arrival Time: {selectedFlight.ExpectedTime.ToString("d/M/yyyy h:mm:ss tt")}");
            Console.WriteLine($"Status: {selectedFlight.Status}");
        }


        static void DisplayFlightSchedule()
        {
            Console.WriteLine("\n=============================================");
            Console.WriteLine("Flight Schedule for Changi Airport Terminal 5");
            Console.WriteLine("=============================================");

            // First row: Flight details headers
            Console.WriteLine(string.Format("{0,-12} {1,-20} {2,-20} {3,-20} {4,-12}",
                "Flight Number", "Airline Name", "Origin", "Destination", "Expected"));

            // Second row: Status, Boarding Gate, Departure/Arrival Time headers
            Console.WriteLine(string.Format("{0,-12} {1,-20} {2,-20} {3,-20} {4,-12}",
                "Departure/Arrival Time", "Status", "Boarding Gate", "", ""));

            List<Flight> allFlights = new List<Flight>();

            // Collect all flights
            foreach (var airline in terminal.Airlines.Values)
            {
                allFlights.AddRange(airline.Flights.Values);
            }

            // Sort flights by ExpectedTime (earliest first)
            allFlights.Sort((x, y) => x.ExpectedTime.CompareTo(y.ExpectedTime));

            // Print sorted flights
            foreach (var flight in allFlights)
            {
                string airlineName = terminal.Airlines[flight.FlightNumber.Split(' ')[0]].Name; // Get airline name

                // Find assigned boarding gate
                string boardingGate = "Unassigned";
                foreach (var gate in terminal.BoardingGates.Values)
                {
                    if (gate.AssignedFlight == flight)
                    {
                        boardingGate = gate.GateName;
                        break;
                    }
                }

                // Print the first row (Flight details)
                Console.WriteLine(string.Format("{0,-12} {1,-20} {2,-20} {3,-20} {4,-12}",
                    flight.FlightNumber, airlineName, flight.Origin, flight.Destination,
                    flight.ExpectedTime.ToString("d/M/yyyy")));

                // Print the second row (Departure/Arrival Time, Status, Boarding Gate)
                Console.WriteLine(string.Format("{0,-12} {1,-20} {2,-20} {3,-20} {4,-12}",
                    flight.ExpectedTime.ToString("h:mm:ss tt"), flight.Status, boardingGate, "", ""));
            }
        }
    }
}
