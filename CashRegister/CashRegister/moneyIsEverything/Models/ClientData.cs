
namespace CashRegister.moneyIsEverything.models
{
    public class ClientData : GetParams
    {
        public string encryptedCardNumber { get; set; }
        public string encryptedExpiryMonth { get; set; }
        public string encryptedExpiryYear { get; set; }
        public string encryptedSecurityCode { get; set; }
        public long amountValue { get; set; }
        public string amountCurrency { get; set; }
        public string reference { get; set; }
        public string ClientApiKey { get; set; }
        
    }
}
