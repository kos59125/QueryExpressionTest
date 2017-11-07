using System;
using System.Collections.Generic;

namespace QueryExpressionTest.Models
{
    public partial class Account
    {
        public Account()
        {
            Repository = new HashSet<Repository>();
        }

        public string AccountId { get; set; }
        public string AccountName { get; set; }

        public Team Team { get; set; }
        public User User { get; set; }
        public ICollection<Repository> Repository { get; set; }
    }
}
