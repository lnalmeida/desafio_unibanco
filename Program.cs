using DesafioUnibanco.Entities;
using Microsoft.Extensions.Logging.Console;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var transactions = new List<Transaction>();

app.MapPost("/transacao", async (HttpContext context, ILogger<Program> logger) =>
    {
        try
        {
            using var reader = new StreamReader(context.Request.Body);
            var body = await reader.ReadToEndAsync();
            
            var transaction = System.Text.Json.JsonSerializer.Deserialize<Transaction>(body, 
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            transactions.Add(transaction);
            logger.LogInformation("Transação cadastrada.");
            return Results.StatusCode(StatusCodes.Status201Created);
        }
        catch (ArgumentException e)
        {
            logger.LogError(e.Message, "Erro ao cadastrar transação.");
            return Results.StatusCode(StatusCodes.Status422UnprocessableEntity);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message, "Erro ao cadastrar transação.");
            return Results.StatusCode(StatusCodes.Status400BadRequest);
        }
    })
    .WithName("AddTransaction")
    .WithOpenApi();

app.Run();
