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

namespace S10270243_PRG2Assignment
{
    internal class CFFTFlight : Flight
    {
        private const double RequestFee = 150;

        public CFFTFlight(string flightNumber, string origin, string destination, DateTime expectedTime)
            : base(flightNumber, origin, destination, expectedTime, "Scheduled") { }

        public override double CalculateFees()
        {
            double fee = base.CalculateFees() + RequestFee;

            // Apply Discounts
            if (ExpectedTime.Hour < 11 || ExpectedTime.Hour >= 21)
                fee -= 110; // Early/Late Discount

            if (Origin == "DXB" || Origin == "BKK" || Origin == "NRT")
                fee -= 25; // Specific Airport Discount

            fee *= 0.97; // Apply 3% discount last as per assignment v3

            return fee > 0 ? fee : 0; // Ensure no negative fees
        }
    }
}
