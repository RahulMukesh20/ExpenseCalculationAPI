namespace ExpenseCalculation.DAL
{
    public class Payment
    {
        public int CategoryId { get; set; }
        public int MemberId { get; set; }
        public int GroupId { get; set; }
        public string PaymentDate { get; set; }
        public string PaymentType { get; set; }
        public long Amount { get; set; }
        public string Notes { get; set; }
    }
}
