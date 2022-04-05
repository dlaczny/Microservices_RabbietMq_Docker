using WorkerService;
using WorkerService.Repository;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
#if DEBUG
        services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite(@"Data Source=..\..\..\..\Data\Visits.db"));

#else
        services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite(@"Data Source=../var/Visits.db"));

#endif
        services.AddHostedService<Worker>();
        services.AddSingleton<IVisitRepository, VisitRepository>();
    })
    .Build();

await host.RunAsync();