using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG_2_Assignment
{
    internal class NORMFlight
    {
        public NORMFlight(string flightNumber, Airline airline, string origin, string destination, DateTime expectedTime)
       : base(flightNumber, airline, origin, destination, expectedTime) { }

        public override double CalculateFees()
        {
            return 800;
        }
    }
}
