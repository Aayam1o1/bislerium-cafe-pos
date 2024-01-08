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
    public class AddOnsService
    {
        private readonly List<AddOnItem> _addOns = new()
        {
            new() { AddOnsName = "Extra Sugar", Price = 150.0 },
            new() { AddOnsName = "Caramel", Price = 170.0 },
            new() { AddOnsName = "Cream", Price = 120.0 },
            new() { AddOnsName = "Chocolate Syrup", Price = 140.0 },
            
        };

        //add new addon and save it
        public void AddAddOnItem(String addOnName, double price)
        {
            AddOnItem addOnItem = new()
            {
                AddOnsName = addOnName,
                Price = price
            };
            List<AddOnItem> addOnItemTable = GetAddOnItemTableFromJson();

            addOnItemTable.Add(addOnItem);
            SaveAddOnTableInJson(addOnItemTable);
        }

        //save cofee table in json file

        public void SaveAddOnTableInJson(List<AddOnItem> addOnItemTable)
        {
            //folder path for storing 
            string appDataDirctory = AppUtils.GetDesktopDirectoryPath();

            string addOnItemTableFilePath = AppUtils.GetAddOnTableFilePath();

            if (!Directory.Exists(appDataDirctory))
            {
                Directory.CreateDirectory(appDataDirctory);

            }
            var jsonFile = JsonSerializer.Serialize(addOnItemTable);

            File.WriteAllText(addOnItemTableFilePath, jsonFile);
        }

        //retrive coffee tbale form json
        public List<AddOnItem> GetAddOnItemTableFromJson()
        {
            string addOnTableFilePath = AppUtils.GetAddOnTableFilePath();

            if (!File.Exists(addOnTableFilePath))
            {
                return new List<AddOnItem>();
            }
            var jsonFile = File.ReadAllText(addOnTableFilePath);

            return JsonSerializer.Deserialize<List<AddOnItem>>(jsonFile);

        }


        //if there is no json file creates new json file
        public void SeedAddOnDetails()
        {
            List<AddOnItem> AddOnTable = GetAddOnItemTableFromJson();

            //SaveaddonTableInJson(_addOns);
            if (AddOnTable.Count == 0)
            {
                SaveAddOnTableInJson(_addOns);
            }
        }

        //retrive cofee by id from json
        public AddOnItem GetAddOnItemDataById(String addOnItemID)
        {
            List<AddOnItem> addOnItemTable = GetAddOnItemTableFromJson();
            AddOnItem addOnItem = addOnItemTable.FirstOrDefault(addOn => addOn.Id.ToString() == addOnItemID);
            return addOnItem;
        }

        //update existing cofee in the llist and json
        public void UpdateAddOnItemData(AddOnItem addOnItem)
        {
            List<AddOnItem> addOnItemTable = GetAddOnItemTableFromJson();
            AddOnItem AddOnToUpdate = addOnItemTable.FirstOrDefault(_addOnItem => _addOnItem.Id.ToString() == addOnItem.Id.ToString());
            if (AddOnToUpdate == null)
            {
                throw new Exception("Add-On item is not found");
            }

            AddOnToUpdate.AddOnsName = addOnItem.AddOnsName;
            AddOnToUpdate.Price = addOnItem.Price;

            SaveAddOnTableInJson(addOnItemTable);
        }

        //deletes a coffee by id and update in json
        public List<AddOnItem> DeleteAddOnItem(Guid addOnItemID)
        {
            List<AddOnItem> addOnItemTable = GetAddOnItemTableFromJson();
            AddOnItem addOnItem = addOnItemTable.FirstOrDefault(item => item.Id.ToString() == addOnItemID.ToString());

            if (addOnItem != null)
            {
                addOnItemTable.Remove(addOnItem);
                SaveAddOnTableInJson(addOnItemTable);
            }
            return addOnItemTable;

        }

    }
}
