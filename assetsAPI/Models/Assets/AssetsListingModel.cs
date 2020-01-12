using System;

namespace assetsAPI.Models.Assets
{
    public class AssetsListingModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AssetCategory { get; set; }
        public string CurrentOwner { get; set; }
        public DateTime? LastTransaction { get; set; }
        public string Supplier { get; set; }
        public string Invoce { get; set; }
        public string State { get; set; }
        public string Department { get; set; }
    }
}
