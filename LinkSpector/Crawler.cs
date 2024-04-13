using System.Net.Cache;

namespace LinkSpector;

public class Crawler
{
	private const string USER_AGENT = "LinkSpector";
	private readonly HttpClient _client;
	

	/// <summary>
	/// Initializes a new instance of the <see cref="Crawler"/> class.
	/// </summary>
	/// <param name="acceptHeader">The Accept header.</param>
	/// <param name="timeout">The timeout in seconds.</param>
	public Crawler(string acceptHeader = "text/html", int timeout = 60)
	{
		_client = new();
		_client.Timeout = TimeSpan.FromSeconds(timeout);
		_client.DefaultRequestHeaders.Add("User-Agent", USER_AGENT);
		_client.DefaultRequestHeaders.Add("Accept", acceptHeader);
	}
	
	public Task<HttpResponseMessage> GetWebPage(string url)
	{
		return _client.GetAsync(url);
	}
}