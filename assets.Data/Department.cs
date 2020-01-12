using System.Collections.Generic;

namespace assets.Data
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual IEnumerable<ApplicationUser> Users { get; set; }
    }
}