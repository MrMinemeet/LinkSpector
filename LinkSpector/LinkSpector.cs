namespace LinkSpector;

public class LinkSpector
{
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
		List<HttpResponseMessage> responseMessages = PerformCrawl().Result;
		foreach (HttpResponseMessage response in responseMessages)
		{
			Uri uri = response.RequestMessage?.RequestUri ?? new Uri("http://unknown.local");
			Results.Add(new LinkSpectorResult(
				uri,
				(int)response.StatusCode,
				response.ReasonPhrase ?? "Unknown"
			));
		}

		Console.WriteLine("Running LinkSpector...");
	}

	/// <summary>
	/// Performs the crawl asynchronously.
	/// </summary>
	/// <returns>A <see cref="Task"/> containing the list of HTTP response messages.</returns>
	private async Task<List<HttpResponseMessage>> PerformCrawl()
	{
		List<Task> runningTasks = new();
		List<Uri> toRequest = [_rootUri];
		Dictionary<Uri, HttpResponseMessage> visited = new();

		while (toRequest.Count != 0)
		{
			runningTasks.Clear();
			// Perform a batch of requests
			foreach (Uri uri in toRequest)
			{
				runningTasks.Add(Task.Run(async () =>
				{
					Console.WriteLine($"Task requesting '{uri}'");
					Task<HttpResponseMessage> response = _crawler.GetWebPage(uri);
					Console.WriteLine($"Task '{uri}' completed");
					await response;
					visited[uri] = response.Result;
				}));
			}

			// Wait for all tasks of the batch to finish
			await Task.WhenAll(runningTasks);

			// Remove the visited URIs from the list of URIs to request
			toRequest.Clear();

			// TODO: Extract the URIs from the responses and add them to the list of URIs to request
		}

		return visited.Values.ToList();
	}
}