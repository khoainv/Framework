using System;
using System.Collections.Generic;

namespace SSD.Web.UI.Tags
{
    /// <summary>
    /// Represents a single tag in the cloud
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// Gets or sets the text of the tag
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the url that is used when a tag is clicked. 
        /// Leave empty or null to produce a non clickable tag.
        /// </summary>
        public string NavigateUrl { get; set; }

        /// <summary>
        /// Gets or sets the css class of the tag.
        /// </summary>
        public string CssClass { get; set; }

        /// <summary>
        /// Gets or sets the weight of the tag.
        /// Does not affect graphical output and can be left 0 without any impact on result.
        /// When you let a tag cloud get generated from a tag list this property will be set automatically.
        /// Used only for informational purpose.
        /// </summary>
        public int TagWeight { get; set; }

        /// <summary>
        /// Gets or sets the tooltip text of the tag.
        /// The text is displayed when the cursor hovers over the tag.
        /// Leave empty or null to disable tooltip functionality for this tag.
        /// </summary>
        public string ToolTip { get; set; }

        /// <summary>
        /// Gets or sets the additional html attributes for this tag.
        /// </summary>
        public IDictionary<string, object> HtmlAttributes { get; set; }

    }
}
