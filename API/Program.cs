using API;
using API.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#if DEBUG

builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlite(@"Data Source=..\..\..\..\Data\Visits.db"));

#else

builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlite(@"Data Source=../var/Visits.db"));
#endif

builder.Services.AddSingleton<IMessageBusRepository, MessageBusRepository>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//SeedDatabase();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();