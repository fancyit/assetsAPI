using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace assets.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        //Creating Roles for users
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityRole>().HasData(
                new { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new { Id = "2", Name = "Moderator", NormalizedName = "MODERATOR" },
                new { Id = "3", Name = "Customer", NormalizedName = "CUSTOMER" }
                );
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<ChangesHistory> Changes { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<AssetCategory> AssetCategory { get; set; }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            var httpContext = _httpContextAccessor.HttpContext;
            // При регистрации, пользователя еще не существует, поэтому смысла в аудите в таком случае нет
            if (!(httpContext.Request.Path == "/api/Account/Register")) 
            {
                var user = httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (httpContext != null && user != null)
                {
                    ChangeTracker.Entries().Where(p => p.State == EntityState.Modified || p.State == EntityState.Deleted)
                        .ToList().ForEach(entry =>
                        {
                            Audit(entry);
                        });
                }
            }
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);            
        }
        private void Audit(EntityEntry entry)
        {            
            var key = entry.Property("Id").CurrentValue.ToString();
            var httpContext = _httpContextAccessor.HttpContext;
            foreach (var property in entry.Properties)
            {
                if (property.OriginalValue.Equals(property.CurrentValue) || property.Metadata.Name == "Modified")
                {
                    continue;
                }
                else
                {
                    var auditEntry = new ChangesHistory
                    {
                        EntityName = entry.Entity.GetType().Name,
                        PropertyName = property.Metadata.Name,
                        PrimaryKeyValue = key,
                        OldValue = property.OriginalValue.ToString(),
                        NewValue = property.CurrentValue.ToString(),
                        DateChanged = DateTime.Now,
                        Author = httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value
                    };
                    this.Changes.Add(auditEntry);
                }
            }
        }
    }
}

