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
    public class CategoryController : ControllerBase
    {
        ExpenseCategoryRepo repo;

        public CategoryController()
        {
            repo = new ExpenseCategoryRepo();
        }

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            List<Category> categories = new List<Category>();
            try
            {
                categories = repo.GetCategories();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(categories);
        }

        [HttpPost]
        public IActionResult AddCategory([FromForm] string categoryName)
        {
            string result = "";
            try
            {
                result = repo.AddCategory(categoryName);
            }
            catch (Exception ex)
            {
                result = "Failed - " + ex.Message;
            }
            return Ok(result);
        }

        [HttpDelete]
        public IActionResult DeleteCategory(int id)
        {
            bool status = false;
            string message = "";
            try
            {
                status = repo.DeleteCategory(id);
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

        [HttpGet]
        public IActionResult GetAmountCategory(int id)
        {
            List<AmountSpentCategory> amountSpents = new List<AmountSpentCategory>();
            try
            {
                amountSpents = repo.GetAmountSpentsCategorywise(id);
            }
            catch(Exception ex) 
            { 
                return BadRequest(ex.Message);
            }
            return Ok(amountSpents);
        }
    }
}
