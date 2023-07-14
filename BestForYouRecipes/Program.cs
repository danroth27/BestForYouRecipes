using BestForYouRecipes;
using BestForYouRecipes.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddServerComponents()
    .AddWebAssemblyComponents();
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

app.MapRazorComponents<App>()
    .AddServerRenderMode()
    .AddWebAssemblyRenderMode();

app.Map("images/uploaded/{filename}", async (string filename, IRecipesStore recipeStore) =>
    Results.File(await recipeStore.GetImage(filename), "image/jpeg"));

app.MapPost("api/recipes", async (Recipe recipe, IRecipesStore recipeStore) =>
    await recipeStore.AddRecipe(recipe)); // TODO: Validate https://github.com/dotnet/aspnetcore/issues/46349

app.MapPost("api/images", async (HttpRequest req, IRecipesStore recipeStore) =>
    await recipeStore.AddImage(req.Body));

app.Run();
