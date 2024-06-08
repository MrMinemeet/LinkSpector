/*
 * Copyright (c) 2024. Alexander Voglsperger
 * Licensed under the MIT License. See License in the project root for license information.
 */

using System.Net;

namespace LinkSpector;

/// <summary>
/// Represents the result of a request.
/// </summary>
/// <param name="Uri">The checked URI.</param>
/// <param name="StatusCode">The status code of the response.</param>
/// <param name="StatusDescription">The status description of the response.</param>
/// <param name="IsQuirky">True if something is quirky, false otherwise.</param>
public record LinkSpectorResult(
	Uri Uri,
	int StatusCode,
	string StatusDescription,
	bool IsQuirky
)
{
	public override string ToString()
	{
		return $"[{StatusCode}] {Uri} - {StatusDescription}";
	}
	
	public LinkSpectorResult(Uri uri, LinkSpector.HttpCodes statusCode, string statusDescription, bool IsQuirky=false) : this(uri, (int)statusCode, statusDescription, IsQuirky) { }
	public LinkSpectorResult(Uri uri, HttpStatusCode statusCode, string statusDescription, bool IsQuirky=false) : this(uri, (int)statusCode, statusDescription, IsQuirky) { }
	
	
}