using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Data
{
    public enum CurbsideOrderStatus {  Processing, Approved }
    public class CurbsideOrder
    {
        public int Id { get; set; }
        public string For { get; set; }
        public string Items { get; set; }
        public DateTime? PickupDate { get; set; }
        public CurbsideOrderStatus Status { get; set; }
    }
}
