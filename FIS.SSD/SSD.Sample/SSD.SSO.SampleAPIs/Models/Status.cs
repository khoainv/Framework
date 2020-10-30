using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace UG.Common.Security
{
    public class Status
    {
        public bool Successeded { get; set; }
        public string UGToken { get; set; }
        public string Message { get; set; }
        public List<int> ListStores { get; set; }
    }
}
