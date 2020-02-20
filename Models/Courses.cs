using System;
using System.Collections.Generic;

namespace Eduraise.Models
{
    public partial class Courses
    {
        public Courses()
        {
            Block = new HashSet<Block>();
            CourseCategory = new HashSet<CourseCategory>();
            CourseStudent = new HashSet<CourseStudent>();
            Marks = new HashSet<Marks>();
        }

        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public DateTime DataOfCreation { get; set; }
        public float? CourseRating { get; set; }
        public int TeachersId { get; set; }
        public bool? IsVerified { get; set; }

        public virtual Teachers Teachers { get; set; }
        public virtual ICollection<Block> Block { get; set; }
        public virtual ICollection<CourseCategory> CourseCategory { get; set; }
        public virtual ICollection<CourseStudent> CourseStudent { get; set; }
        public virtual ICollection<Marks> Marks { get; set; }
    }
}
