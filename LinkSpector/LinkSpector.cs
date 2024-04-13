using System.Collections.ObjectModel;

namespace LinkSpector;

public class LinkSpector
{
	private readonly string[] _args;
	public List<Result> Results { get; private set; } = new();

	public LinkSpector(string[] args)
	{
		// TODO: Parse the command line arguments
		_args = args;
	}

	public void Run()
	{
		// TODO: Run the LinkSpector
		Console.WriteLine("Running LinkSpector...");
	}
}