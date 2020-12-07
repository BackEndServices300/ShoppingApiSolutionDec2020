using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Data
{
    public class Product
    {
        public int Id { get; set; }
       
        public string Name { get; set; }
        public Category Category { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }

        public bool RemovedFromInventory { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
