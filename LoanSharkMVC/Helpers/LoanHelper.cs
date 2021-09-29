using LoanSharkMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanSharkMVC.Helpers
{
    public class LoanHelper
    {
        public Loan GetPayments(Loan loan)
        {
            //Calculate monthly payment and set the Payment attribute of the loan object
            loan.Payment = CalcPayment(loan.Amount, loan.Rate, loan.Term);

            //Loop from 1 to the end of the term 
            var balance = loan.Amount; //Balance equals loan amount on first month
            var totalInterest = 0.0m; //Interest paid on the month, 0 on first month
            var monthlyInterest = 0.0m; //Same as above
            //Subtracting interest from payment tells us how much went to the principal to pay off the loan
            var monthlyPrincipal = 0.0m;
            var monthlyRate = CalcMonthlyRate(loan.Rate);

            for (int month = 1; month <= loan.Term; month++)
            {
                monthlyInterest = CalcMonthlyInterest(balance, monthlyRate);
                //Running total of interest, each month add to it
                totalInterest += monthlyInterest;
                monthlyPrincipal = loan.Payment - monthlyInterest;
                balance -= monthlyPrincipal;

                //Add to LoanPayment schedule
                LoanPayment loanPayment = new();

                loanPayment.Month = month;
                loanPayment.Payment = loan.Payment;
                loanPayment.MonthlyPrincipal = monthlyPrincipal;
                loanPayment.MonthlyInterest = monthlyInterest;
                loanPayment.TotalInterest = totalInterest;
                loanPayment.Balance = balance;

                //Push the loanPayment object into the loan model
                loan.Payments.Add(loanPayment);
            }

            //Push values to the properties of the loan model
            loan.TotalInterest = totalInterest;
            loan.TotalCost = loan.Amount + totalInterest; //What was borrowed plus total interest over the life of the loan

            return loan;
        }

        private decimal CalcPayment(decimal amount, decimal rate, int term)
        {
            //Let the compiler determine what the type is based on what's on the other side of the function, implicitly defined
            var monthlyRate = CalcMonthlyRate(rate); 
            var amountD = Convert.ToDouble(amount);
            var rateD = Convert.ToDouble(monthlyRate);

            var paymentD = (amountD * rateD) / (1 - Math.Pow(1 + rateD, -term));

            return Convert.ToDecimal(paymentD);
        }

        private decimal CalcMonthlyRate(decimal rate)
        {
            return rate / 1200;
        }

        private decimal CalcMonthlyInterest(decimal balance, decimal monthlyRate)
        {
            return balance * monthlyRate;
        }
    }
}
