using System;
using System.Collections.Generic;

namespace ExpenseCalculation.DAL.Models
{
    public partial class PaymentDetail
    {
        public int PaymentId { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int? MemberId { get; set; }
        public string MemberName { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string PaymentType { get; set; }
        public decimal? Amount { get; set; }
        public string Notes { get; set; }
    }
}
