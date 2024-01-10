using bislerium_cafe_pos.Models;
using bislerium_cafe_pos.Utils;
using System.Text.Json;


namespace bislerium_cafe_pos.Services
{
    public class CustomerService
    {
        private OrderService _orderServices;

        public CustomerService(OrderService orderService)
        {
            _orderServices = orderService;
        }

        //Retrvies the list of customer form json
        public List<Customer> GetCustomerListFromJson()
        {
            string customersFilePath = AppUtils.GetCustomersListFilePath();

            if (!File.Exists(customersFilePath))
            {
                return new List<Customer>();
            }
            var json = File.ReadAllText(customersFilePath);

            return JsonSerializer.Deserialize<List<Customer>>(json);
        }

        //saves a list of customer to json file
        public void SaveCustomerListInJson(List<Customer> customers)
        {
            string appDataDirPath = AppUtils.GetDesktopDirectoryPath();
            string customerListFilePath = AppUtils.GetCustomersListFilePath();

            if(!Directory.Exists(appDataDirPath))
            {
                Directory.CreateDirectory(appDataDirPath);
            }
            var json = JsonSerializer.Serialize(customers);
            File.WriteAllText(customerListFilePath, json);
        }

        //Retrives a customer by their phone number from json
        public Customer GetCustomerByPhoneNumber(string customerPhoneNumber)
        {
            List<Customer> customers = GetCustomerListFromJson();
            Customer customer = customers.FirstOrDefault(c => c.CustomerPhoneNumber == customerPhoneNumber);
            return customer;
        }
        //add a new customer to list and update in json
        public void AddCustomer(Customer _customer)
        {
            Customer customerExists = GetCustomerByPhoneNumber(_customer.CustomerPhoneNumber);
            if (customerExists != null)
            {
                throw new Exception("Customer is already registerd");
            }

            List<Customer> customers = GetCustomerListFromJson();
            customers.Add(_customer);

            SaveCustomerListInJson(customers);

        }

        //update customer order when a order is placed and save in updated order list in json
        public void UpdateRedeemedCoffeeCount(string customerPhoneNumber, int redeemedCoffeeCount)
        {
            List<Customer> customers = GetCustomerListFromJson();
            Customer customer = customers.FirstOrDefault(c => c.CustomerPhoneNumber == customerPhoneNumber);
            customer.RedeemedCoffeeCount += redeemedCoffeeCount;

            SaveCustomerListInJson(customers);

        }
        //For free coffee when order is more thatn 28
        public bool IfCustomerIsRegularMember(string customerPhoneNumber)

        {
            List<Order> orders = _orderServices.GetOrdersFromJson();

            //ffiltering months according to the previous and new year
            int month = DateTime.Now.Month -1;
            int year = month == 12 ? DateTime.Now.Year -1 : DateTime.Now.Year;

            //Filterign the customer number and previous month, and grouping the order by day adn calculating the total count of orders.
            int totalOrderCount = orders
                .Where(order => order.CustomerPhoneNumber == customerPhoneNumber && order.OrderDateTime.Month == month && order.OrderDateTime.Year == year)
                .GroupBy(order => order.OrderDateTime.Day)
                .ToList().Count();

            //retunss the total order count for a regular memeber
            return totalOrderCount >= 26;
        }
        //for total freee coffee redemption calculation
        public int TotalFreeCoffeeCount (string customerPhoneNumber)
        {
            List <Order> orders = _orderServices.GetOrdersFromJson();
            int totalOrderCount = orders
            .Where(order => order.CustomerPhoneNumber == customerPhoneNumber)
                .ToList().Count();

            return totalOrderCount / 10;
        }
    }
}
