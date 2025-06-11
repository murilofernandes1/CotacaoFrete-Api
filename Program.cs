var builder = WebApplication.CreateBuilder(args);

// Configura serviços
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware de Swagger (interface de teste no navegador)
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Clear();
app.Urls.Add($"http://*:{port}");

app.Run();

//https://cotacaofrete-api-1.onrender.com/swagger/index.html esse é o link para rodar a api no navegador
