using Microsoft.AspNetCore.Mvc;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using RebusSagaSample.Api.Emails;
using RebusSagaSample.Api.Messages;
using RebusSagaSample.Api.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddRebus(rebus =>

    rebus
    .Routing(r => r.TypeBased().MapAssemblyOf<Program>("newsletter-queue"))
    .Transport(t => t.UseRabbitMq(
        builder.Configuration.GetConnectionString("RabbitMq"),
        inputQueueName: "newsletter-queue"))
    .Sagas(s => s.StoreInPostgres(
        builder.Configuration.GetConnectionString("Postgres"),
        dataTableName: "Sagas",
        indexTableName: "SagaIndexes"))
    .Timeouts(t => t.StoreInPostgres(
        builder.Configuration.GetConnectionString("Postgres"),
        tableName: "Timeouts"))
);

builder.Services.AutoRegisterHandlersFromAssemblyOf<Program>();

// Add services to the container.

var app = builder.Build();

app.MapPost("api/newsletters", async ([FromBody] EmailRequest email, IBus bus) =>
{
    await bus.Send(new SubscribeToNewsletter(email.Email));

    return Results.Accepted();
});


app.Run();
