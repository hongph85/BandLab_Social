﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BandLab_Social.Entities
{
    public class PostPagination
    {
        public IEnumerable<Post> Posts { get; set; }
        public string ContinuationToken { get; set; }
    }
}
