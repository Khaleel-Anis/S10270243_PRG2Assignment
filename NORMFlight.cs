using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG_2_Assignment
{
    internal class NORMFlight : Flight
    {
        public NORMFlight(string flightNumber, Airline airline, string origin, string destination, DateTime expectedTime)
           : base(flightNumber, airline, origin, destination, expectedTime) { }

        public override double CalculateFees()
        {
            double totalFee = base.CalculateFees();

            // Apply Discounts
            if (ExpectedTime.Hour < 11 || ExpectedTime.Hour >= 21)
                totalFee -= 110;

            if (Origin == "DXB" || Origin == "BKK" || Origin == "NRT")
                totalFee -= 25;

            totalFee -= 50; // No special request discount

            return totalFee;
        }
    }
}
