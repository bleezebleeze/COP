using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using COP.Models;

namespace COP.Servuces
{
    public class CartServiceProxy
    {
        private ProductServiceProxy _prodSvc = ProductServiceProxy.Current;
        private Dictionary<string, List<Item>> carts;
        private string currentCartName = "Default";
        

        public List<Item> CartItems
        {
            get
            {
                if (!carts.ContainsKey(currentCartName))
                {
                    carts[currentCartName] = new List<Item>();
                }
                return carts[currentCartName];
            }
        }

        public List<string> AvailableCartNames => carts.Keys.ToList();

        public string CurrentCartName
        {
            get => currentCartName;
            set
            {
                if (value != null && currentCartName != value)
                {
                    currentCartName = value;
                    if (!carts.ContainsKey(currentCartName))
                    {
                        carts[currentCartName] = new List<Item>();
                    }
                }
            }
        }
        public static CartServiceProxy Current
        {
            get
            {
                if(instance == null)
                {
                    instance = new CartServiceProxy();
                }

                return instance;
            }
        }

        private static CartServiceProxy? instance;

        private CartServiceProxy()
        {
            carts = new Dictionary<string, List<Item>>
            {
                { "Default", new List<Item>() },
                { "Wishlist", new List<Item>() }
            };
        }

        public Item? AddOrUpdate(Item item)
        {
            var existingInvItem = _prodSvc.GetById(item.Id);
            if (existingInvItem == null || existingInvItem.Quantity == 0)
            {
                return null;
            }
            
            if (existingInvItem != null)
            {
                existingInvItem.Quantity--;
            }
            
            var existingItem = CartItems.FirstOrDefault(i => i.Id == item.Id);
            if(existingItem == null)
            {
                var newItem = new Item(item);
                newItem.Quantity = 1;
                CartItems.Add(newItem);

            } else
            {
                existingItem.Quantity++;
            }

            return existingInvItem;
        }

        public Item? ReturnItem(Item item)
        {
            if (item.Id <= 0 || item == null)
            {
                return null;
            }

            var itemToReturn = CartItems.FirstOrDefault(c => c.Id == item.Id);
            if (itemToReturn != null)
            {
                itemToReturn.Quantity--;
                var inventoryItem = _prodSvc.Products.FirstOrDefault(p => p.Id == itemToReturn.Id);
                if (inventoryItem == null)
                {
                    _prodSvc.AddOrUpdate(new Item(itemToReturn));
                } else
                {
                    inventoryItem.Quantity++;
                }
            }
            return itemToReturn;
        }

        public bool CreateNewCart(string cartName)
        {
            if (string.IsNullOrWhiteSpace(cartName) || carts.ContainsKey(cartName))
            {
                return false;
            }

            carts[cartName] = new List<Item>();
            return true;
        }

        public bool DeleteCart(string cartName)
        {
            if (cartName == "Default" || cartName == "Wishlist" || !carts.ContainsKey(cartName))
            {
                return false;
            }

            if (carts.TryGetValue(cartName, out List<Item> cartToDelete))
            {
                foreach (var item in cartToDelete.ToList())
                {
                    while (item.Quantity > 0)
                    {
                        item.Quantity--;
                        var inventoryItem = _prodSvc.Products.FirstOrDefault(p => p.Id == item.Id);
                        if (inventoryItem == null)
                        {
                            _prodSvc.AddOrUpdate(new Item(item) { Quantity = 1 });
                        }
                        else
                        {
                            inventoryItem.Quantity++;
                        }
                    }
                }
            }

            carts.Remove(cartName);

            if (currentCartName == cartName)
            {
                currentCartName = "Default";
            }

            return true;
        }
    }
}