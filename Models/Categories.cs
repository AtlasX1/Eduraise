using System;
using System.Collections.Generic;

namespace Eduraise.Models
{
    public partial class Categories
    {
        public Categories()
        {
            CourseCategory = new HashSet<CourseCategory>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public virtual ICollection<CourseCategory> CourseCategory { get; set; }
    }
}
