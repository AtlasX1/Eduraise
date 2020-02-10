using System;
using System.Collections.Generic;

namespace Eduraise.Models
{
    public partial class Admins
    {
        public int AdminId { get; set; }
        public string AdminEmail { get; set; }
        public string AdminPassword { get; set; }
        public bool IsEmailConfirmed { get; set; }
    }
}
