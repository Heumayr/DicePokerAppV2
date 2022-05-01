using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DicePokerAppV2.Exceptions
{
    public class PokerValueException : PokerException
    {
        public PokerValueException()
        {
        }

        public PokerValueException(string? message) : base(message)
        {
        }

        public PokerValueException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected PokerValueException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
