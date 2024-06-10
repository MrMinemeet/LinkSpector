/*
 * Copyright (c) 2024. Alexander Voglsperger
 * Licensed under the MIT License. See License in the project root for license information.
 */

using System.Diagnostics;

namespace LinkSpector;

/// <summary>
/// Helper class for logging messages.
/// </summary>
public class Logger
{
	private static Logger? _instance;
	public delegate void LogMessage(string message);
	private static LogMessage _logMessage = Console.WriteLine;
	
	/// <summary>
	/// Gets the singleton instance of the logger.
	/// Will only apply the delegate if the instance is not already created.
	/// By default, logs to the console.
	/// </summary>
	public static Logger GetInstance(LogMessage? loggerDelegate = null)
	{
		if (_instance != null)
		{
			return _instance;
		}

		if (loggerDelegate != null)
		{
			_logMessage = loggerDelegate;
		}
		
		_instance = new Logger();
		return _instance;

	}
	
	/// <summary>
	/// Logs an informational message.
	/// </summary>
	/// <param name="message">The message to log.</param>
	public void Info(string message)
	{
		_logMessage($"{GetTimestamp()} [INFO] {message}");
	}
	
	/// <summary>
	/// Logs an error message. Writes to the error stream.
	/// </summary>
	/// <param name="message">The message to log.</param>
	public void Error(string message)
	{
		_logMessage($"{GetTimestamp()} [ERROR] {message}");
	}
	
	/// <summary>
	/// Logs a debug message. Only logs in debug builds.
	/// </summary>
	/// <param name="message">The message to log.</param>
	[Conditional("DEBUG")]
	public void Debug(string message)
	{
		_logMessage($"{GetTimestamp()} [DEBUG] {message}");
	}
	
	/// <summary>
	/// Gets the current timestamp.
	/// </summary>
	/// <returns>Current time in the format 'dd-MM-yyyy HH:mm:ss.fff'.</returns>
	private static string GetTimestamp()
	{
		return DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.fff");
	}
}