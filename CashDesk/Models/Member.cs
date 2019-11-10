using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CashDesk.Models
{
    public class Member : IMember
    {
        [Key]
        public int MemberNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        public DateTime Birthday { get; set; }

        // Foreign Key
        public List<Membership> Memberships { get; set; }
    }
}
