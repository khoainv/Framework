using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SSD.Web.UI.Tags
{
    /// <summary>
    /// Provides a way to decide how a tag cloud is generated
    /// </summary>
    public class TagCloudGenerationRules
    {
        /// <summary>
        /// Creates a TagCloudGenerationRules object with default settings.
        /// </summary>
        public static TagCloudGenerationRules Default { get { return new TagCloudGenerationRules(); } }

        private string tagUrlFormatString;
        private int? maxNumberOfTags;
        private TagCloudOrder order;
        private int requiredTagWeight;
        private string tagToolTipFormatString;
        private string tagCssClassPrefix;
        private int[] weightClassPartitioning;

        /// <summary>
        /// Creates a TagCloudGenerationRules object with default settings.
        /// </summary>
        public TagCloudGenerationRules()
        {
            // Set defaults
			Order = TagCloudOrder.Alphabetical;
            RequiredTagWeight = 1;
            TagCssClassPrefix = "TagWeight";
            WeightClassPartitioning = new ReadOnlyCollection<int>(new []{10, 15, 20, 25, 30});
        }

        /// <summary>
        /// The formatting applied to the value in the NavigateUrl property of each Tag object. 
        /// If left null or empty, tags will not be links. Default is empty.
        /// All occurrences of "{0}" in this string gets replaced with the url encoded version of the Text property of this tag.
        /// </summary>
        public string TagUrlFormatString
        {
            get { return tagUrlFormatString ?? string.Empty; }
            set { tagUrlFormatString = value; }
        }

        /// <summary>
        /// Value used to determine tag sort order. Default is Alphabetical.
        /// </summary>
        public TagCloudOrder Order
        {
            get { return order; }
            set
            {
                if (!Enum.IsDefined(typeof(TagCloudOrder), value))
                    throw new ArgumentOutOfRangeException("Order"); //Throw exception for enum type safety

                order = value;
            }
        }

        /// <summary>
        /// Max number of tags in cloud. Provide null for unlimited. Default is null.
        /// </summary>
        public int? MaxNumberOfTags
        {
            get { return maxNumberOfTags; }
            set
            {
                if (maxNumberOfTags < 1)
                    throw new ArgumentException("Must be a positive integer.", "MaxNumberOfTags");

                maxNumberOfTags = value;
            }
        }

        /// <summary>
        /// The required weight for a tag to be included in cloud. Default is 1.
        /// </summary>
        public int RequiredTagWeight
        {
            get { return requiredTagWeight; }
            set
            {
                if (value < 1)
                    throw new ArgumentException("Must be a positive integer.", "RequiredTagWeight");

                requiredTagWeight = value;
            }
        }

        /// <summary>
        /// The formatting applied to the value in the TagCount property of each Tag object. 
        /// If left null or empty, tags will not have tooltips. Default is empty.
        /// All occurrences of "{0}" in this string gets replaced with the weight of the tag.
        /// </summary>
        public string TagToolTipFormatString
        {
            get { return tagToolTipFormatString ?? string.Empty; }
            set { tagToolTipFormatString = value; }
        }

        /// <summary>
        /// Decides the naming of the css classes that the different weight groups will get.
        /// The weight groups are in turn made up from the WeightClassPartitioning property.
        /// The name of the css classes are partly decided from this property, 
        /// and partly from the position it has in the WeightClassPartitioning list property.
        /// See WeightClassPartitioning documentation for more information.
        /// </summary>
        public string TagCssClassPrefix
        {
            get { return tagCssClassPrefix; }
            set 
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("TagCssClassPrefix must have a non empty string value.");
                tagCssClassPrefix = value; 
            }
        }
        
		/// <summary>
        /// Decides how the tag weights are partitioned in percent, from the highest to the lowest tag weight.
		/// Each percentage in list is given a css class. The name of the css classes are partly
        /// decided from the TagCssClassPrefix property, and partly from the position it has in this list. 
        /// Default is 10(%), 15(%), 20(%), 25(%), 30(%).
		/// </summary>
        /// <example>
        /// If TagCssClassPrefix is set to "TagWeight" and the first percentage in list is 50, 
        /// then the highest weighted half (50%) of the tags will get the css class named "TagWeight1".
        /// <code>
        /// new TagCloudGenerationRules
        /// {
        ///     TagCssClassPrefix = "TagWeight",
        ///     WeightClassPartitioning = new ReadOnlyCollection<int>(new[] { 50, 50 })"
        /// }
        /// </code>
        /// Above will make the tag cloud use 2 css classes, namely "TagWeight1" and "TagWeight2".
        /// The highest weighted half (50%) of the tags will use TagWeight1 and the lowest weighted half will use
        /// the TagWeight2 css class. You have to create these css classes in your existing css file.
        /// </example>
        public ReadOnlyCollection<int> WeightClassPartitioning
		{
			get { return new ReadOnlyCollection<int>(weightClassPartitioning); }
			set
			{
				if(value == null)
					throw new ArgumentNullException();
				if (value.Sum() != 100)
					throw new ArgumentException("The sum of percentages in list must be 100");

				weightClassPartitioning = value.ToArray();
			}
        }

        /// <summary>
        /// Gets or sets the additional html attributes to be applied for each tag.
        /// </summary>
        public IDictionary<string, object> HtmlAttributes { get; set; }
    }
}
