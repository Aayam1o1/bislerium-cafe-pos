using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bislerium_cafe_pos.Models;

namespace bislerium_cafe_pos.Services
{
    public class ReportService
    {
        private OrderService _orderService { get; set; }
        public ReportService(OrderService orderService)
        {
            _orderService = orderService;
        }

        //for generating oirderlist 
        public List<Order> GenerateOrderTable(string reportType, string reportDate)
        {
            List<Order> orders = _orderService.GetOrdersFromJson();
            if (reportType.ToLower() == "monthly")
            {
                orders = orders.Where(x => reportDate == x.OrderDateTime.ToString("yyyy-MM")).ToList();
            }
            else if (reportType.ToLower() == "daily")
            {
                orders = orders.Where(x => reportDate == x.OrderDateTime.ToString("yyyy-MM-dd")).ToList();
            }
            return orders;
        }

        public Dictionary<string, List<OrderItems>> MostPurchasedCoffeeAndAddOns(List<Order> orders)
        {
            //getting all order items into one list
            List<OrderItems> allOrderitems = orders
            .SelectMany(order => order.OrderItems)
            .ToList();

            //get all coffee and addon in seperate lsits
            List<OrderItems> coffeeList = allOrderitems.Where(item => item.OrderedItemType == "coffee").ToList();
            List<OrderItems> addInsList = allOrderitems.Where(item => item.OrderedItemType == "add-in").ToList();


            //Get most ordered coffee
            List<OrderItems> mostOrderedCoffee = coffeeList
                .GroupBy(coffee => coffee.OrderedItemName)
                .Select(group =>
                {
                    var OrderedItemName = group.Key;
                    int totalQuantity = group.Sum(OrderItem => OrderItem.Quantity);

                    return new OrderItems
                    {
                        OrderedItemName = OrderedItemName,
                        Quantity = totalQuantity,

                    };
                }).ToList();

            //Get most ordered addOns
            List<OrderItems> mostOrderedAddOns = coffeeList
            .GroupBy(addIn => addIn.OrderedItemName)
            .Select(group =>
            {
                var OrderedItemName = group.Key;
                int totalQuantity = group.Sum(orderItem => orderItem.Quantity);

                return new OrderItems
                {
                    OrderedItemName = OrderedItemName,
                    Quantity = totalQuantity,
                };
            }).ToList();

            //Return the dictionary of the most ordered coffee and add-ins
            return new Dictionary<string, List<OrderItems>>
            {
                { "coffees", mostOrderedCoffee.Take(5).ToList() },
                { "add-ons", mostOrderedAddOns.Take(5).ToList() }
            };


        }
    }
}
