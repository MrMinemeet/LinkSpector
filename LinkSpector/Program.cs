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

		List<Result> results = linkSpector.Results;
	}
}