using System;
using System.Collections.Generic;

namespace Eduraise.Models
{
    public partial class Tests
    {
        public Tests()
        {
            Lessons = new HashSet<Lessons>();
        }

        public int TestId { get; set; }
        public string Question { get; set; }
        public string Answers { get; set; }

        public virtual ICollection<Lessons> Lessons { get; set; }
    }
}
