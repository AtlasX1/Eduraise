using System;
using System.Collections.Generic;

namespace Eduraise.Models
{
    public partial class CourseCategory
    {
        public int CourseId { get; set; }
        public int CategoryId { get; set; }

        public virtual Categories Category { get; set; }
        public virtual Courses Course { get; set; }
    }
}
