using System;
using System.Collections.Generic;

namespace Eduraise.Models
{
    public partial class CourseStudent
    {
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public DateTime DateOfOverview { get; set; }

        public virtual Courses Course { get; set; }
        public virtual Students Student { get; set; }
    }
}
