using Adyen;
using System;
using System.Collections.Generic;
using System.Text;

namespace CashRegister.moneyIsEverything
{
    // ICI
    // https://docs.adyen.com/get-started-with-adyen
    /**
     * class allowing online payements using TWINT payment gateway
     */
    internal class PayoutManager
    {
        // singleton
        private static PayoutManager instance = null;
        private static Client client = null;
        // secret storage   
        // https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows&viewFallbackFrom=aspnetcore-2.2#access-a-secret
        // https://docs.microsoft.com/en-us/xamarin/essentials/secure-storage?WT.mc_id=docs-ch9-jamont&tabs=ios
        // TODO: secure safe the API_KEY
        private const String API_KEY = "";
        public static PayoutManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PayoutManager();
                }
                return instance;
            }
        }

        private PayoutManager()
        {
            //client = new Client(API_KEY, Adyen.Model.Enum.Environment.Test);
        }
    }
}
