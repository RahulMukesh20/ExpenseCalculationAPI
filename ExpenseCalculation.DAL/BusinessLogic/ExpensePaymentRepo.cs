using ExpenseCalculation.DAL.Models;

namespace ExpenseCalculation.DAL.BusinessLogic
{
    public class ExpensePaymentRepo
    {
        ExpenseCalcContext calcContext;

        public ExpensePaymentRepo()
        {
            calcContext = new ExpenseCalcContext();
        }

        /// <summary>
        /// Get All the Payment History
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<PaymentDetail> GetPaymentDetails(int groupId)
        {
            List<PaymentDetail> payments = new List<PaymentDetail>();

            try
            {
                payments = calcContext.PaymentDetails.Where(x => x.GroupId == groupId).ToList();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return payments;
        }

        /// <summary>
        /// Add Payment Transaction
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        public bool AddPayment(Payment payment)
        {
            bool status = false;
            try
            {
                PaymentDetail paymentDetail = new PaymentDetail();
                string memberName = "", groupName = "", categoryName = "";

                memberName = calcContext.Members.Where(x => x.GroupId == payment.GroupId && x.MemberId == payment.MemberId).Select(x => x.MemberName).FirstOrDefault();
                groupName = calcContext.TripGroups.Where(x => x.GroupId == payment.GroupId).Select(x => x.GroupName).FirstOrDefault();
                categoryName = calcContext.Categories.Where(x => x.CategoryId == payment.CategoryId).Select(x => x.CategoryName).FirstOrDefault();

                paymentDetail.GroupId = payment.GroupId;
                paymentDetail.GroupName = groupName;
                paymentDetail.MemberId = payment.MemberId;
                paymentDetail.MemberName = memberName;
                paymentDetail.CategoryId = payment.CategoryId;
                paymentDetail.CategoryName = categoryName;
                paymentDetail.PaymentDate = Convert.ToDateTime(payment.PaymentDate);
                paymentDetail.PaymentType = payment.PaymentType;
                paymentDetail.Amount = Convert.ToDecimal(payment.Amount);
                paymentDetail.Notes = payment.Notes;

                calcContext.PaymentDetails.Add(paymentDetail);
                calcContext.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return status;
        }

        /// <summary>
        /// Delete Payment 
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        public bool DeletePayment(int paymentId)
        {
            bool status = false;
            try
            {
                PaymentDetail paymentDetail = new PaymentDetail();
                paymentDetail = calcContext.PaymentDetails.Where(x => x.PaymentId == paymentId).FirstOrDefault();
                calcContext.Remove(paymentDetail);
                calcContext.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return status;
        }

        /// <summary>
        /// Get total amount based on cash type
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<PaymentType> GetAmountCollected(int groupId)
        {
            List<PaymentType> payments = new List<PaymentType>();
            try
            {

                List<PaymentDetail> paymentDetails = new List<PaymentDetail>();
                paymentDetails = GetPaymentDetails(groupId);

                var amtCollected = paymentDetails.Where(x => x.MemberName != "TripAdvisor" && (x.PaymentType == "CASH" || x.PaymentType == "UPI")).GroupBy(g => g.PaymentType).Select(x => new { Id = x.Key, Value = x.Sum(x => x.Amount) }).OrderBy(x => x.Id).ToList();

                var cashAmt = amtCollected.Where(x => x.Id == "CASH").Select(x => x.Value)?.FirstOrDefault();

                PaymentType cashType = new PaymentType();
                cashType.Type = "CASH";
                cashType.Amount = cashAmt != null ? Decimal.ToInt64((decimal)cashAmt) : 0;
                payments.Add(cashType);

                var upiAmt = amtCollected.Where(x => x.Id == "UPI").Select(x => x.Value)?.FirstOrDefault();

                PaymentType upiType = new PaymentType();
                upiType.Type = "UPI";
                upiType.Amount = upiAmt != null ? Decimal.ToInt64((decimal)upiAmt) : 0;
                payments.Add(upiType);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return payments;
        }

        /// <summary>
        /// Trip Advisor Transaction details
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<PaymentType> AdivisorPaymentDetails(int groupId)
        {
            List<PaymentType> payments = new List<PaymentType>();
            try
            {
                List<PaymentDetail> paymentDetails = new List<PaymentDetail>();
                paymentDetails = GetPaymentDetails(groupId);

                var amtCollected = paymentDetails.Where(x => x.MemberName == "TripAdvisor" && (x.PaymentType == "CASH" || x.PaymentType == "UPI")).GroupBy(g => g.PaymentType).Select(x => new { Id = x.Key, Value = x.Sum(x => x.Amount) }).OrderBy(x => x.Id).ToList();
                var amtSpent = paymentDetails.Where(x => x.CategoryName == "Advance" && (x.PaymentType == "CASH" || x.PaymentType == "UPI")).GroupBy(g => g.PaymentType).Select(x => new { Id = x.Key, Value = x.Sum(x => x.Amount) }).OrderBy(x => x.Id).ToList();

                var cashAmt = amtCollected.Where(x => x.Id == "CASH").Select(x => x.Value)?.FirstOrDefault();
                PaymentType cashTypeCollected = new PaymentType();
                cashTypeCollected.Type = "CASH - Collected";
                cashTypeCollected.Amount = cashAmt != null ? Decimal.ToInt64((decimal)cashAmt) : 0;
                payments.Add(cashTypeCollected);

                var upiAmt = amtCollected.Where(x => x.Id == "UPI").Select(x => x.Value)?.FirstOrDefault();
                PaymentType upiTypeCollected = new PaymentType();
                upiTypeCollected.Type = "UPI - Collected";
                upiTypeCollected.Amount = upiAmt != null ? Decimal.ToInt64((decimal)upiAmt) : 0;
                payments.Add(upiTypeCollected);


                var cashSpent = amtSpent.Where(x => x.Id == "CASH").Select(x => x.Value)?.FirstOrDefault();
                PaymentType cashTypeSpent = new PaymentType();
                cashTypeSpent.Type = "CASH - Spent";
                cashTypeSpent.Amount = cashSpent != null ? Decimal.ToInt64((decimal)cashSpent) : 0;
                payments.Add(cashTypeSpent);

                var upiSpent = amtSpent.Where(x => x.Id == "UPI").Select(x => x.Value)?.FirstOrDefault();
                PaymentType upiTypeSpent = new PaymentType();
                upiTypeSpent.Type = "UPI - Spent";
                upiTypeSpent.Amount = upiSpent != null ? Decimal.ToInt64((decimal)upiSpent) : 0;
                payments.Add(upiTypeSpent);

                AddBalance(cashTypeCollected.Amount, upiTypeCollected.Amount, cashTypeSpent.Amount, upiTypeSpent.Amount, payments);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return payments;
        }

        /// <summary>
        /// Trip Adisor Balance Amount
        /// </summary>
        /// <param name="cashCollected"></param>
        /// <param name="upiCollected"></param>
        /// <param name="cashSpent"></param>
        /// <param name="upiSpent"></param>
        /// <param name="payments"></param>
        public void AddBalance(long cashCollected, long upiCollected, long cashSpent, long upiSpent, List<PaymentType> payments)
        {
            long balanceCash = 0, balanceUpi = 0;
            try
            {
                balanceCash = cashCollected - cashSpent;
                balanceUpi = upiCollected - upiSpent;

                PaymentType cashTypeBalance = new PaymentType();
                cashTypeBalance.Type = "CASH - Balance";
                cashTypeBalance.Amount = balanceCash;
                payments.Add(cashTypeBalance);

                PaymentType upiTypeBalance = new PaymentType();
                upiTypeBalance.Type = "UPI - Balance";
                upiTypeBalance.Amount = balanceUpi;
                payments.Add(upiTypeBalance);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
