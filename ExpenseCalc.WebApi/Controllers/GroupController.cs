using ExpenseCalculation.DAL.BusinessLogic;
using ExpenseCalculation.DAL.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseCalc.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [EnableCors("AllowOrigins")]
    public class GroupController : ControllerBase
    {
        ExpenseGroupRepo repo;

        public GroupController()
        {
            repo = new ExpenseGroupRepo();
        }

        [HttpGet]
        public IActionResult GetAllGroups()
        {
            List<TripGroup> tripGroups = new List<TripGroup>();
            try
            {
                tripGroups = repo.GetGroupDetails();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(tripGroups);
        }

        [HttpGet]
        public IActionResult GetGroupShareAmount(int id)
        {
            long amount = 0;
            try
            {
                amount = repo.GetGroupShare(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(amount);
        }

        [HttpPost]
        public IActionResult AddGroup([FromForm] string groupName)
        {
            string result = "";
            try
            {
                result = repo.AddTrip(groupName);
            }
            catch (Exception ex)
            {
                result = "Failed - " + ex.Message;
            }
            return Ok(result);
        }

        [HttpDelete]
        public IActionResult DeleteGroup(int id)
        {
            bool status = false;
            string message = "";
            try
            {
                status = repo.DeleteGroup(id);
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
                message = "Failed - " + ex.Message;
            }
            return Ok(message);
        }

        [HttpPut]
        public IActionResult UpdateGroupShare([FromForm] int groupId, [FromForm]long sharePrice)
        {
            bool status = false;
            string message = "";
            decimal price = 0;
            try
            {
                price = Convert.ToDecimal(sharePrice);
                status = repo.UpdateGroupAmount(groupId, price);
                if (status)
                {
                    message = "Success - Updated completed";
                }
                else
                {
                    message = "Failed - Unknown Error";
                }
            }
            catch (Exception ex)
            {
                message = "Failed - " + ex.Message;
            }
            return Ok(message);
        }
    }
}
