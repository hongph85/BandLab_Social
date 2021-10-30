﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostWebAPI
{
    public class PostResponse
    {
        public IEnumerable<Post> Posts { get; set; }
        public string ContinuationToken { get; set; }
    }
}
