using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bislerium_cafe_pos.Models;
using bislerium_cafe_pos.Utils;
using System.Text.Json;

namespace bislerium_cafe_pos.Services
{
    public class CoffeeServices
    {
        private readonly List<Coffee> _coffeeTable = new()
        {
            new() { CoffeeName = "Cappuccino", Price = 150.0 },
            new() { CoffeeName = "Latte", Price = 170.0 },
            new() { CoffeeName = "Espresso", Price = 120.0 },
            new() { CoffeeName = "Americano", Price = 140.0 },
            new() { CoffeeName = "Mocha", Price = 180.0 },
            new() { CoffeeName = "Flat White", Price = 160.0 },
            new() { CoffeeName = "Affogato", Price = 200.0 },
            new() { CoffeeName = "Irish Coffee", Price = 190.0 },
            new() { CoffeeName = "Turkish Coffee", Price = 130.0 },
            new() { CoffeeName = "lol Coffee", Price = 130.0 },
            new() { CoffeeName = "test Coffee", Price = 130.0 }
        };

        //add new coffe and save it
        public List<Coffee> AddCoffee(String coffeeName, double price)
        {
            Coffee coffee = new()
            {
                CoffeeName = coffeeName,
                Price = price
            };
            List<Coffee> coffeeTable = GetCoffeeTableFromJson();

            coffeeTable.Add(coffee);
            SaveCoffeeTableInJson(coffeeTable);

            return coffeeTable;
        }

        //save cofee table in json file
        public void SaveCoffeeTableInJson(List<Coffee> coffeeTable)
        {
            //folder path for storing 
            string appDataDirctory = AppUtils.GetDesktopDirectoryPath();

            string coffeeTableFilePath = AppUtils.GetCoffeeFilePath();

            if (!Directory.Exists(appDataDirctory))
            {
                Directory.CreateDirectory(appDataDirctory);

            }
            var jsonFile = JsonSerializer.Serialize(coffeeTable);

            File.WriteAllText(coffeeTableFilePath, jsonFile);
        }


        //retrive coffee tbale form json
        public List<Coffee> GetCoffeeTableFromJson()
        {
            string coffeeTableFilePath = AppUtils.GetCoffeeFilePath();

            if (!File.Exists(coffeeTableFilePath))
            {
                return new List<Coffee>();
            }
            var jsonFile = File.ReadAllText(coffeeTableFilePath);
            return JsonSerializer.Deserialize<List<Coffee>>(jsonFile);

        }

        //if there is no json file creates new json file
        public void SeedCoffeeDetails()
        {
            List<Coffee> coffeeTable = GetCoffeeTableFromJson();

            //SaveCoffeeTableInJson(_coffeeTable);
            if (coffeeTable.Count == 0)
            {
                SaveCoffeeTableInJson(_coffeeTable);
            }
        }

        //retrive cofee by id from json
        public Coffee GetCoffeeDataById(String coffeeID)
        {
            List<Coffee> coffeeTable = GetCoffeeTableFromJson();
            Coffee coffee = coffeeTable.FirstOrDefault(coffee => coffee.Id.ToString() == coffeeID);
            return coffee;
        }

        //update existing cofee in the llist and json
        public void UpdateCoffeeData(Coffee coffee)
        {
            List<Coffee> coffeeTable = GetCoffeeTableFromJson();
            Coffee coffeeToUpdate = coffeeTable.FirstOrDefault(_coffee => _coffee.Id.ToString() == coffee.Id.ToString());
            if (coffeeToUpdate == null)
            {
                throw new Exception("Coffee is not found");
            }

            coffeeToUpdate.CoffeeName = coffee.CoffeeName;
            coffeeToUpdate.Price = coffee.Price;

            SaveCoffeeTableInJson(coffeeTable);
        }

        //deletes a coffee by id and update in json
        public List<Coffee> DeleteCoffeeName(Guid coffeeID)
        {
            List<Coffee> coffeeTable = GetCoffeeTableFromJson();
            Coffee coffee = coffeeTable.FirstOrDefault(coffee => coffee.Id.ToString() == coffeeID.ToString());

            if(coffee != null)
            {
                coffeeTable.Remove(coffee);
                SaveCoffeeTableInJson(coffeeTable);
            }
            return coffeeTable;

        }
    }
}
