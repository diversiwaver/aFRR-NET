using MassTransit;
using MessageConsumer.Consumers;
using RabbitMQ.Client;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((host, services) =>
    {
        services.AddMassTransit(x =>
        {
            // [1] Uncomment, as well as [2], in case you want to debug and consume your own messages
            x.AddConsumer<TsoConsumer>(c =>
            {
                c.UseConcurrencyLimit(1);
                c.UseMessageRetry(f => f.Intervals(TimeSpan.FromMilliseconds(250), TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(1000)));
            });
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(host.Configuration["RabbitMQHost"], 5672, "ortfmwaf", h => {
                    h.Username(host.Configuration["RabbitMQUsername"]);
                    h.Password(host.Configuration["RabbitMQPassword"]);
                });
                // [2] Uncomment, as well as [1] in case you want to debug and consume your own messages
                cfg.ReceiveEndpoint("tso-signal-list", x =>
                {
                    x.ConfigureConsumeTopology = false;
                    x.Bind("TSOMessageHub.XML:TSOSignal", s =>
                    {
                        s.RoutingKey = "TSOSignal";
                        s.ExchangeType = ExchangeType.Topic;
                    });
                    x.ConfigureConsumer<TsoConsumer>(context);
                });
                cfg.ConfigureEndpoints(context);
            });
        });
    })
    .Build();

host.Run();

