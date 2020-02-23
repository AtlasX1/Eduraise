using System;
using System.Collections.Generic;

namespace Eduraise.Models
{
    public partial class Tests
    {
        public Tests()
        {
            Questions = new HashSet<Questions>();
        }

        public int TestId { get; set; }
        public string Content { get; set; }
        public int BlockId { get; set; }

        public virtual Block Block { get; set; }
        public virtual ICollection<Questions> Questions { get; set; }
    }
}
