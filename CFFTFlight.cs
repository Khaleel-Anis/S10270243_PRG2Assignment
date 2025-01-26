using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG_2_Assignment
{
    internal class CFFTFlight : Flight
    {
        private const double RequestFee = 150;

        public CFFTFlight(string flightNumber, Airline airline, string origin, string destination, DateTime expectedTime)
            : base(flightNumber, airline, origin, destination, expectedTime) { }

        public override double CalculateFees()
        {
            return base.CalculateFees() + RequestFee;
        }
    }
}
