using System;
using System.Collections.Generic;

namespace Eduraise.Models
{
    public partial class Questions
    {
        public Questions()
        {
            Answers = new HashSet<Answers>();
        }

        public int QuestionId { get; set; }
        public string Content { get; set; }
        public int TestId { get; set; }

        public virtual Tests Test { get; set; }
        public virtual ICollection<Answers> Answers { get; set; }
    }
}
