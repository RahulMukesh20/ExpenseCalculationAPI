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
    public class PaymentController : ControllerBase
    {
        ExpensePaymentRepo repo;

        public PaymentController()
        {
            repo = new ExpensePaymentRepo();
        }

        [HttpPost]
        public IActionResult AddPayment(Payment payment)
        {
            bool status = false;
            string message = "";
            try
            {
                status = repo.AddPayment(payment);
                if (status)
                {
                    message = "Success - Payment Added Successfully";
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

        [HttpGet]
        public IActionResult GetPayment(int groupId)
        {
            List<PaymentDetail> paymentDetails = new List<PaymentDetail>();
            try
            {
                paymentDetails = repo.GetPaymentDetails(groupId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(paymentDetails);
        }

        [HttpGet]
        public IActionResult GetAmountCollected(int groupId)
        {
            List<PaymentDetail> paymentDetails = new List<PaymentDetail>();
            try
            {
                paymentDetails = repo.GetPaymentDetails(groupId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(paymentDetails);
        }

        [HttpDelete]
        public IActionResult DeletePayment(int id)
        {
            bool status = false;
            string message = "";
            try
            {
                status = repo.DeletePayment(id);
                if (status)
                {
                    message = "Success - Deleted Successfully";
                }
                else
                {
                    message = "Failed - Unknkown error";
                }
            }
            catch (Exception ex)
            {
                message = "Failed - " + ex.Message;
            }
            return Ok(message);
        }


        [HttpGet]
        public IActionResult GetAmountCollectedByType(int groupId)
        {
            List<PaymentType> payments = new List<PaymentType>();
            try
            {
                payments = repo.GetAmountCollected(groupId);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(payments);
         }

        [HttpGet]
        public IActionResult AdvisorPayments(int groupId)
        {
            List<PaymentType> payments = new List<PaymentType>();
            try
            {
                payments = repo.AdivisorPaymentDetails(groupId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(payments);
        }

    }
}
