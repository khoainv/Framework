using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SSD.Web.UI.Tags;

namespace SSD.Web.UI.Html
{
    /// <summary>
    /// Provides extension methods for the HtmlHelper class in the System.Web.Mvc namespace.
    /// </summary>
    public static class TagExtensions
    {
        /// <summary>
        /// Creates tag cloud html from a provided list of string tags. 
        /// The more times a tag occures in the list, the larger weight it will get in the tag cloud.
        /// </summary>
        /// <param name="tags">A string list of tags</param>
        /// <param name="generationRules">A TagCloudGenerationRules object to decide how the cloud is generated.</param>
        /// <returns>A string containing the html markup of the tag cloud.</returns>
        public static MvcHtmlString TagCloud(this HtmlHelper htmlHelper, IEnumerable<string> tags, TagCloudGenerationRules generationRules)
        {
            return MvcHtmlString.Create(new TagCloud(tags, generationRules).ToString());
        }

        /// <summary>
        /// Creates tag cloud html from a provided dictionary of string tags along with integer values indicating the weight of each tag. 
        /// This overload is suitable when you have a list of already weighted tags, i.e. from a database query result.
        /// </summary>
        /// <param name="weightedTags">A dictionary that takes a string for the tag text (as the dictionary key) and an integer for the tag weight (as the dictionary value).</param>
        /// <param name="generationRules">A TagCloudGenerationRules object to decide how the cloud is generated.</param>
        /// <returns>A string containing the html markup of the tag cloud.</returns>
        public static MvcHtmlString TagCloud(this HtmlHelper htmlHelper, IDictionary<string, int> weightedTags, TagCloudGenerationRules generationRules)
        {
            return MvcHtmlString.Create(new TagCloud(weightedTags, generationRules).ToString());
        }

        /// <summary>
        /// Creates tag cloud html from a provided list of string tags along with integer values indicating the weight of each tag. 
        /// This overload is suitable when you have a list of already weighted tags, i.e. from a database query result.
        /// </summary>
        /// <param name="weightedTags">A list of KeyValuePair objects that take a string for the tag text and an integer for the weight of the tag.</param>
        /// <param name="generationRules">A TagCloudGenerationRules object to decide how the cloud is generated.</param>
        /// <returns>A string containing the html markup of the tag cloud.</returns>
        public static MvcHtmlString TagCloud(this HtmlHelper htmlHelper, IEnumerable<KeyValuePair<string, int>> weightedTags, TagCloudGenerationRules generationRules)
        {
            return MvcHtmlString.Create(new TagCloud(weightedTags, generationRules).ToString());
        }


        /// <summary>
        /// Creates tag cloud html from a provided list of Tag objects.
        /// Use this overload to have full control over the content in the cloud.
        /// </summary>
        /// <param name="tags">Tag objects used to generate the tag cloud.</param>
        /// <returns>A string containing the html markup of the tag cloud.</returns>
        public static MvcHtmlString TagCloud(this HtmlHelper htmlHelper, IEnumerable<Tag> tags)
        {
            return MvcHtmlString.Create(new TagCloud(tags).ToString());
        }
    }
}