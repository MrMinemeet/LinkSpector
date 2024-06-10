/*
 * Copyright (c) 2024. Alexander Voglsperger
 * Licensed under the MIT License. See License in the project root for license information.
 */

using System.Diagnostics;

namespace LinkSpector;

static class Program
{
	private static Logger? _logger;
	
	static int Main(string[] args)
	{
		if (args.Length == 0)
		{
			Console.Error.WriteLine("Copyright \u00a9\ufe0f 2024. Alexander Voglsperger");
			Console.Error.WriteLine("Usage: ./LinkSpector [options] <rootUri>");
			Console.Error.WriteLine("Options:");
			Console.Error.WriteLine("  -r\tRecursively crawl the website.");
			Console.Error.WriteLine("  -i\tAllow insecure/no TLS certificates.");
			Console.Error.WriteLine("  -t <number>\tTimeout in seconds.");
			Console.Error.WriteLine("  -l <file>\tLog to file.");
			
			return (int)ExitCode.ArgumentError;
		}
		
		int logFileIndex = Array.IndexOf(args, "-l");
		Logger.LogMessage? logMessage = null;
		
		#region Parse argument 0 to n-1 as options
		if (logFileIndex != -1)
		{
			logMessage = message =>
			{
				TextWriter logFile = new StreamWriter(args[logFileIndex + 1], append: true);
				logFile.WriteLine(message);
			};
		}
		_logger = Logger.GetInstance(logMessage);
		
		LinkSpectorOptions options = new();
		if (args.Any(s => s == "-r"))
		{
			options.Recursive = true;
		}
		
		if (args.Any(s => s == "-i"))
		{
			options.AllowInsecure = true;
		}
		
		int timeoutIndex = Array.IndexOf(args, "-t");
		if (timeoutIndex != -1)
		{
			if (timeoutIndex + 1 < args.Length)
			{
				if (int.TryParse(args[timeoutIndex + 1], out int timeout))
				{
					options.Timeout = timeout;
				}
				else
				{
					_logger.Error("Invalid timeout value.");
					return (int)ExitCode.ArgumentError;
				}
			}
			else
			{
				_logger.Error("No timeout value provided.");
				return (int)ExitCode.ArgumentError;
			}
		}
		#endregion
		
		// Parse the last argument as the root URI
		Uri? rootUri = ParseUri(args[^1]); // = args[args.length - 1]
		if (rootUri == null)
		{
			_logger.Error("Invalid URI at last argument.");
			return (int)ExitCode.ArgumentError;
		}

		#region Create a new instance of the LinkSpector and perform the crawl

		LinkSpector linkSpector = new(rootUri, options);
		CancellationTokenSource cts = new();
		Thread statusUpdateThread = new(() =>
		{
			int lastPagesVisited = 0;
			int lastPagesToVisit = 0;
			_logger.Debug("Starting status update thread...");
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
		_logger.Debug($"LinkSpector completed in {stopwatch.ElapsedMilliseconds}ms");
		#endregion
		List<LinkSpectorResult> results = linkSpector.Results;
		
		int totalResults = results.Count;
		int okResults = results.Count(r => r.StatusCode == 200);
		int errorResults = results.Count(r => r.StatusCode != 200 && r.StatusCode > 0 || r.IsQuirky);
		
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