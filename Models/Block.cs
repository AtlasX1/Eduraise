using System;
using System.Collections.Generic;

namespace Eduraise.Models
{
    public partial class Block
    {
        public Block()
        {
            Lessons = new HashSet<Lessons>();
        }

        public int BlockId { get; set; }
        public string BlockName { get; set; }
        public int CourseId { get; set; }
        public int BlockNumber { get; set; }

        public virtual Courses Course { get; set; }
        public virtual ICollection<Lessons> Lessons { get; set; }
    }
}
