var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddSingleton<LegadoCultural.Services.ICatalogoService, LegadoCultural.Services.CatalogoService>();

builder.Services.AddSingleton<LegadoCultural.Services.IVisualizacaoService, LegadoCultural.Services.VisualizacaoService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
