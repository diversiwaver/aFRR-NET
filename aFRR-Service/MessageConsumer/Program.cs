using BaseDataAccess.Interfaces;
using DataAccessLayer;
using DataAccessLayer.Interfaces;
using MassTransit;
using MessageConsumer.Consumers;
using RabbitMQ.Client;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((host, services) =>
    {
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
        HttpClient prioritizationClient = new HttpClient() { BaseAddress = new Uri(host.Configuration["PrioritizationUri"]) };
        HttpClient remoteControlClient = new HttpClient() { BaseAddress = new Uri(host.Configuration["RemoteControlUri"]) };

        services.AddScoped((dataAccess) => DataAccessFactory.GetDataAccess<IPrioritizationDataAccess>(prioritizationClient));
        services.AddScoped((dataAccess) => DataAccessFactory.GetDataAccess<IRemoteControlDataAccess>(remoteControlClient));
        services.AddScoped((dataAccess) => DataAccessFactory.GetDataAccess<ISignalDataAccess>(host.Configuration.GetConnectionString("DefaultConnection")));
    })
    .Build();

host.Run();

