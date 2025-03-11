var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient(); // Esto permite inyectar HttpClient en el controlador
// Add services to the container.
builder.Services.AddControllers();


builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(80);    // HTTP
    serverOptions.ListenAnyIP(443);   // HTTPS
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();