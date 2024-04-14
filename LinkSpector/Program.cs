namespace LinkSpector;

static class Program
{
	static void Main(string[] args)
	{
		
		// TODO: Parse the command line arguments and retrieve the root URI from the arguments
		
		// Create a new instance of the LinkSpector class
		LinkSpector linkSpector = new(new Uri("https://wtf-my-code.works/"));

		// Run the LinkSpector
		linkSpector.Run();

		List<Result> results = linkSpector.Results;
	}
}