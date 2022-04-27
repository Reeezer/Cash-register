using System.Runtime.Serialization;

namespace CashRegisterServer.Models.Exceptions
{
    [Serializable]
    public class CustomException500 : Exception
    {
        public CustomException500(string? message) : base(message)
        {
        }

        public CustomException500(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CustomException500(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
