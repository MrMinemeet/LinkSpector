using System.Diagnostics;

namespace LinkSpector;

static class Program
{
	static int Main(string[] args)
	{
		if (args.Length != 1)
		{
			Console.Error.WriteLine("Usage: ./LinkSpector <rootUri>");
			return 1;
		}
		Uri rootUri = ParseUri(args[0]);
		
		// Create a new instance of the LinkSpector class
		LinkSpector linkSpector = new(rootUri);

		Stopwatch stopwatch = Stopwatch.StartNew();
		// Run the LinkSpector
		linkSpector.Run();
		stopwatch.Stop();
		Logger.Debug($"LinkSpector completed in {stopwatch.ElapsedMilliseconds}ms");

		List<LinkSpectorResult> results = linkSpector.Results;
		
		int totalResults = results.Count;
		int okResults = results.Count(r => r.StatusCode == 200);
		int errorResults = results.Count(r => r.StatusCode != 200);
		
		Console.WriteLine($"\ud83d\udd0e {totalResults} Total (in {stopwatch.ElapsedMilliseconds}ms) - \u2705 {okResults} OK, \u26d4 {errorResults} Errors");
		return 0;
	}
	
	private static Uri ParseUri(string uri)
	{
		if (Uri.TryCreate(uri, UriKind.Absolute, out Uri? result))
		{
			return result;
		}
		
		throw new ArgumentException("Invalid URI", nameof(uri));
	}
}