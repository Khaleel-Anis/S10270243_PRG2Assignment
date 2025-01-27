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
    internal abstract class Flight
    {
        public string FlightNumber { get; }
        public Airline Airline { get; }
        public string Origin { get; }
        public string Destination { get; }
        public DateTime ExpectedTime { get; }
        public string Status { get; private set; }
        public BoardingGate AssignedGate { get; private set; }

        protected Flight(string flightNumber, Airline airline, string origin, string destination, DateTime expectedTime)
        {
            FlightNumber = flightNumber;
            Airline = airline ?? new Airline("N/A", "Unknown Airline");
            Origin = origin;
            Destination = destination;
            ExpectedTime = expectedTime;
            Status = "Scheduled";
        }

        public void AssignGate(BoardingGate gate)
        {
            if (AssignedGate == null)
            {
                AssignedGate = gate;
                gate.AssignFlight(this);
            }
            else
            {
                Console.WriteLine($"Error: Flight {FlightNumber} is already assigned to Gate {AssignedGate.GateName}.");
            }
        }

        public void UpdateStatus(string newStatus)
        {
            Status = newStatus;
        }

        public virtual double CalculateFees()
        {
            double totalFee = 0;
            if (Destination == "SIN")
                totalFee += 500;
            else if (Origin == "SIN")
                totalFee += 800;
            totalFee += 300;
            return totalFee;
        }

        public override string ToString()
        {
            return string.Format("{0,-12} {1,-20} {2,-20} {3,-20} {4,-25} {5,-15} {6,-10}",
                FlightNumber, Airline.Name, Origin, Destination, ExpectedTime.ToString("dd/MM/yyyy hh:mm tt"), Status,
                AssignedGate != null ? AssignedGate.GateName : "None");
        }
    }
}