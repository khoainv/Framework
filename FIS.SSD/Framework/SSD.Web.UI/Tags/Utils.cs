using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace SSD.Web.UI.Tags
{
    /// <summary>
    /// Represents how the tag cloud can sort its tags
    /// </summary>
    public enum TagCloudOrder 
    {
        /// <summary>
        /// Renders tags alphabetically
        /// </summary>
        Alphabetical, 

        /// <summary>
        /// Renders tags alphabetically descending
        /// </summary>
        AlphabeticalDescending, 
        
        /// <summary>
        /// Renders tags with higher weight at the end
        /// </summary>
        Weight,

        /// <summary>
        /// Renders tags with higher weight at the beginning
        /// </summary>
        WeightDescending, 

        /// <summary>
        /// Renders tags with higher weight in the middle
        /// </summary>
        Centralized, 

        /// <summary>
        /// Renders tags with higher weight at the edges (start and end)
        /// </summary>
        Decentralized, 

        /// <summary>
        /// Renders tags rendomly
        /// </summary>
        Random 
    }

	/// <summary>
	/// Used to describe how many (in percent) of the tags in a tag cloud should have a given font size.
	/// </summary>
	public class FontSizeOccurrence 
	{
		public int FontSizeInPx { get; set; }
		public int OccurrenceInPercent { get; set; }
	}

	/// <summary>
	/// Comparer class that can be used to sort any object in a list randomly.
	/// </summary>
	internal class RandomComparer : IComparer<object>, IComparer 
	{
		public static RandomComparer Comparer { get { return new RandomComparer(); } }

		private static RNGCryptoServiceProvider rngcsp = new RNGCryptoServiceProvider();

		int IComparer.Compare(object x, object y)
		{
			return ((IComparer<object>)this).Compare(x, y);
		}

		int IComparer<object>.Compare(object x, object y)
		{
			if (x == y) return 0;

			byte[] randomByte = new byte[1];

			rngcsp.GetBytes(randomByte);

			return (randomByte[0] % 2 == 0) ? 1 : -1;
		}
	}
}
