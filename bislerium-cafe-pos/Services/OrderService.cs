using bislerium_cafe_pos.Models;
using bislerium_cafe_pos.Utils;
using System.Text.Json;

namespace bislerium_cafe_pos.Services
{
    public class OrderService
    {
        public List<Order> GetOrdersFromJson()
        {
            string orderListFilePath = AppUtils.GetOrderListFilePath();

            if (!File.Exists(orderListFilePath))
            {
                return new List<Order>();
            }
            var json = File.ReadAllText(orderListFilePath);

            return JsonSerializer.Deserialize<List<Order>>(json);
            
        }

        //place new order and append new order to order.json file
        public void placeOrder(Order order)
        {
            List<Order> orders = GetOrdersFromJson();
            orders.Add(order);

            //folder path for storing the files
            string appDataDirPath = AppUtils.GetDesktopDirectoryPath();
            string orderListFilePath = AppUtils.GetOrderListFilePath();

            if(!Directory.Exists(appDataDirPath))
            {
                Directory.CreateDirectory(appDataDirPath);
            }
            var json = JsonSerializer.Serialize(orders);

            File.WriteAllText(orderListFilePath, json);
        }
    }
}
