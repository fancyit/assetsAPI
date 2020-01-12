using assets.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace assets.Service
{
    public class AssetService : IAsset
    {
        private ApplicationDbContext _db;
    public AssetService(ApplicationDbContext db)
    {
        _db = db;
    }
    public async Task CreateAsset(Asset asset)
    {
        _db.Add(asset);
        await _db.SaveChangesAsync();
        //throw new System.NotImplementedException();
    }
    public async Task DeleteAsset(int id)
    {
        var asset = GetAssetByID(id);
        _db.Remove(asset);
        await _db.SaveChangesAsync();
        //throw new System.NotImplementedException();
    }
        public async Task UpdateAsset(Asset asset)
        {
            _db.Update(asset);
            await _db.SaveChangesAsync();
            //throw new System.NotImplementedException();
        }
        public Asset GetAssetByID(int id)
        {
            return _db.Assets
                  .Where(x => x.Id == id)
                  .FirstOrDefault();
        }
        public IEnumerable<Asset> GetAssetsList()
        {
            return _db.Assets
                .Include(a => a.Department)                
                .Include(a => a.AssetCategory)
                .Include(a => a.Supplier).ToList();
        }
        public Stock GetStockByID(int id)
        {
            return _db.Stock
                .Where(x => x.Id == id)
                .FirstOrDefault();
        }
        public IEnumerable<Stock> GetStocksList()
        {
            return _db.Stock.ToList();
        }
        public Department GetDepartmentByID(int id)
        {            
                return _db.Departments
                .Where(x => x.Id == id)
                .FirstOrDefault();                        
        }
        public IEnumerable<Department> GetDepartmentsList()
        {
            return _db.Departments.ToList();                
        }
    }
}
