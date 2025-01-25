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

        public override string ToString()
        {
            return $"{GateName,-5} {SupportsDDJB,-8} {SupportsCFFT,-8} {SupportsLWTT,-8}";
        }
    }
}
