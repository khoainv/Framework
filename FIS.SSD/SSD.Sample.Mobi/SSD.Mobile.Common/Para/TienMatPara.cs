using System;
using System.Collections.Generic;
using System.Linq;
using SSD.Framework;

namespace SSD.Mobile.Common.Para
{
    public class TienMatPara : BaseInput
    {
        public int StoreID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
