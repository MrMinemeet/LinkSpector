namespace LinkSpector;

public record Result(
	string Url,
	int StatusCode,
	string StatusDescription);