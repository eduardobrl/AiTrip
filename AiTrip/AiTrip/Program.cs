using AiTrip.Client.Pages;
using AiTrip.Components;
using AiTrip.Domain.Entities;
using AiTrip.Domain.Interfaces;
using AiTrip.Domain.States;
using AiTrip.Infrastructure.Configurations;
using AiTrip.Infrastructure.Database;
using AiTrip.Infrastructure.OpenAi;
using AiTrip.Infrastructure.Secrets;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.Configure<DatabaseConfiguration>(
    builder.Configuration.GetSection(DatabaseConfiguration.Section));
builder.Services.Configure<OpenApiConfiguration>(
    builder.Configuration.GetSection(OpenApiConfiguration.Section));
builder.Services.Configure<VaultConfiguration>(
    builder.Configuration.GetSection(VaultConfiguration.Section));

builder.Services.AddSingleton<IRepository<Flight>, FlightRepository>();
builder.Services.AddSingleton<ISecretVault, AzureKeyVault>();
builder.Services.AddSingleton<ISearchService, SearchState>();
builder.Services.AddSingleton<IOpenAiService, AzureOpenApiService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Counter).Assembly);

app.Run();
