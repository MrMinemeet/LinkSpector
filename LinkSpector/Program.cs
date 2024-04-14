using System.Diagnostics;

namespace LinkSpector;

static class Program
{
	static void Main(string[] args)
	{
		
		// TODO: Parse the command line arguments and retrieve the root URI from the arguments
		
		// Create a new instance of the LinkSpector class
		LinkSpector linkSpector = new(new Uri("https://wtf-my-code.works/"));

		Stopwatch stopwatch = Stopwatch.StartNew();
		// Run the LinkSpector
		linkSpector.Run();
		stopwatch.Stop();
		Console.WriteLine($"LinkSpector completed in {stopwatch.ElapsedMilliseconds}ms");

		List<LinkSpectorResult> results = linkSpector.Results;
		
		int totalResults = results.Count;
		int okResults = results.Count(r => r.StatusCode == 200);
		int errorResults = results.Count(r => r.StatusCode != 200);
		
		Console.WriteLine($"\ud83d\udd0e {totalResults} Total (in {stopwatch.ElapsedMilliseconds}ms) - \u2705 {okResults} OK, \u26d4 {errorResults} Errors");
		
		
	}
}