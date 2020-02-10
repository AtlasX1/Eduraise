using System;
using System.Collections.Generic;

namespace Eduraise.Models
{
    public partial class Teachers
    {
        public Teachers()
        {
            Courses = new HashSet<Courses>();
        }

        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        public string TeacherEmail { get; set; }
        public string TeacherPassword { get; set; }
        public byte[] TeacherPhoto { get; set; }
        public string TeacherBio { get; set; }
        public bool IsEmailConfirmed { get; set; }

        public virtual ICollection<Courses> Courses { get; set; }
    }
}
