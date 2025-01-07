using ExpenseCalculation.DAL.Models;

namespace ExpenseCalculation.DAL.BusinessLogic
{
    public class ExpenseCategoryRepo
    {
        ExpenseCalcContext calcContext;

        public ExpenseCategoryRepo()
        {
            calcContext = new ExpenseCalcContext();
        }

        /// <summary>
        /// Create Payment category
        /// </summary>
        /// <returns></returns>
        public List<Category> GetCategories()
        {
            List<Category> categoryDetails = new List<Category>();
            try
            {
                categoryDetails = calcContext.Categories.Distinct().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return categoryDetails;
        }

        /// <summary>
        /// Create Categary Name in Db
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public string AddCategory(string categoryName)
        {
            string result = "";
            try
            {
                int isUnique = 0, categoryId = 0, count = 0;
                Category category = new Category();

                count = calcContext.Categories.Select(x => x.CategoryId).Count();

                if (count > 0)
                {
                    categoryId = calcContext.Categories.Select(x => x.CategoryId).Max() + 1;
                    isUnique = calcContext.Categories.Where(x => x.CategoryName == categoryName).Count();


                    if (isUnique == 0)
                    {
                        result = CreateCategory(categoryId, categoryName);
                    }
                    else
                    {
                        result = "Failed - Category Name is already present";
                    }
                }
                else
                {
                    categoryId = 1;
                    result = CreateCategory(categoryId, categoryName);
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
        /// Add Category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public string CreateCategory(int categoryId, string name)
        {
            string result = "";
            try
            {
                Category category = new Category();
                category.CategoryId = categoryId;
                category.CategoryName = name;
                calcContext.Categories.Add(category);
                calcContext.SaveChanges();
                result = "Success - Category Name Added Successfully";
            }
            catch (Exception ex)
            {
                result = "Failed - " + ex.Message;
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Delete category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public bool DeleteCategory(int categoryId)
        {
            bool status = false;
            try
            {
                Category category = new Category();
                category = calcContext.Categories.Where(x => x.CategoryId == categoryId).FirstOrDefault();
                calcContext.Categories.Remove(category);
                calcContext.SaveChanges(status);
                status = true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return status;
        }

        /// <summary>
        /// Get Total Amount Spent based on Category
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<AmountSpentCategory> GetAmountSpentsCategorywise(int groupId)
        {
            List<AmountSpentCategory> spentCategories = new List<AmountSpentCategory>();
            try
            {
                var amountPaid = calcContext.PaymentDetails.Where(x => x.GroupId == groupId && x.CategoryName != "Advance").GroupBy(g => g.CategoryName).Select(x => new { Id = x.Key, Value = x.Sum(x => x.Amount) }).OrderBy(x => x.Id).ToList();
                foreach (var c in amountPaid)
                {
                    AmountSpentCategory amount = new AmountSpentCategory();
                    amount.CategoryName = c.Id;
                    amount.AmountSpent = (decimal)c.Value;

                    spentCategories.Add(amount);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return spentCategories;
        }
    }
}
