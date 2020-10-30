This text file provides a quick start guide on how to implement and use the TagCloud in a project.

**** Quick start ************************************************************************************

1.	Add a refenrence to the TagCloud.dll in your project

2.	Paste the CSS text from the bottom of this help text file into your existing CSS file.

3.	In a aspx file, include the following 2 lines:
	<%@ Import Namespace="TagCloud" %>
	<%@ Import Namespace="System.Collections.Generic" %>
	(right under the "<%@ Page ..." -line at the top of the page)

4.	Insert the following where you want the tag cloud to appear:
    <%= new TagCloud(   new Dictionary<string, int> {
                            {"C#", 58},
                            {"ASP.NET", 45},
                            {"VB.NET", 36},
                            {"AJAX", 24},
                            {"LINQ", 13}
                        },
					    new TagCloudGenerationRules
					    {
						    TagToolTipFormatString = "Tag weight: {0}",
						    TagUrlFormatString = "/ProductSearch.aspx?tag={0}"
					    }) %>

For more details, see the following sections.

**** More on implementing the TagCloud ************************************************************************************

For a code behind implementation you can render the tag cloud like this:
HtmlGenericControl div = new HtmlGenericControl("div");
div.InnerHtml = new TagCloud.TagCloud(...);
Controls.Add(div);
(The dots need to be replaced with actual parameters, read section "Using the TagCloud" below.)
Note: A more reusable implementation could be achieved by encapsulating the TagCloud classes in a
System.Web.UI.Control derived class. For more information on creating custom web server controls, see:
http://msdn.microsoft.com/en-us/library/ms178357(VS.80).aspx. For this you need the TagCloud source code.

If you are using ASP.NET MVC it could be more "MVC like" to add a couple of extension methods to the 
System.Web.Mvc.HtmlHelper class. To do this, use the code in the attached file "HtmlHelperExtensions.cs.txt". 
Create a new code file in your main project namned "HtmlHelperExtensions.cs" and in it, paste the content 
of the "HtmlHelperExtensions.cs.txt" file. Also, change the namespace name in the file to match your project.
You should now also be able to use the following:
<%= Html.TagCloud(...) %>
(The dots need to be replaced with actual parameters, read section "Using the TagCloud" below.)


**** Using the TagCloud ************************************************************************************

The tag cloud needs tags as data input. There are 3 ways to provide these tags.

1.	Provide a dictionary that takes a string for the tag text (as the dictionary key) and an integer for the 
	tag weight (as the dictionary value). This overload is suitable when you have a list of already weighted 
	tags, i.e. from a database query result. This is probably the most common choice of methods.

	Example:
    <%= new TagCloud(   new Dictionary<string, int> {
                            {"C#", 58},
                            {"ASP.NET", 45},
                            {"VB.NET", 36}
                        },
                        TagCloudGenerationRules.Default) %>
                        
    (Note: It is also possible to provide a list of KeyValuePair<string, int> objects)
                        
2.	Provide a list of tags as strings. The more times a tag occures in the list, the larger weight it will get 
	in the tag cloud.
	
	Example 1:
    <%= new TagCloud(someText.Split(' '), TagCloudGenerationRules.Default) %>
	
	Example 2:
    <%= new TagCloud(   new[] { "C#", "C#", "C#", "ASP.NET", "ASP.NET", "VB.NET"},
                        TagCloudGenerationRules.Default) %>
                        
	(In the example above, "C#" will get the weight of 3, "ASP.NET" the weight of 2 and "VB.NET" the weight of 1)

3.	Manually provide a list of Tag objects:
	You can directly provide a list of Tag objects with their properties set to exactly what you want each and every 
	tag to display. Use this to have full control of the rendering.
	
	Example:
	<%= new TagCloud(new[] {    new Tag(){ Text = "C#", CssClass="TagWeight1" },
								new Tag(){ Text = "ASP.NET", CssClass="TagWeight2" },
								new Tag(){ Text = "VB", CssClass="TagWeight3" } }) %>

The TagCloudGenerationRules parameter:
In the first two alternatives, you have to provide a TagCloudGenerationRules object as a second parameter. This
object tells the tag cloud how to generate the tags from your data.
The following example sets all of the options of the TagCloudGenerationRules object, thus overwriting their default settings:
<%= new TagCloud(   new Dictionary<string, int> {
                        {"C#", 58},
                        {"ASP.NET", 45},
                        {"VB.NET", 36}
                    },
                    new TagCloudGenerationRules
                    {
                        TagToolTipFormatString = "Tag count: {0}",
                        TagUrlFormatString = "/Articles/Search?searchText={0}",
                        MaxNumberOfTags = 30,
                        Order = TagCloudOrder.WeightDescending,
                        RequiredTagWeight = 10,
                        TagCssClassPrefix = "TagWeightGroup",
                        WeightClassPartitioning = new ReadOnlyCollection<int>(new []{10, 30, 30, 30}),
                        HtmlAttributes = new Dictionary<string, object> { { "style", "font-weight:bold;" } }
                    }) %>
                    
The default settings for a TagCloudGenerationRules object are as follows:
TagToolTipFormatString = string.Empty	(Tags doesn't have tooltip per default)
TagUrlFormatString = string.Empty		(Tags aren't clickable per default)
MaxNumberOfTags = null					(No limit on how many tags will get rendered)
Order = TagCloudOrder.Alphabetical		(The tags that makes it into the cloud gets ordered alphabetically ascending).
RequiredTagWeight = 1					(All tags will get rendered since 1 is the lowest weight requirement possible)
TagCssClassPrefix = "TagWeight"			(CSS classes for the different weight groups will be prefixed with TagWeight and then a number, i.e. "TagWeight1"),
HtmlAttributes = null					(No additional html attributes are added)
WeightClassPartitioning = new ReadOnlyCollection<int>(new []{10, 15, 20, 25, 30})
										(The 10% most high weighted tags gets the most high weighted css class (in this case "TagWeight1") and
										the 30% most low weighted tags gets the most low weighted css class (in this case "TagWeight5"))
                    
For more information on each option, see the code documentation in source code or intellisense.

**** CSS Support ************************************************************************************

The TagCloud renders with a parent div that looks for a TagCloud CSS class. The tag cloud also uses
CSS classes to determine the styles of the different tag weights.
Paste the following lines of CSS text into your existing CSS file to provide basic styling of the tag cloud.

[Copy everything under this line]

/* TagCloud
----------------------------------------------------------*/

.TagCloud			/* Applies to the entire tag cloud */
{
	font-family:Trebuchet MS;
	border:1px solid #888;
	padding:3px; 
	text-align:center;
}

.TagCloud > span	/* Applies to each tag of the tag cloud */
{
	margin-right:3px;
	text-align:center;
}

.TagCloud > span.TagWeight1	/* Applies to the largest tags */
{
	font-size:40px;
}

.TagCloud > span.TagWeight2
{
	font-size:32px;
}

.TagCloud > span.TagWeight3
{
	font-size:25px;
}

.TagCloud > span.TagWeight4
{
	font-size:18px;
}

.TagCloud > span.TagWeight5	/* Applies to the smallest tags */
{
	font-size:12px;
}

[Copy everything above this line]