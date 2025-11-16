using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Generated.Routing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointHandlers();

var app = builder.Build();

app.MapEndpointHandlers();

app.Run();
