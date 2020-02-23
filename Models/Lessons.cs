using System;
using System.Collections.Generic;

namespace Eduraise.Models
{
    public partial class Lessons
    {
        public int LessonId { get; set; }
        public string LessonName { get; set; }
        public string Content { get; set; }
        public int BlockId { get; set; }
        public int LessonNumber { get; set; }

        public virtual Block Block { get; set; }
    }
}
