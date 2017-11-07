using System;
using System.Collections.Generic;

namespace QueryExpressionTest.Models
{
    public partial class Repository
    {
        public string RepositoryId { get; set; }
        public string RepositoryName { get; set; }
        public string OwnerId { get; set; }
        public string Visibility { get; set; }

        public Account Owner { get; set; }
    }
}
