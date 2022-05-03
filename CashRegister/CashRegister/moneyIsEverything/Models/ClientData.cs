﻿
namespace CashRegister.moneyIsEverything.models
{
    public class ClientData : GetParams
    {
        public string EncryptedCardNumber { get; set; }
        public string EncryptedExpiryMonth { get; set; }
        public string EncryptedExpiryYear { get; set; }
        public string EncryptedSecurityCode { get; set; }
        public long AmountValue { get; set; }
        public string AmountCurrency { get; set; }
        public string Reference { get; set; }
        public string ClientApiKey { get; set; }
        
    }
}
