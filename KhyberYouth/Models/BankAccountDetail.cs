using System.ComponentModel.DataAnnotations;

namespace KhyberYouth.Models
{
    public class BankAccountDetail
    {
        public int Id { get; set; }
        [Display(Name = "Account Title")]
        public string AccountTitle { get; set; }

        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; }

        [Display(Name = "IBAN Number")]
        public string IbanNumber { get; set; }

        [Display(Name = "Currency Type")]
        public string CurrencyType { get; set; }

        [Display(Name = "Bank Name")]
        public string BankName { get; set; }

        [Display(Name = "Branch Code")]
        public string BranchCode { get; set; }

        [Display(Name = "Swift Code")]
        public string SwiftCode { get; set; }

        [Display(Name = "Bank Address")]
        public string BankAddress { get; set; }

        [Display(Name = "Contact Phone Number")]
        public string ContactPhoneNumber { get; set; }
    }
}
