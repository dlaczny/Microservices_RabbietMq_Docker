using API.Models;
using API.Repository;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Test
{
    public class AppDbContext : DbContext
    {
        public DbSet<Visit> Visits { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }

    public abstract class TestWithSqlite : IDisposable
    {
        private const string ConnectionString = @"Data Source=..\..\..\..\Data\Visits.db";
        private readonly SqliteConnection _connection;

        protected readonly AppDbContext DbContext;

        protected TestWithSqlite()
        {
            _connection = new SqliteConnection(ConnectionString);
            _connection.Open();
            var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseSqlite(_connection)
                    .Options;
            DbContext = new AppDbContext(options);
            DbContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }

    public class ToDoDbContextTests : TestWithSqlite
    {
        [Fact]
        public async Task DatabaseIsAvailableAndCanBeConnectedTo()
        {
            Assert.True(await DbContext.Database.CanConnectAsync());
        }

        [Fact]
        public async Task WorkerServiceTestAsync()
        {
            List<string> countries = new List<string>();
            countries.Add("pl");
            countries.Add("en");
            countries.Add("se");

            Random rnd = new Random();
            int r = rnd.Next(countries.Count);

            Visit visitCreateDto = new Visit()
            {
                Country = countries[r],
                Date = DateTime.Now,
                Visitors = rnd.Next(1, 1000)
            };
            var guid = Guid.NewGuid();

            using (StreamWriter file = System.IO.File.CreateText(@"..\..\..\..\Data\Data\" + guid + ".json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, visitCreateDto);
            }

            MessageBusRepository messageBusRepository = new MessageBusRepository();

            await messageBusRepository.AddMessage(guid.ToString());

            Thread.Sleep(3000);

            var visitFromDb = DbContext.Visits
                   .OrderByDescending(p => p.Id)
                   .FirstOrDefault();

            Assert.Equal(visitCreateDto.Country, visitFromDb.Country);
            Assert.Equal(visitCreateDto.Date.ToUniversalTime(), visitFromDb.Date);
            Assert.Equal(visitCreateDto.Visitors, visitFromDb.Visitors);
        }
    }
}