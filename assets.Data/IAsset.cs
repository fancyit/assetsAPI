using System.Collections.Generic;
using System.Threading.Tasks;

namespace assets.Data
{
    public interface IAsset
    {
        Asset GetAssetByID(int id);
        Department GetDepartmentByID(int id);
        Stock GetStockByID(int id);
        IEnumerable<Asset> GetAssetsList();
        IEnumerable<Department> GetDepartmentsList();
        IEnumerable<Stock> GetStocksList();
        Task CreateAsset(Asset asset);
        Task UpdateAsset(Asset asset);
        Task DeleteAsset(int Id);
    }
}
