using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bislerium_cafe_pos.Models
{
    public class Order
    {
        public Guid OrderID { get; set; } = Guid.NewGuid();
        public Guid CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string EmployeeUserName { get; set; }
        public DateTime OrderDateTime { get; set; } = DateTime.Now;
        public List<OrderItems> OrderItems { get; set; }
        public double OrderTotalAmount { get; set; }
        public double DiscountAmount { get; set; } = 0;

        public bool ShowOrderItems { get; set; } = false;
    }
}
