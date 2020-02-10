using System;
using System.Collections.Generic;

namespace Eduraise.Models
{
    public partial class Marks
    {
        public int MarkId { get; set; }
        public int Value { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }

        public virtual Courses Course { get; set; }
    }
}
