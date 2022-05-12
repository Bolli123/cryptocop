using System.Collections.Generic;

namespace Cryptocop.Software.API.Models.Entities
{
    public class PaymentCard
    {
        public int Id {get; set;}
        public int UserId {get; set;}
        public string CardholderName {get; set;}
        public string CardNumber {get; set;}
        public int Month {get; set;}
        public int Year {get; set;}
        // Navigation properties
        public User User {get; set;}
    }
}