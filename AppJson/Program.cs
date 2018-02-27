using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace AppJson
{
    class Item : IEquatable<Item>
    {
        public string Name { get; set; }
        public int Price { get; set; }

        public Item(int price, string name)
        {
            this.Name = name;
            this.Price = price;
        }

        public bool Equals(Item other)
        {
            if (other == null) return false;
            return (this.Name.Equals(other.Name));
        }
    }



    class StorageItem
    {
        private List<Item> listItem;
        private static StorageItem storage { get; set; }
        public static StorageItem Storage
        {
            get { return storage ?? (storage = new StorageItem()); }
            set { storage = value; }
        }

        public StorageItem()
        {
            List<Item>Items = JsonConvert.DeserializeObject<List<Item>>(File.ReadAllText("Storage.json"));
            if (Items == null){ listItem = new List<Item>(); }else { listItem = Items; };
        }

        public void AddNewItem(Item newItem)
        {
            listItem.Add(newItem);
        }

        public bool DeleteItem(Item item)
        {
           return listItem.Remove(item);
        }

        public void UpdateList()
        {
            string data = "";

            data = JsonConvert.SerializeObject(listItem);
            File.WriteAllText("Storage.json", data);
        }

        public List<Item> GetItems()
        {
            return listItem;
        }

    }



    class Program
    {   
        static void Main(string[] args)
        {
            char option = ' ';

            while (option != 'Q')
            {
                PrintMenu();
                option = Char.ToUpper(Char.Parse((Console.ReadLine())));
                ExecuteAction(option);
            }
        }

        public static void ExecuteAction(char option)
        {
            switch (option)
            {
                case 'A':
                    AddNewItem();
                    break;
                case 'D':
                    DeleteItem();
                    break;
                case 'S':
                    ShowItems();
                    break;
                case 'Q':
                    StorageItem.Storage.UpdateList();
                    Console.WriteLine("Closing the program");
                    break;
                default:
                    Console.WriteLine("Incorrect command, try again");
                    break;
            }
        }

        public static void PrintMenu()
        {
            Console.WriteLine(
                "\nSelect an option\n" +
                "Press A to add a new Item\n" +
                "Press D to delete an Item\n" +
                "Press S to show Items\n" +
                "Pres Q to quit program"
           );
        }

        public static void AddNewItem()
        {
            int inputInt = 0;
            string inputString = "";
            var storage = StorageItem.Storage;

            Console.WriteLine("Adding a new item");
            Console.WriteLine("Enter item name");
            inputString = Console.ReadLine();
            Console.WriteLine("Enter item price");
            inputInt = Int32.Parse(Console.ReadLine());


            storage.AddNewItem(new Item(inputInt,inputString));
        }

        public static void DeleteItem()
        {
            string name = "";

            Console.WriteLine("Deleting item");
            Console.WriteLine("Enter item name");
            name = Console.ReadLine();
            string message = StorageItem.Storage.DeleteItem(new Item(0, name)) ? "Item deleted":"Item name invalid";
            Console.WriteLine(message);

        }

        public static void ShowItems()
        {
            List<Item> listItem = StorageItem.Storage.GetItems();

            if(listItem.Count > 0)
            {
                foreach (var item in listItem)
                {
                    Console.WriteLine(String.Format("Name: {0}      Price: {1}", item.Name, item.Price));
                }
            }
            else
            {
                Console.WriteLine("There are not any items");
            }
            
        }
    }
}
