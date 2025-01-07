using ExpenseCalculation.DAL.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ExpenseCalculation.DAL.BusinessLogic
{
    public class ExpenseMemberRepo
    {
        public ExpenseCalcContext calcContext;

        public ExpenseMemberRepo()
        {
            calcContext = new ExpenseCalcContext();
        }


        /// <summary>
        /// Get All Friends based on Group Id
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<Member> GetAllMemebers(int groupId)
        {
            List<Member> members = new List<Member>();
            try
            {
                members = calcContext.Members.Where(x => x.GroupId == groupId && x.MemberName != "TripAdvisor").Distinct().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return members;
        }

        /// <summary>
        /// Add a Friend in the specified Group
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="memberName"></param>
        /// <returns></returns>
        public string AddMember(int groupId, string memberName)
        {
            string result = "";
            try
            {
                int isUnique = 0, memberId = 0, count = 0;
                Member newMember = new Member();

                count = calcContext.Members.Select(x => x.MemberId).Count();

                if (count > 0)
                {
                    memberId = calcContext.Members.Select(x => x.MemberId).Max() + 1;
                    isUnique = calcContext.Members.Where(x => x.GroupId == groupId && x.MemberName == memberName).Count();

                    if (isUnique == 0)
                    {
                        result = CreateFriend(memberId, memberName, groupId);
                    }
                    else
                    {
                        result = "Failed - Name is already present";
                    }
                }
                else
                {
                    memberId = 1;
                    result = CreateFriend(memberId, memberName, groupId);
                }
            }
            catch (Exception ex)
            {
                result = "Failed - " + ex.Message;
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Create a Friend in the Db
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="name"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public string CreateFriend(int memberId, string name, int groupId)
        {
            string result = "";
            try
            {
                Member newMember = new Member();
                newMember.MemberId = memberId;
                newMember.MemberName = name;
                newMember.GroupId = groupId;
                calcContext.Members.Add(newMember);
                calcContext.SaveChanges();
                result = "Success - Friend Name Added Successfully";
            }
            catch (Exception ex)
            {
                result = "Failed - " + ex.Message;
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Delete a Friend on the Specified Group
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public bool DeleteMember(int memberId)
        {
            bool status = false;
            try
            {
                Member member = new Member();
                List<PaymentDetail> payment = new List<PaymentDetail>();

                member = calcContext.Members.Where(x => x.MemberId == memberId).FirstOrDefault();
                payment = calcContext.PaymentDetails.Where(x => x.MemberId == memberId).ToList();

                if (member != null)
                {
                    calcContext.Members.Remove(member);
                    calcContext.PaymentDetails.RemoveRange(payment);
                    calcContext.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return status;
        }

        /// <summary>
        /// Get Total Amount Spent for each Member
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<AmountSpentMemeber> GetAmountSpentByMember(int groupId)
        {
            List<AmountSpentMemeber> amountPaidbyMembers = new List<AmountSpentMemeber>();
            try
            {
                DataSet ds = new DataSet();
                var connectionString = calcContext.Database.GetConnectionString();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand sqlComm = new SqlCommand("get_member_amountPaid", conn);
                    sqlComm.Parameters.AddWithValue("@groupId", groupId);

                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);
                }

                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        AmountSpentMemeber amount = new AmountSpentMemeber();
                        amount.MemberId = Convert.ToInt32(dr["member_id"]);
                        amount.MemberName = dr["member_Name"].ToString();
                        amount.AmountSpent = Convert.ToDecimal(dr["amount_spent"]);

                        amountPaidbyMembers.Add(amount);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return amountPaidbyMembers;
        }

        /// <summary>
        /// Get Transaction Details by member ID
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public List<PaymentDetail> GetPaymentDetailsByMemberId(int groupId, int memberId)
        {
            List<PaymentDetail> paymentDetails = new List<PaymentDetail>();
            try
            {
                paymentDetails = calcContext.PaymentDetails.Where(x => x.GroupId == groupId && x.MemberId == memberId).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return paymentDetails;
        }

        /// <summary>
        /// Get Trip Advisor Details
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public Member GetTripAdvisor(int groupId)
        {
            Member member = new Member();
            try
            {
                member = calcContext.Members.Where(x => x.GroupId == groupId && x.MemberName == "TripAdvisor").FirstOrDefault();
            }
            catch (Exception ex)
            {
                member = null;
                Console.WriteLine(ex.Message);
            }
            return member;
        }
    }
}
