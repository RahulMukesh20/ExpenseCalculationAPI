using ExpenseCalculation.DAL.Models;

namespace ExpenseCalculation.DAL.BusinessLogic
{
    public class ExpenseGroupRepo
    {
        public ExpenseCalcContext calcContext;

        public ExpenseGroupRepo()
        {
            calcContext = new ExpenseCalcContext();
        }

        /// <summary>
        /// Get All Groups 
        /// </summary>
        /// <returns></returns>
        public List<TripGroup> GetGroupDetails()
        {
            List<TripGroup> groups = new List<TripGroup>();
            try
            {
                groups = calcContext.TripGroups.Distinct().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return groups;
        }

        /// <summary>
        /// Add Group
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public string AddTrip(string groupName)
        {
            string result = "";
            try
            {
                int isUnique = 0, groupId = 0, count = 0;
                Member newMember = new Member();

                count = calcContext.TripGroups.Select(x => x.GroupId).Count();

                if (count > 0)
                {
                    groupId = calcContext.TripGroups.Select(x => x.GroupId).Max() + 1;
                    isUnique = calcContext.TripGroups.Where(x => x.GroupName == groupName).Count();

                    if (isUnique == 0)
                    {
                        result = CreateGroup(groupId, groupName);
                    }
                    else
                    {
                        result = "Failed - Group Name is already present";
                    }
                }
                else
                {
                    groupId = 1;
                    result = CreateGroup(groupId, groupName);
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
        /// Create Group and add TripAdvisor by default
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public string CreateGroup(int groupId, string groupName)
        {
            string result = "";
            try
            {
                ExpenseMemberRepo repo = new ExpenseMemberRepo();
                TripGroup group = new TripGroup();
                group.GroupId = groupId;
                group.GroupName = groupName;
                calcContext.TripGroups.Add(group);
                calcContext.SaveChanges();

                repo.CreateFriend(1, "TripAdvisor", groupId);

                result = "Success - Group Name Added Successfully";
            }
            catch (Exception ex)
            {
                result = "Failed - " + ex.Message;
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Delete the Group
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public bool DeleteGroup(int groupId)
        {
            bool status = false;
            try
            {
                TripGroup group = new TripGroup();
                List<Member> member = new List<Member>();
                List<PaymentDetail> paymentDetail = new List<PaymentDetail>();

                group = calcContext.TripGroups.Where(x => x.GroupId == groupId).FirstOrDefault();
                member = calcContext.Members.Where(x => x.GroupId == groupId).ToList();
                paymentDetail = calcContext.PaymentDetails.Where(x => x.GroupId == groupId).ToList();
                calcContext.TripGroups.Remove(group);
                calcContext.Members.RemoveRange(member);
                calcContext.PaymentDetails.RemoveRange(paymentDetail);
                calcContext.SaveChanges(status);
                status = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return status;
        }

        /// <summary>
        /// Update Grouup Share Price
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool UpdateGroupAmount(int groupId, decimal amount)
        {
            bool status = false;
            try
            {
                TripGroup trip = new TripGroup();
                trip = calcContext.TripGroups.Find(groupId);
                if (trip != null)
                {
                    {
                        trip.GroupShare = amount;
                        calcContext.TripGroups.Update(trip);
                        calcContext.SaveChanges();
                        status = true;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return status;
        }
    }
}
