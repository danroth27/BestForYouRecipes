using BestForYouRecipes;
using BestForYouRecipes.Components;
using BestForYouRecipes.Data;
using BestForYouRecipes.Pages;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddSingleton<IRecipesStore, RecipesStore>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(SubmitRecipe).Assembly);

app.Map("images/uploaded/{filename}", (string filename, IRecipesStore recipeStore) =>
    Results.Stream(body => recipeStore.DownloadImage(filename, body), "image/jpeg"));

app.MapPost("api/recipes", async (Recipe recipe, IRecipesStore recipeStore) =>
    await recipeStore.AddRecipe(recipe)); // TODO: Validate https://github.com/dotnet/aspnetcore/issues/46349

app.MapPost("api/images", async (Stream body, IRecipesStore recipeStore) =>
    await recipeStore.AddImage(body));

app.Run();
