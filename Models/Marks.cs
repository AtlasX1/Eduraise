using System;
using System.Collections.Generic;

namespace Eduraise.Models
{
    public partial class Marks
    {
        public int MarkId { get; set; }
        public int Value { get; set; }
        public int StudentId { get; set; }
        public int TestId { get; set; }

        public virtual Tests Test { get; set; }
    }
}
