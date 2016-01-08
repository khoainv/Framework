using System;
using System.Collections.Generic;
using System.Linq;
using SSD.Framework;

namespace SSD.Mobile.Common.Para
{
    public class PagedPara : LastestPagedPara
    {
        public int StartIndex { get; set; }
    }
    public class LastestPagedPara : ListFullPara
    {
        public long CurrentOnID { get; set; }
    }
    public class ListFullPara : ListFullNotDatePara
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    public class ListFullNotDatePara : BaseInput
    {
        public List<int> ListStore
        {
            get;
            set;
        }
    }
    public class SearchPagedPara : BaseInput
    {
        public string Keywords { get; set; }
        public int StartIndex { get; set; }
    }
}
