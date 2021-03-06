﻿using System;
using System.Collections.Generic;

namespace QueryExpressionTest.Models
{
    public partial class User
    {
        public User()
        {
            TeamMember = new HashSet<TeamMember>();
        }

        public string AccountId { get; set; }

        public Account Account { get; set; }
        public ICollection<TeamMember> TeamMember { get; set; }
    }
}
