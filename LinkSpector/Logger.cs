/*
 * Copyright (c) 2024. Alexander Voglsperger
 * Licensed under the MIT License. See License in the project root for license information.
 */

using System.Diagnostics;

namespace LinkSpector;

/// <summary>
/// Helper class for logging messages.
/// </summary>
public static class Logger
{
	// TODO: Add ability to write to a file instead of the console
	
	/// <summary>
	/// Logs an informational message.
	/// </summary>
	/// <param name="message">The message to log.</param>
	public static void Info(string message)
	{
		Console.WriteLine($"{GetTimestamp()} [INFO] {message}");
	}
	
	/// <summary>
	/// Logs an error message. Writes to the error stream.
	/// </summary>
	/// <param name="message">The message to log.</param>
	public static void Error(string message)
	{
		Console.Error.WriteLine($"{GetTimestamp()} [ERROR] {message}");
	}
	
	/// <summary>
	/// Logs a debug message. Only logs in debug builds.
	/// </summary>
	/// <param name="message">The message to log.</param>
	[Conditional("DEBUG")]
	public static void Debug(string message)
	{
		Console.WriteLine($"{GetTimestamp()} [DEBUG] {message}");
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