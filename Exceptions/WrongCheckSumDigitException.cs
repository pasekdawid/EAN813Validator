using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAN_Validator
{
    public class WrongCheckSumDigitException : Exception
    {
        public WrongCheckSumDigitException(string message)
            : base(message)
        {

        }
    }
}
