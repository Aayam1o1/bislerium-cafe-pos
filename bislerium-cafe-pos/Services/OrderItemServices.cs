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
    }
}
