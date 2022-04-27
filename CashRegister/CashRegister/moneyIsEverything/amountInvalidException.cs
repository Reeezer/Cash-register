using System;
using System.Runtime.Serialization;

namespace CashRegister.moneyIsEverything
{
    [Serializable]
    internal class amountInvalidException : Exception
    {
        public amountInvalidException()
        {
        }

        public amountInvalidException(string message) : base(message)
        {
        }

        public amountInvalidException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected amountInvalidException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}