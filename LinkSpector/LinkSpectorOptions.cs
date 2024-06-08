/*
 * Copyright (c) 2024. Alexander Voglsperger
 * Licensed under the MIT License. See License in the project root for license information.
 */

namespace LinkSpector;

/// <summary>
/// Holds the options for the LinkSpector.
/// </summary>
public struct LinkSpectorOptions()
{
	/// <summary>
	/// The timeout in seconds.
	/// </summary>
	public int Timeout = 20;
	/// <summary>
	/// Allow insecure/no TLS certificates.
	/// </summary>
	public bool AllowInsecure = false;
	/// <summary>
	/// Recursively crawl the website.
	/// Will only crawl the root URI host recursively.
	/// </summary>
	public bool Recursive = false;
}