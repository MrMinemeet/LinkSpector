namespace LinkSpector;

public class Crawler
{
	private const string USER_AGENT = "LinkSpector";
	private readonly HttpClient _client;
	

	/// <summary>
	/// Initializes a new instance of the <see cref="Crawler"/> class.
	/// </summary>
	/// <param name="timeout">The timeout in seconds.</param>
	public Crawler(int timeout = 60)
	{
		_client = new();
		_client.Timeout = TimeSpan.FromSeconds(timeout);
		_client.DefaultRequestHeaders.Add("User-Agent", USER_AGENT);
	}
	
	/// <summary>
	/// Gets the web page.
	/// </summary>
	/// <param name="url">The URL.</param>
	/// <returns>A <see cref="Task"/> representing the HTTP response message.</returns>
	public Task<HttpResponseMessage> GetWebPage(string url)
	{
		return GetWebPage(new Uri(url));
	}
	
	/// <summary>
	/// Gets the web page.
	/// </summary>
	/// <param name="uri">The URI.</param>
	/// <returns>A <see cref="Task"/> representing the HTTP response message.</returns>
	public Task<HttpResponseMessage> GetWebPage(Uri uri)
	{
		return _client.GetAsync(uri);
	}
}