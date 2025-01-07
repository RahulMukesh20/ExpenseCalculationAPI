using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseCalculation.DAL
{
    public class AmountSpentMemeber
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public decimal AmountSpent { get; set; }
    }
}
