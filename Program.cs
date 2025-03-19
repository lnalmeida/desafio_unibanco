using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using DesafioUnibanco.Domain.Entities;
using DesafioUnibanco.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;
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

var transactions = new TransactionList();

app.MapPost("/transacao", async (HttpContext context, ILogger<Program> logger) => 
    {
        try
        {
            using var reader = new StreamReader(context.Request.Body);
            var body = await reader.ReadToEndAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                IncludeFields = true
            };
            var transaction = System.Text.Json.JsonSerializer.Deserialize<Transaction>(body, options);
            
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
    .Accepts<Transaction>("application/json")
    .Produces(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status422UnprocessableEntity)
    .Produces(StatusCodes.Status400BadRequest)
    .WithName("AddTransaction")
    .WithOpenApi();

app.MapGet("/estatisca", (ILogger<Program> logger, [FromQuery] int seconds) =>
    {
        var lastTransactions = transactions.GetEstatistics(seconds);

        if (lastTransactions == null!)
        {
            logger.LogInformation("Nenhuma transação nos ultimos 60 segundos");
            return Results.StatusCode(StatusCodes.Status422UnprocessableEntity);
        }

        logger.LogInformation($"Estatísticas das transações feitas nos últimos {seconds} segundos geradas.");
        return Results.Json(lastTransactions);
    })
    .Produces(StatusCodes.Status200OK)
    .WithName("GetStatistics")
    .WithOpenApi();

app.MapDelete("/transacao", (ILogger<Program> logger) =>
    {
        logger.LogInformation("Lista de transações apagada.");
        transactions.ClearTransactionsList();
        return Results.StatusCode(StatusCodes.Status200OK);
    })
    .WithName("ClearTransactions")
    .WithOpenApi();

app.Run();
