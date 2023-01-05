using BaseDataAccess;
using DataAccessLayer.Interfaces;
using MassTransit;
using MessageConsumer.Consumers;
using RabbitMQ.Client;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((host, services) =>
    {
        string? connectionString = host.Configuration.GetConnectionString("DefaultConnection");
        if (connectionString is null)
        {
            // TODO: Log an error when retrieving the connection string
            throw new ArgumentNullException(nameof(connectionString), "Failed to retrieve connection string. Is it defined in the configuration?");
        }
        services.AddScoped((dataAccess) => DataAccessFactory.GetDataAccess<ISignalDataAccess>(connectionString));
        services.AddMassTransit(x =>
        {
            // [1] Uncomment, as well as [2], in case you want to debug and consume your own messages
            x.AddConsumer<TsoSignalConsumer>(c =>
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
                cfg.ReceiveEndpoint("tso-signal-list", configurator =>
                {
                    configurator.ConfigureConsumeTopology = false;
                    configurator.Bind("TSOMessageHub.XML:TSOSignal", s =>
                    {
                        s.RoutingKey = "TSOSignal";
                        s.ExchangeType = ExchangeType.Topic;
                    });
                    configurator.ConfigureConsumer<TsoSignalConsumer>(context);
                });
                cfg.ConfigureEndpoints(context);
            });
        });
    })
    .Build();

host.Run();

