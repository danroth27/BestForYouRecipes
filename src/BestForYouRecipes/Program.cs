using BestForYouRecipes;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http.Json;
using System.Text.Json;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var httpClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
var jsonOptions = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    AllowTrailingCommas = true
};
var recipes = await httpClient.GetFromJsonAsync<Dictionary<string, Recipe>>("recipes.json", jsonOptions);
if (recipes is null)
{
    throw new InvalidDataException("Failed to download recipes: recipes is null");
}

builder.Services.AddScoped(sp => httpClient);
builder.Services.AddSingleton<IRecipesStore>(new JsonRecipesStore(recipes));

await builder.Build().RunAsync();
