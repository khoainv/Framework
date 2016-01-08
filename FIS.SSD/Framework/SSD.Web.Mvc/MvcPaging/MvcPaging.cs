namespace SSD.Web.Mvc.MvcPaging
{
    public enum Size
    {
        normal,
        mini,
        small,
        large,
    }
    public enum Alignment
    {
        left,
        centered,
        right,
    }
    /// <summary>
    /// Title tooltip
    /// 
    /// </summary>
    public class TooltipTitles
    {
        private string next = "Go to next page";
        private string previous = "Go to previous page";
        private string page = "";
        private string first = "Go to first page";
        private string last = "Go to last page";

        /// <summary>
        /// Default value "Go to next page"
        /// 
        /// </summary>
        public string Next
        {
            get
            {
                return this.next;
            }
            set
            {
                this.next = value;
            }
        }

        /// <summary>
        /// Default value "Go to previous page"
        /// 
        /// </summary>
        public string Previous
        {
            get
            {
                return this.previous;
            }
            set
            {
                this.previous = value;
            }
        }

        public string Page
        {
            get
            {
                return this.page;
            }
            set
            {
                this.page = value;
            }
        }

        /// <summary>
        /// Default value "Go to first page"
        /// 
        /// </summary>
        public string First
        {
            get
            {
                return this.first;
            }
            set
            {
                this.first = value;
            }
        }

        /// <summary>
        /// Default value "Go to last page"
        /// 
        /// </summary>
        public string Last
        {
            get
            {
                return this.last;
            }
            set
            {
                this.last = value;
            }
        }
    }
    /// <summary/>
    public class ItemIcon
    {
        private string next = string.Empty;
        private string previous = string.Empty;
        private string page = string.Empty;
        private string first = string.Empty;
        private string last = string.Empty;

        /// <summary>
        /// Default value string.Empty
        /// 
        /// </summary>
        public string Next
        {
            get
            {
                return this.next;
            }
            set
            {
                this.next = value;
            }
        }

        /// <summary>
        /// Default value string.Empty
        /// 
        /// </summary>
        public string Previous
        {
            get
            {
                return this.previous;
            }
            set
            {
                this.previous = value;
            }
        }

        /// <summary>
        /// Default value string.Empty
        /// 
        /// </summary>
        public string Page
        {
            get
            {
                return this.page;
            }
            set
            {
                this.page = value;
            }
        }

        /// <summary>
        /// Default value string.Empty
        /// 
        /// </summary>
        public string First
        {
            get
            {
                return this.first;
            }
            set
            {
                this.first = value;
            }
        }

        /// <summary>
        /// Default value string.Empty
        /// 
        /// </summary>
        public string Last
        {
            get
            {
                return this.last;
            }
            set
            {
                this.last = value;
            }
        }
    }
    /// <summary/>
    public class ItemTexts
    {
        private string next = "»";
        private string previous = "«";
        private string page = "";
        private string first = "First";
        private string last = "Last";

        /// <summary>
        /// Default value "»"
        /// 
        /// </summary>
        public string Next
        {
            get
            {
                return this.next;
            }
            set
            {
                this.next = value;
            }
        }

        /// <summary>
        /// Default value "«"
        /// 
        /// </summary>
        public string Previous
        {
            get
            {
                return this.previous;
            }
            set
            {
                this.previous = value;
            }
        }

        /// <summary>
        /// Default value null
        /// 
        /// </summary>
        public string Page
        {
            get
            {
                return this.page;
            }
            set
            {
                this.page = value;
            }
        }

        /// <summary>
        /// Default value First
        /// 
        /// </summary>
        public string First
        {
            get
            {
                return this.first;
            }
            set
            {
                this.first = value;
            }
        }

        /// <summary>
        /// Default value Last
        /// 
        /// </summary>
        public string Last
        {
            get
            {
                return this.last;
            }
            set
            {
                this.last = value;
            }
        }
    }
    /// <summary/>
    public class Options
    {
        private bool _isShowControls = true;
        private bool _isShowFirstLast = false;
        private bool _isShowPages = true;
        private ItemTexts _itemTexts;
        private ItemIcon _itemIcon;
        private TooltipTitles _tooltipTitles;
        /// <summary>
        /// Set curent page value
        /// 
        /// </summary>
        public int CurrentPage;
        /// <summary>
        /// Set page size
        /// 
        /// </summary>
        public int PageSize;
        /// <summary>
        /// Set total item count
        /// 
        /// </summary>
        public int TotalItemCount;
        /// <summary>
        /// Set action name
        /// 
        /// </summary>
        public string ActionName;

        /// <summary>
        /// Set font size normal, mini, small, large
        /// 
        /// <para>
        /// Size = Size.normal
        /// </para>
        /// 
        /// </summary>
        public Size Size { get; set; }

        /// <summary>
        /// Set Alignment left, centered right
        /// 
        /// <para>
        /// Alignment = Alignment.centered
        /// </para>
        /// 
        /// </summary>
        public Alignment Alignment { get; set; }

        /// <summary>
        /// Set Paging Next, Previous, Page value
        /// 
        /// <example>
        /// 
        /// <code>
        /// 
        /// <para>
        /// ItemTexts = new ItemTexts() { Next = "»", Previous = "«", Page = "" }
        /// </para>
        /// 
        /// </code>
        /// 
        /// </example>
        /// 
        /// </summary>
        public ItemTexts ItemTexts
        {
            get
            {
                return this._itemTexts == null ? new ItemTexts() : this._itemTexts;
            }
            set
            {
                this._itemTexts = value;
            }
        }

        /// <summary>
        /// Set Paging Next, Previous, Page icon class
        /// 
        /// <example>
        /// 
        /// <code>
        /// 
        /// <para>
        /// ItemIcon = new ItemIcon() { Next = "icon-chevron-right", Previous = "icon-chevron-left" }
        /// </para>
        /// 
        /// </code>
        /// 
        /// </example>
        /// 
        /// </summary>
        public ItemIcon ItemIcon
        {
            get
            {
                return this._itemIcon == null ? new ItemIcon() : this._itemIcon;
            }
            set
            {
                this._itemIcon = value;
            }
        }

        /// <summary>
        /// Set title tooltip for next, previous and page link
        /// 
        /// <para>
        /// TooltipTitles = new TooltipTitles() { Next = "Next page", Previous = "Previous page", Page = "Page" }
        /// </para>
        /// 
        /// </summary>
        public TooltipTitles TooltipTitles
        {
            get
            {
                return this._tooltipTitles == null ? new TooltipTitles() : this._tooltipTitles;
            }
            set
            {
                this._tooltipTitles = value;
            }
        }

        /// <summary>
        /// Set bool value for next and previous button
        /// 
        /// </summary>
        public bool IsShowControls
        {
            get
            {
                return this._isShowControls;
            }
            set
            {
                this._isShowControls = value;
            }
        }

        /// <summary>
        /// Set bool value for first and last button
        /// 
        /// </summary>
        public bool IsShowFirstLast
        {
            get
            {
                return this._isShowFirstLast;
            }
            set
            {
                this._isShowFirstLast = value;
            }
        }

        /// <summary>
        /// Set bool value for 1,2,3,4,5 Paging list, Default value is true
        /// 
        /// </summary>
        public bool IsShowPages
        {
            get
            {
                return this._isShowPages;
            }
            set
            {
                this._isShowPages = value;
            }
        }

        /// <summary>
        /// Set css class for custom design
        /// 
        /// </summary>
        public string CssClass { get; set; }
    }
}