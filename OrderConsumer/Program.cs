using GreenPipes;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(c =>
{
    c.AddConsumer<TicketConsumer>();

    c.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(c =>
    {
        c.UseHealthCheck(provider);

        c.Host(new Uri("rabbitmq://localhost"), h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        c.ReceiveEndpoint("orderTicketQueue", c =>
        {
            c.PrefetchCount = 10;
            c.UseMessageRetry(r => r.Intervals(2, 100));
            c.ConfigureConsumer<TicketConsumer>(provider);
        });
    }));
});

builder.Services.AddMassTransitHostedService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
