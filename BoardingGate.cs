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
    internal class BoardingGate
    {
        private string gateName;
        private bool supportsCFFT;
        private bool supportsDDJB;
        private bool supportsLWTT;
        private Flight assignedFlight;

        public string GateName { get { return gateName; } }
        public bool SupportsCFFT { get { return supportsCFFT; } }
        public bool SupportsDDJB { get { return supportsDDJB; } }
        public bool SupportsLWTT { get { return supportsLWTT; } }
        public Flight? AssignedFlight { get { return assignedFlight; } }

        public BoardingGate(string gateName, bool supportsCFFT, bool supportsDDJB, bool supportsLWTT)
        {
            this.gateName = gateName;
            this.supportsCFFT = supportsCFFT;
            this.supportsDDJB = supportsDDJB;
            this.supportsLWTT = supportsLWTT;
            this.assignedFlight = null;
        }

        public bool AssignFlight(Flight flight)
        {
            if (assignedFlight != null)
            {
                Console.WriteLine($"Error: Boarding Gate {gateName} is already assigned to Flight {assignedFlight.FlightNumber}.");
                return false;
            }

            // Ensure gate supports flight type
            if ((flight is CFFTFlight && !supportsCFFT) ||
                (flight is DDJBFlight && !supportsDDJB) ||
                (flight is LWTTFlight && !supportsLWTT))
            {
                Console.WriteLine($"Error: Boarding Gate {gateName} does not support this flight type.");
                return false;
            }

            assignedFlight = flight;
            return true;
        }

        public override string ToString()
        {
            string assigned = assignedFlight != null ? assignedFlight.FlightNumber : "Unassigned";
            return string.Format("{0,-10} {1,-10} {2,-10} {3,-10} {4,-15}",
                gateName, supportsDDJB ? "Yes" : "No", supportsCFFT ? "Yes" : "No", supportsLWTT ? "Yes" : "No", assigned);
        }
    }
}