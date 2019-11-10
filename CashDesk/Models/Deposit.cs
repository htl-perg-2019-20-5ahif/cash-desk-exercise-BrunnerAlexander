using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CashDesk.Models
{
    public class Deposit : IDeposit
    {
        public int DepositId { get; set; }

        [Required]
        public Membership Membership { get; set; }

        private decimal amount = Decimal.MaxValue;
        [Required]
        public decimal Amount
        {
            get { return amount; }
            set
            {
                if(value <= 0)
                {
                    throw new ArgumentException("The value must be greater than 0");
                }
                amount = value;
            }
        }

        IMembership IDeposit.Membership => Membership;
    }
}
