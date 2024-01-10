using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bislerium_cafe_pos.Models;

namespace bislerium_cafe_pos.Services
{
    public class OrderItemServices
    {
        //Add item to the list of ordered items and increase the quantity if same item is repeated
        public void AddItemInOrderList(List<OrderItems> _orderItems, Guid itemID, string OrderedItemName, String OrderedItemType, Double OrderedItemPrice)
        {
            OrderItems orderItem = _orderItems.FirstOrDefault(x => x.ItemID.ToString() == itemID.ToString() && x.OrderedItemType == OrderedItemType);

            if (orderItem != null)
            {
                orderItem.Quantity++;
                orderItem.TotalPrice = orderItem.Quantity * OrderedItemPrice;

                return;
            }

            orderItem = new()
            {
                ItemID = itemID,
                OrderedItemName = OrderedItemName,
                OrderedItemType = OrderedItemType,
                Quantity = 1,
                Price = OrderedItemPrice,
                TotalPrice = OrderedItemPrice
            };

            _orderItems.Add(orderItem);
        }

        //deletes item form the list or order
        public void DeleteItemInOrderList(List<OrderItems> _orderItems, Guid orderItemID)
        {
            OrderItems orderItem = _orderItems.FirstOrDefault(x => x.OrderItemID == orderItemID);

            if (orderItem != null)
            {
                _orderItems.Remove(orderItem);
            }
        }

        //add or subtract quanttiy of the exisiting order
        public void ManageQuantityOrderItems(List<OrderItems> _orderItems, Guid orderItemID, string action)
        {
            OrderItems orderItem = _orderItems.FirstOrDefault(x => x.OrderItemID == orderItemID);

            if (orderItem != null)
            {
                if (action == "add")
                {
                    orderItem.Quantity++;
                    orderItem.TotalPrice = orderItem.Quantity * orderItem.Price;
                }
                else if (action == "subtract" && orderItem.Quantity > 1)
                {
                    orderItem.Quantity--;
                    orderItem.TotalPrice = orderItem.Quantity * orderItem.Price;
                }
            }
        }

        public double CalculateTotalAmount(IEnumerable<OrderItems> Elements)
        {
            double totalAmount = 0;
            foreach (var item in Elements)
            {
                totalAmount += item.TotalPrice;
            }
            return totalAmount;
        }

        public Dictionary<string, double> ReedeemCoffee(int totalFreeCoffeeCount, List<OrderItems> orderCartItems)
        {
            // Initialize variables for redeemed coffees and total discount amount.
            int totalRedeemedCoffeeCount = 0;
            double totalDiscountAmount = 0;

            // If no free coffees is available, this returns an empty dictionary.
            if (orderCartItems.Count == 0 || totalFreeCoffeeCount <= 0)
            {
                return new Dictionary<string, double>();
            }

            //Caluclating total quantity in cart
            int totalItemsQuantityCart = orderCartItems
                .Where(item => item.OrderedItemType == "coffee")
                .Sum(item => item.Quantity);

            // Filtering, order coffee items in cart with help of price in descending order.
            var coffeeItems = orderCartItems
                .Where(item => item.OrderedItemType == "coffee")
                .OrderByDescending(item => item.Price)
                .ToList();

            foreach (var orderItem in coffeeItems)
            { 
                // Calculating the quantity of the coffee item after redeeming free coffee
                int diffBetweenCartAndFreeCoffeeCount = Math.Max(0, orderItem.Quantity - totalFreeCoffeeCount);

                int reedeemedItemQuantity = diffBetweenCartAndFreeCoffeeCount == 0 ? orderItem.Quantity : diffBetweenCartAndFreeCoffeeCount;

                // Calculate the number of redeemed coffee
                totalRedeemedCoffeeCount += reedeemedItemQuantity;

                // Calculate the discount amount for the item
                totalDiscountAmount += (orderItem.Price * reedeemedItemQuantity);

                // Update the remaining free coffee count.
                totalFreeCoffeeCount -= reedeemedItemQuantity;

                // if no more free coffee- > break the loop
                if (totalFreeCoffeeCount <= 0)
                {
                    break;
                }
            }

            // Return a dictionary containing the total discount amount and the count of redeemed coffees.
            return new Dictionary<string, double>
            {
                { "discount", totalDiscountAmount },
                { "redeemedCoffeeCount", totalRedeemedCoffeeCount }
            };
        }

    }
}
