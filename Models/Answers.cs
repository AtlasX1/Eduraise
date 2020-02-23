using System;
using System.Collections.Generic;

namespace Eduraise.Models
{
    public partial class Answers
    {
        public int AnswerId { get; set; }
        public string Content { get; set; }
        public int QuestionId { get; set; }
        public bool IsCorrect { get; set; }

        public virtual Questions Question { get; set; }
    }
}
