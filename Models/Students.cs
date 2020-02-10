using System;
using System.Collections.Generic;

namespace Eduraise.Models
{
    public partial class Students
    {
        public Students()
        {
            CourseStudent = new HashSet<CourseStudent>();
        }

        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }
        public string StudentPassword { get; set; }
        public byte[] StudentPhoto { get; set; }
        public string StudentBio { get; set; }
        public bool IsEmailConfirmed { get; set; }

        public virtual ICollection<CourseStudent> CourseStudent { get; set; }
    }
}
