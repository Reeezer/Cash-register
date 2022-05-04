using Adyen.Model;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace CashRegister.moneyIsEverything.models
{
    /// <summary>
    /// model representing the data the the cashregister server responds to the client
    /// </summary>
    public class ServerData
    {
        public string PayoutId { get; set; }
        public string PayoutStatus { get; set; }
        public string MerchantAccount { get; set; }
        public long AmountValue { get; set; }
        public string AmountCurrency { get; set; }
        public string Reference { get; set; }
        public string ClientApiKey { get; set; }
        
        override public string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("[" + this.GetType().Name + "]");
            sb.AppendLine("PayoutId: " + this.PayoutId);
            sb.AppendLine("PayoutStatus: " + this.PayoutStatus);
            sb.AppendLine("MerchantAccount: " + this.MerchantAccount);
            sb.AppendLine("AmountValue: " + this.AmountValue);
            sb.AppendLine("AmountCurrency: " + this.AmountCurrency);
            sb.AppendLine("Reference: " + this.Reference);
            sb.AppendLine("ClientApiKey: " + this.ClientApiKey);

            return sb.ToString();
        }
        static public ServerData CreateFromJsonString(string json)
        {
            ServerData s = JsonSerializer.Deserialize<ServerData>(json);
            return s;
        }
        /**
         * return the amount as Adyen Amout
         */
        public Amount GetAmout()
        {
            return new Amount(AmountCurrency, AmountValue);
        }
    }
}
