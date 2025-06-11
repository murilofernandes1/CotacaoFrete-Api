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

var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Urls.Clear();
app.Urls.Add($"http://*:{port}");

app.Run();

//http://localhost:5022/swagger esse é o link para rodar a api no navegador
