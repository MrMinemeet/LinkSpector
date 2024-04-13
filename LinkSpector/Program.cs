namespace LinkSpector;

static class Program
{
	static void Main(string[] args)
	{
		// Create a new instance of the LinkSpector class
		LinkSpector linkSpector = new(args);

		// Run the LinkSpector
		linkSpector.Run();

		List<Result> results = linkSpector.Results;
	}
}