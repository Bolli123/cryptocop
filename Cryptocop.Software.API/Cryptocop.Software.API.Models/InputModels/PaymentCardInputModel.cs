using System.ComponentModel.DataAnnotations;

namespace Cryptocop.Software.API.Models.InputModels
{
    public class PaymentCardInputModel 
    {
        [Required]
        [MinLength(3)]
        public string CardholderName { get; set; }
        [Required]
        [CreditCard]
        public string CardNumber { get; set; }
        [Range(1, 12, 
            ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Month { get; set; }
        [Range(0, 99, 
            ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Year { get; set; }
    }
}