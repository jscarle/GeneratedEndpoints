namespace GeneratedEndpoints.Common;

internal readonly record struct RequestHandler(
    RequestHandlerClass Class,
    RequestHandlerMethod Method,
    string HttpMethod,
    string Pattern,
    EndpointConfiguration Configuration
);
