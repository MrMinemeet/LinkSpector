namespace LinkSpector;

/// <summary>
/// Holds the options for the LinkSpector.
/// </summary>
public struct LinkSpectorOptions()
{
	/// <summary>
	/// The timeout in seconds.
	/// </summary>
	public int Timeout = 20;
	/// <summary>
	/// Allow insecure/no TLS certificates.
	/// </summary>
	public bool AllowInsecure = false;
	/// <summary>
	/// Recursively crawl the website.
	/// Will only crawl the root URI host recursively.
	/// </summary>
	public bool Recursive = false;
}