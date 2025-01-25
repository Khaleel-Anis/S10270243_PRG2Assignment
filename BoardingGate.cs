using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// BoardingGate.cs
namespace PRG_2_Assignment
{
    internal class BoardingGate
    {
        public string GateName { get; }
        public bool SupportsCFFT { get; }
        public bool SupportsDDJB { get; }
        public bool SupportsLWTT { get; }
        public Flight AssignedFlight { get; private set; }

        public BoardingGate(string gateName, bool supportsCFFT, bool supportsDDJB, bool supportsLWTT)
        {
            GateName = gateName;
            SupportsCFFT = supportsCFFT;
            SupportsDDJB = supportsDDJB;
            SupportsLWTT = supportsLWTT;
        }

        public double CalculateFees()
        {
            double gateBaseFee = 300; // Every gate has a base fee
            return gateBaseFee + (AssignedFlight?.CalculateFees() ?? 0);
        }

        public void AssignFlight(Flight flight)
        {
            if (AssignedFlight == null)
            {
                AssignedFlight = flight;
            }
            else
            {
                Console.WriteLine($"Error: Boarding Gate {GateName} is already assigned to Flight {AssignedFlight.FlightNumber}.");
            }
        }

        public void RemoveAssignedFlight()
        {
            AssignedFlight = null;
        }

        public override string ToString()
        {
            return string.Format("{0,-10} {1,-10} {2,-10} {3,-10}", GateName, SupportsDDJB, SupportsCFFT, SupportsLWTT);
        }
    }
}