using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DicePokerAppV2.Exceptions
{
    public class PokerException : Exception
    {
        public PokerException()
        {
        }

        public PokerException(string? message) : base(message)
        {
        }

        public PokerException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected PokerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
