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
            logger.LogInformation($"Transação cadastrada.{transactions.Count}");
            Console.WriteLine(transactions.Count);
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
    .Accepts<Transaction>("application/json")
    .Produces(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status422UnprocessableEntity)
    .Produces(StatusCodes.Status400BadRequest)
    .WithName("AddTransaction")
    .WithOpenApi();

app.MapGet("/estatisca", (ILogger<Program> logger) =>
    {
        var lastTransactions = transactions.Where(t => 
            t.GetCreatedAt() > DateTime.Now.AddSeconds(-60));

        if (!lastTransactions.Any())
        {
            logger.LogInformation("Nenhuma transação nos ultimos 60 segundos");
            return Results.StatusCode(StatusCodes.Status422UnprocessableEntity);
        }
        
        var count = lastTransactions.Count();
        var sum = lastTransactions.Sum(t => t.GetValor());
        var average = lastTransactions.Average(t => t.GetValor());
        var max = lastTransactions.Max(t => t.GetValor());
        var min = lastTransactions.Min(t => t.GetValor());
        
        var statistics = new 
        {
            Count = count,
            Sum = sum,
            Average = average,
            Min = min,
            Max = max
        };
        
        logger.LogInformation("Estatísticas geradas.");
        return Results.Json(statistics);
    })
    .Produces(StatusCodes.Status200OK)
    .WithName("GetStatistics")
    .WithOpenApi();

app.MapDelete("/transacao", (ILogger<Program> logger) =>
    {
        logger.LogInformation($"Lista de transações apagada. {transactions.Count}");
        transactions.Clear();
        return Results.StatusCode(StatusCodes.Status200OK);
    })
    .WithName("ClearTransactions")
    .WithOpenApi();

app.Run();
