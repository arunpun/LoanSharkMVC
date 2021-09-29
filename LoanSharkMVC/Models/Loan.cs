using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanSharkMVC.Models
{
    public class Loan
    {
        public decimal Amount { get; set; }
        public decimal Rate { get; set; }
        public int Term { get; set; } //In months
        public decimal Payment { get; set; } //Monthly payment
        public decimal TotalInterest { get; set; } //Interest over life of the loan
        public decimal TotalCost { get; set; } //Amount borrowed plus total interest

        public List<LoanPayment> Payments = new List<LoanPayment>(); //Will hold instance of loan payments, for a 12 month loan, it will hold 12 instances of loan payment

    }
}
