using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace file_signature_gen.exceptions
{
    public class InvalidValueException : Exception
    {
        public InvalidValueException()
        {
        }

        public InvalidValueException(string message)
            : base(message)
        {
        }

        public InvalidValueException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
