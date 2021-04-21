﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.ModelDTO
{
    public class VirtualizeResponse<T>
    {
        public List<T> Items { get; set; }
        public int TotalSize { get; set; }
    }
}
