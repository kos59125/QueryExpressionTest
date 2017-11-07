using System;
using System.Collections.Generic;

namespace QueryExpressionTest.Models
{
    public partial class TeamMember
    {
        public string TeamId { get; set; }
        public string MemberId { get; set; }

        public User Member { get; set; }
        public Team Team { get; set; }
    }
}
