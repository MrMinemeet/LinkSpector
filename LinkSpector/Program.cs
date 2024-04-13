using System.Diagnostics;

namespace LinkSpector;

static class Program
{
	static async Task Main(string[] args)
	{
		Crawler c = new();
		List<Task> tasks = new();
		List <HttpResponseMessage> responses = new();
		
		
		for (int i = 0; i < 20; i++)
		{
			int i1 = i;
			tasks.Add(Task.Run(async () =>
			{
				Console.WriteLine($"Task {i1} started");
				Task<HttpResponseMessage> response = c.GetWebPage("https://wtf-my-code.works");
				Console.WriteLine($"Task {i1} waiting for response");
				await response;
				responses.Add(response.Result);
			}));
		}
		Console.WriteLine("Waiting for tasks to complete...");
		Stopwatch sw = Stopwatch.StartNew();
		await Task.WhenAll(tasks);
		sw.Stop();
		Console.WriteLine($"All tasks completed in {sw.ElapsedMilliseconds}ms");
		
		// Create a new instance of the LinkSpector class
		LinkSpector linkSpector = new(args);

		// Run the LinkSpector
		linkSpector.Run();

		List<Result> results = linkSpector.Results;
	}
}