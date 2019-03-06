using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAN_Validator.Exceptions
{
    public class TooShortNumberException : Exception
    {
        public TooShortNumberException(string message)
            : base(message)
        {

        }

    }
}
