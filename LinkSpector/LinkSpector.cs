using System.Net;
using System.Text.RegularExpressions;

namespace LinkSpector;

public class LinkSpector
{
	private const int EXCEPTION_DURING_REQUEST = -100;
	
	public List<LinkSpectorResult> Results { get; private set; } = new();
	private readonly Crawler _crawler;
	private readonly Uri _rootUri;

	public LinkSpector(Uri rootUri)
	{
		_crawler = new();
		_rootUri = rootUri;
	}

	public LinkSpector(string rootUri)
	{
		_crawler = new();
		_rootUri = new Uri(rootUri);
	}

	/// <summary>
	/// Runs the LinkSpector.
	/// </summary>
	public void Run()
	{
		Logger.Debug("Running LinkSpector...");
		List<HttpResponseMessage?> responseMessages = PerformCrawl().Result;
		foreach (HttpResponseMessage? response in responseMessages)
		{
			if (response == null)
			{
				Results.Add(new LinkSpectorResult(
					new Uri("http://unknown.local"),
					EXCEPTION_DURING_REQUEST,
					"Exception during request, see logs for more information"
				));
			}
			else
			{
				Uri uri = response.RequestMessage?.RequestUri ?? new Uri("http://unknown.local");
				Results.Add(new LinkSpectorResult(
					uri,
					(int)response.StatusCode,
					response.ReasonPhrase ?? "Unknown"
				));
			}
		}

		Logger.Debug("LinkSpector completed");
	}

	/// <summary>
	/// Performs the crawl asynchronously.
	/// </summary>
	/// <returns>A <see cref="Task"/> containing the list of HTTP response messages.</returns>
	private async Task<List<HttpResponseMessage?>> PerformCrawl()
	{
		List<Task> runningTasks = new();
		HashSet<Uri> toRequest = [_rootUri];
		Dictionary<Uri, HttpResponseMessage?> visited = new();

		Logger.Debug("Starting crawl...");
		while (toRequest.Count != 0)
		{
			#region Perform request batch

			Dictionary<Uri, HttpResponseMessage?> currentVisited = new();

			runningTasks.Clear();
			// Perform a batch of requests
			foreach (Uri uri in toRequest)
			{
				runningTasks.Add(Task.Run(async () =>
				{
					try
					{
						Logger.Debug($"Requesting '{uri}'");
						Task<HttpResponseMessage> response = _crawler.GetWebPage(uri);
						await response;
						Logger.Debug($"Received '{uri}'");
						currentVisited[uri] = response.Result;
					}
					catch (Exception e)
					{
						Logger.Error($"Error requesting '{uri}': {e.Message}");
						// Mark as visited with a null response
						currentVisited[uri] = null;
					}
				}));
			}

			// Wait for all tasks of the batch to finish
			await Task.WhenAll(runningTasks);
			runningTasks.Clear();

			// Remove the visited URIs from the list of URIs to request
			toRequest.Clear();

			# endregion

			# region Extract URIs from responses

			foreach ((Uri uri, HttpResponseMessage? response) in currentVisited)
			{
				// Skip if…
				if (
					response == null || // …the response is null
					response.StatusCode != HttpStatusCode.OK || // …the status code is not OK
					!uri.Host.Equals(_rootUri.Host) // …the host is not the same as the root URI
				)
				{
					continue;
				}

				Logger.Debug($"Extracting URIs from response from '{uri}'");


				runningTasks.Add(Task.Run(async () =>
				{
					HashSet<Uri> uris = await ExtractUrisFromResponse(response);
					foreach (Uri discoveredUri in uris.Where(u => !visited.ContainsKey(u)))
					{
						toRequest.Add(discoveredUri);
					}
				}));
			}

			await Task.WhenAll(runningTasks);
			runningTasks.Clear();

			currentVisited.ToList().ForEach(kv => visited[kv.Key] = kv.Value);

			#endregion
		}

		Logger.Debug("Crawl completed");
		return visited.Values.ToList();
	}

	/// <summary>
	/// Extracts URIs from the response.
	/// This method extracts URIs from the "href", "src" and "actino" attributes of the HTML content.
	/// </summary>
	/// <param name="response">The HTTP response message.</param>
	/// <returns>A <see cref="HashSet{T}"/> of URIs.</returns>
	private async Task<HashSet<Uri>> ExtractUrisFromResponse(HttpResponseMessage response)
	{
		HashSet<Uri> uris = new();
		if (response.Content.Headers.ContentType?.MediaType != "text/html") return uris;
		string content = await response.Content.ReadAsStringAsync();

		List<Match> matches = new();
		// TODO: Improve URI extraction, this is very basic and does not find all URIs in the content (Maybe match the URI itself instead of the attributes)
		// Match URIs in "href" attributes
		Regex.Matches(content, @"href=""(?<uri>[^""]*)""").ToList().ForEach(matches.Add);
		// Match URIs in "src" attributes
		Regex.Matches(content, @"src=""(?<uri>[^""]*)""").ToList().ForEach(matches.Add);
		// Match URIs in "action" attributes
		Regex.Matches(content, @"action=""(?<uri>[^""]*)""").ToList().ForEach(matches.Add);

		Logger.Debug($"Found {matches.Count} URIs in response of '{response.RequestMessage?.RequestUri}'");
		foreach (Match match in matches)
		{
			string uri = match.Groups["uri"].Value;
			// Check if the URI is relative
			if (!Uri.TryCreate(uri, UriKind.Absolute, out Uri? result))
			{
				Uri baseUri = response.RequestMessage?.RequestUri ?? new Uri("http://unknown.local");
				uris.Add(new Uri(baseUri, uri));
			}
			else
			{
				uris.Add(result);
			}
		}

		return uris;
	}
}