using System;
using System.Collections.Generic;

namespace ExpenseCalculation.DAL.Models
{
    public partial class Member
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public int GroupId { get; set; }
    }
}
