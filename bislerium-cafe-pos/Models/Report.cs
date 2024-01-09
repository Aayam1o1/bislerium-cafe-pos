using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bislerium_cafe_pos.Models
{
    public class Report
    {
        public List<Order> Orders { get; set; }
        public string ReportType { get; set; }
        public string ReportDate { get; set; }
        public double TotalRevenue { get; set; } = 0;
        public List<OrderItems> CoffeeList { get; set; }
        public List<OrderItems> AddOnsList { get; set; }
    }
}
