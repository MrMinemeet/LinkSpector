/*
 * Copyright (c) 2024. Alexander Voglsperger
 * Licensed under the MIT License. See License in the project root for license information.
 */

using System.Reflection;

namespace LinkSpector;

public class Crawler
{
	private const string USER_AGENT = "LinkSpector";
	private readonly HttpClient _client;

	/// <summary>
	/// Initializes a new instance of the <see cref="Crawler"/> class.
	/// </summary>
	/// <param name="timeout">The timeout in seconds.</param>
	/// <param name="allowInsecure">if set to <c>true</c> allow insecure SSL certificates.</param>
	public Crawler(int timeout = 20, bool allowInsecure = false)
	{
		if (allowInsecure)
		{
			HttpClientHandler handler = new();
			handler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;
			_client = new(handler);
		}
		else
		{
			_client = new();
		}
		
		_client.Timeout = TimeSpan.FromSeconds(timeout);
		_client.DefaultRequestHeaders.Add("User-Agent", $"{USER_AGENT} v{Assembly.GetExecutingAssembly().GetName().Version}");
		_client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
	}
	
	/// <summary>
	/// Gets the web page.
	/// </summary>
	/// <param name="url">The URL.</param>
	/// <returns>A <see cref="Task"/> representing the HTTP response message.</returns>
	public Task<HttpResponseMessage> GetWebPage(string url)
	{
		return GetWebPage(new Uri(url));
	}
	
	/// <summary>
	/// Gets the web page.
	/// </summary>
	/// <param name="uri">The URI.</param>
	/// <returns>A <see cref="Task"/> representing the HTTP response message.</returns>
	public Task<HttpResponseMessage> GetWebPage(Uri uri)
	{
		return _client.GetAsync(uri);
	}
}