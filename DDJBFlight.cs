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
    internal class DDJBFlight : Flight
    {
        private const double RequestFee = 300;

        public DDJBFlight(string flightNumber, Airline airline, string origin, string destination, DateTime expectedTime)
            : base(flightNumber, airline, origin, destination, expectedTime) { }

        public override double CalculateFees()
        {
            return base.CalculateFees() + RequestFee;
        }   
    }
}
