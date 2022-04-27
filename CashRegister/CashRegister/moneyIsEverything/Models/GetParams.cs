using System;
using System.Collections.Generic;
using System.Text;

namespace CashRegister.moneyIsEverything.models
{
    public abstract class GetParams
    {
        public string GetParamsString()
        {
            string ret = "?";
            foreach (var property in this.GetType().GetProperties())
            {
                if (property.GetValue(this) != null)
                {
                    ret += property.Name + "=" + property.GetValue(this).ToString() + "&";
                }
            }
            // remove last &
            if (ret.Length > 1)
            {
                ret = ret.Substring(0, ret.Length - 1);
            }
            return ret;
        }
    }
}
