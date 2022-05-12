using System;

namespace Cryptocop.Software.API.Repositories.Helpers
{
    public class PaymentCardHelper
    {
        public static string MaskPaymentCard(string paymentCardNumber)
        {
            var str = paymentCardNumber.Substring(paymentCardNumber.Length - 4);
            return "****-****-****-" + str;
        }
    }
}