using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using assets.Data;
using assetsAPI.Models.Assets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace assetsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetController : ControllerBase
    {
        private readonly IAsset assetService;

        private static UserManager<ApplicationUser> userManager;

        public AssetController(IAsset AssetService, UserManager<ApplicationUser> UserManager)
        {
            assetService = AssetService;
            userManager = UserManager;
        }
        // GET: api/values        
        [HttpGet("[action]")]
        [Authorize(Policy = "RequireLoggedIn")]
        public IActionResult GetAssets()
        {
            var assets = assetService.GetAssetsList()
                .Select(a => new AssetsListingModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    AssetCategory = a.AssetCategory.Name,
                    CurrentOwner = a.CurrentOwner,
                    LastTransaction = a.Modified == null ? DateTime.Today : a.Modified,
                    Supplier = a.Supplier.Name,                    
                    State = a.Status.ToString(),
                    Department = a.Department.Name
                });
            return Ok(assets);
        }
        [HttpGet("[action]/{id}")]
        [Authorize(Policy = "RequireLoggedIn")]
        public IActionResult GetAssetById(int id)
        {
            return Ok(assetService.GetAssetByID(id));
            
        }
        // GET: api/values        
        [HttpGet("[action]")]
        //[Authorize(Policy = "RequireLoggedIn")]
        public IActionResult GetDepartmentsList()
        {
            var mvzList = assetService.GetDepartmentsList()
                .Select(a => new DepartmentsListingModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description
                });
            return Ok(mvzList.ToList());
        }
        [HttpPost("[action]")]
        [Authorize(Policy = "RequireAdministratorRole")]
        public async Task<IActionResult> AddAsset([FromBody] Asset formdata)
        {
            //var userId = userManager.GetUserId(User);
            //var user = await userManager.FindByIdAsync(userId);
            //var user = await userManager.FindByNameAsync(formdata.CurrentOwner);
            //var newAsset = BuildNewAsset(formdata, user);
            var newAsset = BuildNewAsset(formdata);
            await assetService.CreateAsset(newAsset);
            return Ok(new JsonResult("The asset was Added Successfully"));
        }

        [HttpPost("[action]")]
        [Authorize(Policy = "RequireAdministratorRole")]
        public async Task<IActionResult> UpdateAsset([FromBody] Asset formdata)
        {
            var asset = assetService.GetAssetByID(formdata.Id);            
            asset.Name = formdata.Name;
            asset.ProductNumber = formdata.ProductNumber;
            asset.SerialNumber = formdata.SerialNumber;
            asset.DepartmentId = formdata.DepartmentId;
            asset.Description = formdata.Description;
            asset.Modified = DateTime.Now;
            asset.StockId = formdata.StockId;
            asset.AssetCategoryId = formdata.AssetCategoryId;
            asset.SupplierId = formdata.SupplierId;
            asset.CurrentOwner = formdata.CurrentOwner;
            await assetService.UpdateAsset(asset);
            return Ok(new JsonResult("The Asset was updated Successfully"));
        }

        //private Asset BuildNewAsset(Asset formdata, ApplicationUser user)
        private Asset BuildNewAsset(Asset formdata)
        {
            return new Asset
            {
                //Id = formdata.Id,
                Name = formdata.Name,
                SerialNumber = formdata.SerialNumber,
                ProductNumber = formdata.ProductNumber,
                DepartmentId = formdata.DepartmentId,
                Description = formdata.Description==null ? "" : formdata.Description,
                IncomeDate = DateTime.Now,
                Modified = DateTime.Now,
                StockId = formdata.StockId == null ? 1 : formdata.StockId,                
                AssetCategoryId = formdata.AssetCategoryId,
                SupplierId = formdata.SupplierId,
                CurrentOwner = formdata.CurrentOwner
            };
            //throw new NotImplementedException();
        }
    }
}