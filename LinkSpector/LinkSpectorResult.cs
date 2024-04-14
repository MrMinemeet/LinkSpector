namespace LinkSpector;

/// <summary>
/// Represents the result of a request.
/// </summary>
/// <param name="Uri">The checked URI.</param>
/// <param name="StatusCode">The status code of the response.</param>
/// <param name="StatusDescription">The status description of the response.</param>
public record LinkSpectorResult(
	Uri Uri,
	int StatusCode,
	string StatusDescription
);