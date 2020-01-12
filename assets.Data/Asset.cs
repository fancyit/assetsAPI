using System;
using System.ComponentModel;

namespace assets.Data
{
    public enum Status
    {
        [Description("Используется")]
        InUse,
        [Description("На складе")]
        InStock,
        [Description("Резализовано")]
        Decomissioned
    }
    public class Asset
    {
        private Status stat;
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProductNumber { get; set; }
        public string SerialNumber { get; set; }
        public string Description { get; set; }
        public DateTime IncomeDate { get; set; }
        public Status Status
        {
            get 
            {
                return stat;
            }
            set 
            {
                stat = value;
            }
        }
        public string CurrentOwner { get; set; }
        public DateTime? Modified { get; set; }
        public int AssetCategoryId { get; set; }        
        public int? DepartmentId { get; set; }
        public int SupplierId { get; set; }
        public int? StockId { get; set; }
        public AssetCategory AssetCategory { get; set; }        
        public Department Department { get; set; }
        public Supplier Supplier { get; set; }
        //public History History { get; set; }
        public Stock Stock { get; set; }
    }
}