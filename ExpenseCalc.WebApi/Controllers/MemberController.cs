using ExpenseCalculation.DAL;
using ExpenseCalculation.DAL.BusinessLogic;
using ExpenseCalculation.DAL.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseCalc.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [EnableCors("AllowOrigins")]
    public class MemberController : ControllerBase
    {
        ExpenseMemberRepo repo;

        public MemberController()
        {
            repo = new ExpenseMemberRepo();
        }
        
        [HttpGet]
        public IActionResult GetAllFriends(int id)
        {
            List<Member> members = new List<Member>();
            try
            {
                members = repo.GetAllMemebers(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(members);
        }

        [HttpGet]
        public IActionResult GetTripAdvisor(int groupId)
        {
            Member member = new Member();
            try
            {
                member = repo.GetTripAdvisor(groupId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(member);
        }

        [HttpPost]
        public IActionResult AddFriend([FromForm]int groupId, [FromForm] string friendName)
        {
            string message = "";
            try
            {
                message = repo.AddMember(groupId, friendName);
            }
            catch (Exception ex) 
            { 
                message = ex.Message;
            }
            return Ok(message);
        }

        [HttpDelete]
        public IActionResult DeleteFriend(int id)
        {
            bool status = false;
            string message = "";
            try
            {
                status = repo.DeleteMember(id);
                if (status)
                {
                    message = "Deleted Successfully";
                }
                else
                {
                    message = "Unknown error: Delete is not successfull";
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return Ok(message); 
        }

        [HttpGet]
        public IActionResult GetAmountSpentbyMembers(int id)
        {
            List<AmountSpentMemeber> amountSpents = new List<AmountSpentMemeber>();
            try
            {
                amountSpents = repo.GetAmountSpentByMember(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(amountSpents);
        }

        [HttpGet]
        public IActionResult GetMemberPaymentDetails(int groupId, int memberId)
        {
            List<PaymentDetail> paymentDetails = new List<PaymentDetail>();
            try
            {
                paymentDetails = repo.GetPaymentDetailsByMemberId(groupId, memberId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(paymentDetails);
        }
    }
}
