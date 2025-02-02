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
                Console.WriteLine("4. Create Flight");
                Console.WriteLine("5. Display Airline Flights");
                Console.WriteLine("6. Modify Flight Details");
                Console.WriteLine("7. Display Flight Schedule");
                Console.WriteLine("8. Process Unassigned Flights");
                Console.WriteLine("9. Display Total Fees Per Airline");
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
                    case "4":
                        CreateFlight();
                        break;
                    case "5":
                        DisplayAirlineFlights();
                        break;
                    case "6":
                        ModifyFlightDetails();
                        break;
                    case "7":
                        DisplayFlightSchedule();
                        break;
                    case "8":
                        ProcessUnassignedFlights();
                        break;
                    case "9":
                        DisplayTotalFeesPerAirline();
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

            Console.Write("Enter Flight Number (or type 'exit' to cancel): ");
            string flightNumber = Console.ReadLine()?.Trim().ToUpper();
            if (flightNumber == "EXIT" || flightNumber == "0") return;

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

            Console.Write("Enter Boarding Gate Name (or type 'exit' to cancel): ");
            string gateName = Console.ReadLine()?.Trim().ToUpper();
            if (gateName == "EXIT" || gateName == "0") return;

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

            Console.Write("\nWould you like to update the status of the flight? (Y/N or 'exit' to cancel): ");
            string statusChoice = Console.ReadLine()?.Trim().ToUpper();
            if (statusChoice == "EXIT" || statusChoice == "0") return;

            if (statusChoice == "Y")
            {
                Console.WriteLine("\n1. Delayed");
                Console.WriteLine("2. Boarding");
                Console.WriteLine("3. On Time");
                Console.Write("Please select the new status of the flight (or type 'exit' to cancel): ");
                string statusOption = Console.ReadLine();
                if (statusOption == "EXIT" || statusOption == "0") return;

                if (statusOption == "1") flight.Status = "Delayed";
                else if (statusOption == "2") flight.Status = "Boarding";
                else if (statusOption == "3") flight.Status = "On Time";
                else Console.WriteLine("Invalid option. Status unchanged.");
            }

            Console.WriteLine($"\nFlight {flight.FlightNumber} has been assigned to Boarding Gate {gate.GateName}!");
        }

        static void CreateFlight()
        {
            List<string> newFlights = new List<string>();

            while (true)
            {
                Console.WriteLine("\n=============================================");
                Console.WriteLine("Create a New Flight");
                Console.WriteLine("=============================================");

                Console.Write("Enter Flight Number (or type 'exit' to cancel): ");
                string flightNumber = Console.ReadLine()?.Trim().ToUpper();
                if (flightNumber == "EXIT" || flightNumber == "0") return;

                foreach (var airline in terminal.Airlines.Values)
                {
                    if (airline.Flights.ContainsKey(flightNumber))
                    {
                        Console.WriteLine($"Error: Flight {flightNumber} already exists.");
                        return;
                    }
                }

                Console.Write("Enter Origin (or type 'exit' to cancel): ");
                string origin = Console.ReadLine()?.Trim();
                if (origin == "EXIT" || origin == "0") return;

                Console.Write("Enter Destination (or type 'exit' to cancel): ");
                string destination = Console.ReadLine()?.Trim();
                if (destination == "EXIT" || destination == "0") return;

                Console.Write("Enter Expected Departure/Arrival Time (dd/MM/yyyy HH:mm) (or type 'exit' to cancel): ");
                string dateTimeInput = Console.ReadLine();
                if (dateTimeInput == "EXIT" || dateTimeInput == "0") return;

                if (!DateTime.TryParseExact(dateTimeInput, "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime expectedTime))
                {
                    Console.WriteLine("Error: Invalid date format. Flight not added.");
                    continue;
                }

                Console.Write("Would you like to enter a Special Request Code? (Y/N or 'exit' to cancel): ");
                string addSpecialRequest = Console.ReadLine()?.Trim().ToUpper();
                if (addSpecialRequest == "EXIT" || addSpecialRequest == "0") return;

                string specialRequest = "None";
                if (addSpecialRequest == "Y")
                {
                    Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT) (or type 'exit' to cancel): ");
                    specialRequest = Console.ReadLine()?.Trim().ToUpper();
                    if (specialRequest == "EXIT" || specialRequest == "0") return;

                    if (specialRequest != "CFFT" && specialRequest != "DDJB" && specialRequest != "LWTT")
                    {
                        Console.WriteLine("Invalid Special Request Code. Defaulting to None.");
                        specialRequest = "None";
                    }
                }

                Flight flight = specialRequest switch
                {
                    "CFFT" => new CFFTFlight(flightNumber, origin, destination, expectedTime),
                    "DDJB" => new DDJBFlight(flightNumber, origin, destination, expectedTime),
                    "LWTT" => new LWTTFlight(flightNumber, origin, destination, expectedTime),
                    _ => new NORMFlight(flightNumber, origin, destination, expectedTime)
                };

                string airlineCode = flightNumber.Split(' ')[0];

                if (terminal.Airlines.ContainsKey(airlineCode))
                {
                    terminal.Airlines[airlineCode].AddFlight(flight);
                }
                else
                {
                    Console.WriteLine($"Warning: No airline found for flight {flightNumber}. Flight not added.");
                    return;
                }

                Console.WriteLine($"Flight {flightNumber} successfully added!");

                newFlights.Add($"{flightNumber},{origin},{destination},{expectedTime:HH:mm},{specialRequest}");

                Console.Write("Would you like to add another Flight? (Y/N or 'exit' to cancel): ");
                string response = Console.ReadLine()?.Trim().ToUpper();
                if (response == "EXIT" || response == "0" || response != "Y") break;
            }

            if (newFlights.Count > 0)
            {
                using (StreamWriter writer = new StreamWriter("flights.csv", true))
                {
                    foreach (string flightEntry in newFlights)
                    {
                        writer.WriteLine(flightEntry);
                    }
                }
                Console.WriteLine("\nFlight(s) successfully added to the system!");
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

        static void DisplayAirlineFlights()
        {
            Console.WriteLine("\n=============================================");
            Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
            Console.WriteLine("=============================================");
            Console.WriteLine(string.Format("{0,-12} {1,-20}", "Airline Code", "Airline Name"));

            // Display all airlines
            foreach (var airline in terminal.Airlines.Values)
            {
                Console.WriteLine(string.Format("{0,-12} {1,-20}", airline.Code, airline.Name));
            }

            Console.Write("\nEnter Airline Code: ");
            string airlineCode = Console.ReadLine().ToUpper();

            // Validate airline code
            if (!terminal.Airlines.ContainsKey(airlineCode))
            {
                Console.WriteLine("Error: Invalid Airline Code.");
                return;
            }

            Airline selectedAirline = terminal.Airlines[airlineCode];

            Console.WriteLine("\n=============================================");
            Console.WriteLine($"List of Flights for {selectedAirline.Name}");
            Console.WriteLine("=============================================");
            Console.WriteLine(string.Format("{0,-12} {1,-20} {2,-20}", "Flight Number", "Origin", "Destination"));

            // Display all flights for selected airline
            foreach (var flight in selectedAirline.Flights.Values)
            {
                Console.WriteLine(string.Format("{0,-12} {1,-20} {2,-20}",
                    flight.FlightNumber, flight.Origin, flight.Destination));
            }

            Console.Write("\nChoose a Flight Number: ");
            string flightNumber = Console.ReadLine().ToUpper();

            // Validate flight selection
            if (!selectedAirline.Flights.ContainsKey(flightNumber))
            {
                Console.WriteLine("Error: Flight not found.");
                return;
            }

            Flight selectedFlight = selectedAirline.Flights[flightNumber];

            // Find assigned boarding gate (if any)
            string boardingGate = "Unassigned";
            foreach (var gate in terminal.BoardingGates.Values)
            {
                if (gate.AssignedFlight == selectedFlight)
                {
                    boardingGate = gate.GateName;
                    break;
                }
            }

            // Determine Special Request Code (CFFT, DDJB, LWTT, None)
            string specialRequest = selectedFlight is CFFTFlight ? "CFFT"
                                : selectedFlight is DDJBFlight ? "DDJB"
                                : selectedFlight is LWTTFlight ? "LWTT"
                                : "None";

            // Display full flight details
            Console.WriteLine("\n=============================================");
            Console.WriteLine("Flight Details");
            Console.WriteLine("=============================================");
            Console.WriteLine($"Flight Number: {selectedFlight.FlightNumber}");
            Console.WriteLine($"Airline Name: {selectedAirline.Name}");
            Console.WriteLine($"Origin: {selectedFlight.Origin}");
            Console.WriteLine($"Destination: {selectedFlight.Destination}");
            Console.WriteLine($"Expected Departure/Arrival Time: {selectedFlight.ExpectedTime.ToString("d/M/yyyy h:mm:ss tt")}");
            Console.WriteLine($"Special Request Code: {specialRequest}");
            Console.WriteLine($"Boarding Gate: {boardingGate}");
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

            // ✅ Adjusted column widths for better alignment
            Console.WriteLine(string.Format("{0,-12} {1,-22} {2,-20} {3,-20} {4,-18} {5,-12} {6,-15}",
                "Flight No", "Airline Name", "Origin", "Destination", "Expected Time", "Status", "Boarding Gate"));

            List<Flight> allFlights = new List<Flight>();

            foreach (var airline in terminal.Airlines.Values)
            {
                allFlights.AddRange(airline.Flights.Values);
            }

            try
            {
                // ✅ Sorting flights in chronological order
                allFlights.Sort();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Sorting Error: " + ex.Message);
                return;
            }

            foreach (var flight in allFlights)
            {
                string airlineName = terminal.Airlines.ContainsKey(flight.FlightNumber.Split(' ')[0])
                    ? terminal.Airlines[flight.FlightNumber.Split(' ')[0]].Name
                    : "Unknown";

                string boardingGate = "Unassigned";

                foreach (var gate in terminal.BoardingGates.Values)
                {
                    if (gate.AssignedFlight == flight)
                    {
                        boardingGate = gate.GateName;
                        break;
                    }
                }

                // ✅ Adjusted widths for proper alignment
                Console.WriteLine(string.Format("{0,-12} {1,-22} {2,-20} {3,-20} {4,-18} {5,-12} {6,-15}",
                    flight.FlightNumber, airlineName, flight.Origin, flight.Destination,
                    flight.ExpectedTime.ToString("dd/MM/yyyy HH:mm"), flight.Status, boardingGate));

            }

        }
        static void ProcessUnassignedFlights()
        {
            Queue<Flight> unassignedFlights = new Queue<Flight>();

            // ✅ Step 1: Identify flights with no assigned boarding gate
            foreach (var airline in terminal.Airlines.Values)
            {
                foreach (var flight in airline.Flights.Values)
                {
                    if (!terminal.BoardingGates.Values.Any(g => g.AssignedFlight == flight))
                    {
                        unassignedFlights.Enqueue(flight);
                    }
                }
            }

            Console.WriteLine($"Total unassigned flights: {unassignedFlights.Count}");

            // ✅ Step 2: Identify boarding gates with no assigned flights
            List<BoardingGate> availableGates = terminal.BoardingGates.Values.Where(g => g.AssignedFlight == null).ToList();
            Console.WriteLine($"Total unassigned boarding gates: {availableGates.Count}");

            int assignedFlights = 0;
            int assignedGates = 0;

            while (unassignedFlights.Count > 0 && availableGates.Count > 0)
            {
                Flight flight = unassignedFlights.Dequeue();
                BoardingGate assignedGate = null;

                // ✅ Step 3: Check if flight has a special request code
                bool hasSpecialRequest = flight is CFFTFlight || flight is DDJBFlight || flight is LWTTFlight;

                if (hasSpecialRequest)
                {
                    // ✅ Step 4: Find a gate that supports the special request
                    assignedGate = availableGates.FirstOrDefault(g =>
                        (flight is CFFTFlight && g.SupportsCFFT) ||
                        (flight is DDJBFlight && g.SupportsDDJB) ||
                        (flight is LWTTFlight && g.SupportsLWTT));
                }

                // ✅ Step 5: If no special request or no matching gate found, find a normal gate
                if (assignedGate == null)
                {
                    assignedGate = availableGates.FirstOrDefault(g => !g.SupportsCFFT && !g.SupportsDDJB && !g.SupportsLWTT);
                }

                // ✅ Step 6: Assign flight to gate if available
                if (assignedGate != null)
                {
                    assignedGate.AssignFlight(flight); // ✅ Correctly assigning flight to gate
                    availableGates.Remove(assignedGate);
                    assignedFlights++;
                    assignedGates++;

                    // ✅ Step 7: Display assigned flight details
                    Console.WriteLine(string.Format("{0,-12} {1,-20} {2,-20} {3,-20} {4,-18} {5,-12} {6,-15}",
                        flight.FlightNumber,
                        terminal.Airlines.ContainsKey(flight.FlightNumber.Split(' ')[0]) ? terminal.Airlines[flight.FlightNumber.Split(' ')[0]].Name : "Unknown",
                        flight.Origin, flight.Destination,
                        flight.ExpectedTime.ToString("dd/MM/yyyy HH:mm"),
                        hasSpecialRequest ? "Special Request" : "None",
                        assignedGate.GateName));
                }
            }

            // ✅ Step 8: Display summary of assignments
            Console.WriteLine($"\nTotal flights assigned: {assignedFlights}");
            Console.WriteLine($"Total boarding gates assigned: {assignedGates}");

            // ✅ Step 9: Calculate percentage of flights/gates assigned automatically
            int totalFlights = terminal.Airlines.Values.Sum(a => a.Flights.Count);
            int totalGates = terminal.BoardingGates.Count;

            double flightAssignPercentage = (totalFlights > 0) ? ((double)assignedFlights / totalFlights) * 100 : 0;
            double gateAssignPercentage = (totalGates > 0) ? ((double)assignedGates / totalGates) * 100 : 0;

            Console.WriteLine($"Flights processed automatically: {flightAssignPercentage:F2}%");
            Console.WriteLine($"Boarding gates processed automatically: {gateAssignPercentage:F2}%");
        }


        static void DisplayTotalFeesPerAirline()
        {
            Dictionary<string, double> airlineFees = new Dictionary<string, double>();
            double totalFees = 0, totalDiscounts = 0;

            // Ensure all flights have assigned boarding gates
            if (terminal.Airlines.Values.SelectMany(a => a.Flights.Values).Any(f => !terminal.BoardingGates.Values.Any(g => g.AssignedFlight == f)))
            {
                Console.WriteLine("Ensure all flights have been assigned boarding gates before running this feature.");
                return;
            }

            foreach (var airline in terminal.Airlines.Values)
            {
                double airlineTotal = 0, airlineDiscount = 0;

                // ✅ Retrieve flights for this airline
                List<Flight> airlineFlights = airline.Flights.Values.ToList();

                foreach (var flight in airlineFlights)
                {
                    double flightFee = flight.CalculateFees(); // ✅ Use flight's CalculateFees()
                    airlineTotal += flightFee;
                }

                // ✅ Apply discount logic (e.g., 10% off for > 10 flights)
                if (airlineFlights.Count > 10)
                {
                    airlineDiscount = airlineTotal * 0.10;
                }

                airlineFees[airline.Name] = airlineTotal - airlineDiscount;
                totalFees += airlineTotal;
                totalDiscounts += airlineDiscount;
            }

            // ✅ Properly formatted output
            Console.WriteLine("\n--- Airline Fee Breakdown ---");
            foreach (var entry in airlineFees)
            {
                Console.WriteLine($"\nAirline: {entry.Key}"); // ✅ Shows the airline name separately
                if (entry.Value > 0)
                {
                    Console.WriteLine($"  → Total Flight Fees: ${entry.Value:F2}"); // ✅ Only shows fees if > 0
                }
                else
                {
                    Console.WriteLine("  → No flights were charged fees.");
                }
            }

            Console.WriteLine($"\nTotal Fees from All Flights: ${totalFees:F2}");
            Console.WriteLine($"Total Discounts Applied: ${totalDiscounts:F2}");
            Console.WriteLine($"Final Amount Terminal Collects: ${totalFees - totalDiscounts:F2}");
            Console.WriteLine($"Discount Percentage: {(totalDiscounts / (totalFees == 0 ? 1 : totalFees)) * 100:F2}%");
        }


    }
}


