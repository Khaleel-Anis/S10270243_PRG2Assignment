//==========================================================
// Student Number : S10270243
// Student Number : S10269256
// Student Name : Khaleel Anis
// Partner Name : Hia Wei Dai
//==========================================================

namespace PRG_2_Assignment
{
    internal class Program
    {
        static void Main(string[] args)
        {
            static void Main(string[] args)
            {
                Terminal terminal = new Terminal("Changi Airport Terminal 5");

                while (true)
                {
                    Console.WriteLine("\n=============================================");
                    Console.WriteLine("Welcome to Changi Airport Terminal 5");
                    Console.WriteLine("=============================================");
                    Console.WriteLine("1. List All Flights");
                    Console.WriteLine("0. Exit");
                    Console.Write("\nPlease select your option: ");

                    string option = Console.ReadLine();

                    switch (option)
                    {
                        case "1":
                            ListAllFlights(terminal);
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

            static void ListAllFlights(Terminal terminal)
            {
                Console.WriteLine("\n=============================================");
                Console.WriteLine("List of Flights for Changi Airport Terminal 5");
                Console.WriteLine("=============================================");
                Console.WriteLine(string.Format("{0,-12} {1,-20} {2,-20} {3,-20} {4,-25}", "Flight No.", "Airline Name", "Origin", "Destination", "Expected Time"));

                foreach (var flight in terminal.Flights.Values)
                {
                    Console.WriteLine(flight.ToString());
                }
            }
        }
    }
}
