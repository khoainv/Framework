using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace SSD.Web.UI.Tags
{
    /// <summary>
    /// Creates a tag cloud for use on a web page.
    /// </summary>
    public class TagCloud
    {
        /// <summary>
        /// Gets the Tag objects of the tag cloud.
        /// </summary>
        public IEnumerable<Tag> Tags { get; private set; }

        #region CreateTags

        /// <summary>
        /// Creates Tag objects from a provided list of string tags. 
        /// The more times a tag occures in the list, the larger weight it will get in the tag cloud.
        /// </summary>
        /// <param name="tags">A string list of tags</param>
        /// <param name="rules">A TagCloudGenerationRules object to decide how the cloud is generated.</param>
        /// <returns>A list of Tag objects that can be used to create the tag cloud.</returns>
		public static IEnumerable<Tag> CreateTags(IEnumerable<string> tags, TagCloudGenerationRules generationRules)
        {
            #region Parameter validation

            if (tags == null) throw new ArgumentNullException("tags");
            if (generationRules == null) throw new ArgumentNullException("generationRules");

            #endregion

            //Transform tag string list to list with each distinct tag and its weight
            return CreateTags(from tag in tags
                              group tag by tag into tagGroup
                              select new KeyValuePair<string, int>(tagGroup.Key, tagGroup.Count()), generationRules);
        }

        /// <summary>
        /// Creates Tag objects from a provided dictionary of string tags along with integer values indicating the weight of each tag. 
        /// This overload is suitable when you have a list of already weighted tags, i.e. from a database query result.
        /// </summary>
        /// <param name="weightedTags">A dictionary that takes a string for the tag text (as the dictionary key) and an integer for the tag weight (as the dictionary value).</param>
        /// <param name="rules">A TagCloudGenerationRules object to decide how the cloud is generated.</param>
        /// <returns>A list of Tag objects that can be used to create the tag cloud.</returns>
        public static IEnumerable<Tag> CreateTags(IDictionary<string, int> weightedTags, TagCloudGenerationRules generationRules)
        {
            #region Parameter validation

            if (weightedTags == null) throw new ArgumentNullException("weightedTags");
            if (generationRules == null) throw new ArgumentNullException("generationRules");

            #endregion

            return CreateTags(weightedTags.ToList(), generationRules);
        }

        /// <summary>
        /// Creates Tag objects from a provided list of string tags along with integer values indicating the weight of each tag. 
        /// This overload is suitable when you have a list of already weighted tags, i.e. from a database query result.
        /// </summary>
        /// <param name="weightedTags">A list of KeyValuePair objects that take a string for the tag text and an integer for the weight of the tag.</param>
        /// <param name="rules">A TagCloudGenerationRules object to decide how the cloud is generated.</param>
        /// <returns>A list of Tag objects that can be used to create the tag cloud.</returns>
        public static IEnumerable<Tag> CreateTags(IEnumerable<KeyValuePair<string, int>> weightedTags, TagCloudGenerationRules generationRules)
        {
			#region Parameter validation

			if (weightedTags == null) throw new ArgumentNullException("weightedTags");
            if (generationRules == null) throw new ArgumentNullException("generationRules");

			#endregion

			// Select all tags that exists "settings.RequiredTagWeight" times or more and order them by weight.
			weightedTags = from weightedTag in weightedTags
						   where weightedTag.Value >= generationRules.RequiredTagWeight
						   orderby weightedTag.Value descending
						   select weightedTag;

            // Crop list if "settings.MaxNumberOfTags" is specified
			if (generationRules.MaxNumberOfTags.HasValue)
				weightedTags = weightedTags.Take(generationRules.MaxNumberOfTags.Value);

			// Change sort order if necessary (the list is already ordered by weight descending)
            switch (generationRules.Order) 
            {
                case TagCloudOrder.Alphabetical:    // Renders tags alphabetically
                    weightedTags = weightedTags.OrderBy(tag => tag.Key);
                    break;
                case TagCloudOrder.AlphabeticalDescending:  // Renders tags alphabetically descending
                    weightedTags = weightedTags.OrderByDescending(tag => tag.Key);
                    break;
                case TagCloudOrder.Weight:          // Renders tags with higher weight at the end
                    weightedTags = weightedTags.OrderBy(tag => tag.Value);
                    break;
                case TagCloudOrder.Centralized:     // Renders tags with higher weight in the middle
                    weightedTags = weightedTags.OrderBy(tag => tag.Value);
                    weightedTags = weightedTags.Where((kvp, i) => (i % 2 == 0)).Concat(weightedTags.Where((kvp, i) => (i % 2 == 1)).Reverse());
                    break;
                case TagCloudOrder.Decentralized:   // Renders tags with higher weight at the edges (start and end)
                    weightedTags = weightedTags.OrderBy(tag => tag.Value);
                    weightedTags = weightedTags.Where((kvp, i) => (i % 2 == 0)).Reverse().Concat(weightedTags.Where((kvp, i) => (i % 2 == 1)));
                    break;
                case TagCloudOrder.Random:          // Renders tags rendomly
                    weightedTags = weightedTags.OrderBy(tag => tag, RandomComparer.Comparer);
                    break;
            }

			// Retrieve the css class table used to decide the style of the tags
            Dictionary<int, string> cssClassTable = GenerateCssClassTable(weightedTags, generationRules.TagCssClassPrefix, generationRules.WeightClassPartitioning.ToArray());

			// Transform all the string tags into Tag objects
            IEnumerable<Tag> cloudTags = from weightedTag in weightedTags
                                         select new Tag
                                         {
                                             Text = weightedTag.Key,
                                             TagWeight = weightedTag.Value,
                                             CssClass = cssClassTable[weightedTag.Value],
                                             NavigateUrl = string.Format(generationRules.TagUrlFormatString, HttpUtility.UrlEncode(weightedTag.Key)),
                                             ToolTip = string.Format(generationRules.TagToolTipFormatString, weightedTag.Value),
                                             HtmlAttributes = generationRules.HtmlAttributes
                                         };

			return cloudTags;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a tag cloud from a provided list of string tags. 
        /// The more times a tag occures in the list, the larger weight it will get in the tag cloud.
        /// </summary>
        /// <param name="tags">A string list of tags</param>
        /// <param name="generationRules">A TagCloudGenerationRules object to decide how the cloud is generated.</param>
		public TagCloud(IEnumerable<string> tags, TagCloudGenerationRules generationRules) 
        {
            #region Parameter validation

            if (tags == null) throw new ArgumentNullException("tags");
            if (generationRules == null) throw new ArgumentNullException("generationRules");

            #endregion

			this.Tags = CreateTags(tags, generationRules);
        }

        /// <summary>
        /// Creates Tag objects from a provided dictionary of string tags along with integer values indicating the weight of each tag. 
        /// This constructor is suitable when you have a list of already weighted tags, i.e. from a database query result.
        /// </summary>
        /// <param name="weightedTags">A dictionary that takes a string for the tag text (as the dictionary key) and an integer for the tag weight (as the dictionary value).</param>
        /// <param name="rules">A TagCloudGenerationRules object to decide how the cloud is generated.</param>
        /// <returns>A list of Tag objects that can be used to create the tag cloud.</returns>
        public TagCloud(IDictionary<string, int> weightedTags, TagCloudGenerationRules generationRules)
        {
            #region Parameter validation

            if (weightedTags == null) throw new ArgumentNullException("weightedTags");
            if (generationRules == null) throw new ArgumentNullException("generationRules");

            #endregion

            this.Tags = CreateTags(weightedTags.ToList(), generationRules);
        }

        /// <summary>
        /// Creates a tag cloud from a provided list of string tags along with integer values indicating the weight of each tag. 
        /// This constructor is suitable when you have a list of already weighted tags, i.e. from a database query result.
        /// </summary>
        /// <param name="weightedTags">A list of KeyValuePair objects that take a string for the tag text and an integer for the weight of the tag.</param>
        /// <param name="generationRules">A TagCloudGenerationRules object to decide how the cloud is generated.</param>
		public TagCloud(IEnumerable<KeyValuePair<string, int>> weightedTags, TagCloudGenerationRules generationRules) 
        {
            #region Parameter validation

            if (weightedTags == null) throw new ArgumentNullException("weightedTags");
            if (generationRules == null) throw new ArgumentNullException("generationRules");

            #endregion

            this.Tags = CreateTags(weightedTags, generationRules);
        }

        /// <summary>
        /// Creates a tag cloud from a provided list of Tag objects.
        /// Use this constructor to have full control over the content in the cloud.
        /// </summary>
        /// <param name="tags">Tag objects used to generate the tag cloud.</param>
        public TagCloud(IEnumerable<Tag> tags)
        {
            if (tags == null) throw new ArgumentNullException("tags");

            this.Tags = tags;
        }

        #endregion

		#region GenerateFontSizeTable

		/// <summary>
		/// Generates a css class table so we can keep track of what tag weights relates to what css class.
        /// The result is based upon the provided list of FontSizeOccurrence objects.
		/// </summary>
        private static Dictionary<int, string> GenerateCssClassTable(IEnumerable<KeyValuePair<string, int>> weightedTags, string tagCssClassPrefix, int[] tagWeightDistribution)
		{
			//Group tags with the same weight together and get the number of tags for each weight to produce a list that tells us how many tags has a specific weight.
			var weightOccurrences = from weightedTag in weightedTags
									group weightedTag by weightedTag.Value into weightedTagGroup
									orderby weightedTagGroup.Key
									select new
									{
										Weight = weightedTagGroup.Key,
										NumberOfTags = weightedTagGroup.Count()
                                    };

			Dictionary<int, string> cssClassTable = new Dictionary<int, string>();
			int distinctTagsCount = weightedTags.Count();
            int tagWeightDistributionIndex = tagWeightDistribution.Length, percentageCovered = 0, tagsCovered = 0;

            // Distribute the css classes according to the tagWeightDistribution parameter
			foreach (var weightOccurrence in weightOccurrences)
			{
                cssClassTable[weightOccurrence.Weight] = string.Concat(tagCssClassPrefix, tagWeightDistributionIndex);

				tagsCovered += weightOccurrence.NumberOfTags;
                if (tagsCovered / (double)distinctTagsCount > (tagWeightDistribution[tagWeightDistributionIndex - 1] + percentageCovered) * 0.01)
                    percentageCovered += tagWeightDistribution[(tagWeightDistributionIndex--) - 1];
			}

            return cssClassTable;
		}

		#endregion

        /// <summary>
        /// Gets the html for the tag cloud.
        /// </summary>
        public override string ToString()
        {
            using (HtmlTextWriter htw = new HtmlTextWriter(new StringWriter()))
            {
                htw.AddAttribute(HtmlTextWriterAttribute.Class, "TagCloud");
                htw.RenderBeginTag(HtmlTextWriterTag.Div);

                foreach (Tag tag in this.Tags)
                {
                    htw.Write(htw.NewLine);
                    if (!string.IsNullOrEmpty(tag.CssClass)) htw.AddAttribute(HtmlTextWriterAttribute.Class, tag.CssClass);
                    if (!string.IsNullOrEmpty(tag.ToolTip)) htw.AddAttribute(HtmlTextWriterAttribute.Title, tag.ToolTip);
                    if (tag.HtmlAttributes != null)
                        foreach (KeyValuePair<string, object> attribute in tag.HtmlAttributes)
                            htw.AddAttribute(attribute.Key, Convert.ToString(attribute.Value, CultureInfo.InvariantCulture));

                    htw.RenderBeginTag(HtmlTextWriterTag.Span);

                    if (!string.IsNullOrEmpty(tag.NavigateUrl))
                    {
                        htw.AddAttribute(HtmlTextWriterAttribute.Href, tag.NavigateUrl);
                        htw.RenderBeginTag(HtmlTextWriterTag.A);
                        htw.WriteEncodedText(tag.Text);
                        htw.RenderEndTag();
                    }
                    else
                        htw.WriteEncodedText(tag.Text);

                    htw.RenderEndTag();
                }

                htw.RenderEndTag();

                return ((StringWriter)htw.InnerWriter).ToString();
            }
        }

    }
}
