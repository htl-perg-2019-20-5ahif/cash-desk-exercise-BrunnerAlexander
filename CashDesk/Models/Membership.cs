using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CashDesk.Models
{
    public class Membership : IMembership
    {
        public int MembershipId { get; set; }

        [Required]
        public Member Member { get; set; }

        [Required]
        public DateTime Begin { get; set; }

        private DateTime end = DateTime.MaxValue;
        public DateTime End
        {
            get { return end; }
            set
            {
                if(value < Begin)
                {
                    throw new ArgumentException("End must be > than Begin");
                }

                end = value;
            }
        }

        // Foreign Keys
        IMember IMembership.Member => Member;

        public List<Deposit> Deposits { get; set; }
    }
}
