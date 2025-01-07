using System;
using System.Collections.Generic;

namespace ExpenseCalculation.DAL.Models
{
    public partial class TripGroup
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public decimal? GroupShare { get; set; }
    }
}
