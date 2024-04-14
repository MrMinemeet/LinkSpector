using System.Text.RegularExpressions;

namespace LinkSpector;

public static class Quirks
{
	// X (formals Twitter) 
	private const string X_TWITTER_REGEX = @"https?://(?:www\.)?(?:twitter|x)\.com/.*";
	// Youtube Videos
	private const string YOUTUBE_REGEX = @"https?://(?:www\.)?youtube\.com/watch\?v=(?<id>.*)";
	private const string YOUTUBE_SHORT_REGEX = @"https?://youtu\.be/(?<id>.*)";
	
	/// <summary>
	/// Checks if the URI is excluded from the crawl.
	/// </summary>
	/// <param name="uri">The URI to check.</param>
	/// <returns>True if the URI is excluded, false otherwise.</returns>
	public static bool IsExcluded (Uri uri)
	{
		if (Regex.IsMatch(uri.AbsoluteUri, X_TWITTER_REGEX)) // Requires a logged-in user (and returns a 200) all the time
		{
			return true;
		}
		// TODO: Maybe add more Excludes
		return false;
	}


	/// <summary>
	/// Some Addresses may require some quirks to check if the content is available.
	/// </summary>
	/// <param name="uri">The URI to check.</param>
	/// <returns>The quirks URI if a quirk was found, the original URI otherwise.</returns>
	public static Uri IsQuirkyAddress(Uri uri)
	{
		#region YouTube
		/*
		 * In case of YouTube, the page always returns a 200 and then may display "This video isn't available anymore".
		 * Remembering the talk by Matthias Endler at RustLinz 2023, the workaround is to check if the thumbnail exists.
		 * This is also the workaround used in Lychee: https://github.com/lycheeverse/lychee/blob/master/lychee-lib/src/quirks/mod.rs#L43-L68
		 */
		Match match = Regex.Match(uri.AbsoluteUri, YOUTUBE_SHORT_REGEX);
		if (match.Success)
		{
			return new Uri($"https://www.youtube.com/watch?v={match.Groups["id"].Value}");
		}
		match = Regex.Match(uri.AbsoluteUri, YOUTUBE_REGEX);
		if (match.Success)
		{
			return new Uri($"https://www.youtube.com/watch?v={match.Groups["id"].Value}");
		}
		#endregion
		
		// If no quirk was found, return the original URI
		return uri;
	}
}