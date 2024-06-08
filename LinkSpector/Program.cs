using System.Diagnostics;

namespace LinkSpector;

static class Program
{
	static int Main(string[] args)
	{
		if (args.Length == 0)
		{
			Console.Error.WriteLine("Usage: ./LinkSpector [options] <rootUri>");
			Console.Error.WriteLine("Options:");
			Console.Error.WriteLine("  -r\tRecursively crawl the website.");
			Console.Error.WriteLine("  -i\tAllow insecure/no TLS certificates.");
			Console.Error.WriteLine("  -t <number>\tTimeout in seconds.");
			
			return (int)ExitCode.ArgumentError;
		}
		Uri? rootUri = ParseUri(args[^1]); // = args[args.length - 1]
		if (rootUri == null)
		{
			Logger.Error("Invalid URI at last argument.");
			return (int)ExitCode.ArgumentError;
		}
		
		#region Parse argument 0 to n-1 as options
		LinkSpectorOptions options = new();
		if (args.Any(s => s == "-r"))
		{
			options.Recursive = true;
		}
		if (args.Any(s => s == "-i"))
		{
			options.AllowInsecure = true;
		}
		if (args.Any(s => s == "-t"))
		{
			int timeoutIndex = Array.IndexOf(args, "-t");
			if (timeoutIndex + 1 < args.Length)
			{
				if (int.TryParse(args[timeoutIndex + 1], out int timeout))
				{
					options.Timeout = timeout;
				}
				else
				{
					Logger.Error("Invalid timeout value.");
					return (int)ExitCode.ArgumentError;
				}
			}
			else
			{
				Logger.Error("No timeout value provided.");
				return (int)ExitCode.ArgumentError;
			}
		}
		#endregion

		#region Create a new instance of the LinkSpector and perform the crawl

		LinkSpector linkSpector = new(rootUri, options);
		CancellationTokenSource cts = new();
		Thread statusUpdateThread = new(() =>
		{
			int lastPagesVisited = 0;
			int lastPagesToVisit = 0;
			Logger.Debug("Starting status update thread...");
			while (!cts.IsCancellationRequested)
			{
				if (linkSpector.PagesVisited != lastPagesVisited || linkSpector.PagesToVisit != lastPagesToVisit)
				{
					lastPagesVisited = linkSpector.PagesVisited;
					lastPagesToVisit = linkSpector.PagesToVisit;
					Console.WriteLine($"{linkSpector.PagesVisited} out of {linkSpector.PagesToVisit} pages visited");
				}
				Thread.Sleep(500);
			}
		});
		Stopwatch stopwatch = Stopwatch.StartNew();
		// Run the LinkSpector
		statusUpdateThread.Start();
		linkSpector.Run();
		stopwatch.Stop();
		cts.Cancel();
		Logger.Debug($"LinkSpector completed in {stopwatch.ElapsedMilliseconds}ms");
		#endregion
		List<LinkSpectorResult> results = linkSpector.Results;
		
		int totalResults = results.Count;
		int okResults = results.Count(r => r.StatusCode == 200);
		int errorResults = results.Count(r => r.StatusCode != 200);
		
		Console.WriteLine($"\ud83d\udd0e {totalResults} Total (in {stopwatch.ElapsedMilliseconds}ms) - \u2705 {okResults} OK, \u26d4 {errorResults} Error(s)");
		
		foreach(LinkSpectorResult result in results.Where(r => r.StatusCode != 200 && r.StatusCode > 0 || r.IsQuirky))
		{
			Console.WriteLine(result);
		}
		
		return (int)ExitCode.Ok;
	}
	
	private static Uri? ParseUri(string uri)
	{
		return Uri.TryCreate(uri, UriKind.Absolute, out Uri? result) ? result : null;
	}
}