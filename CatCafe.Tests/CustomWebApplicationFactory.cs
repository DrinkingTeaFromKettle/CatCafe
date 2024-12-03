using CatCafe.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Common;

namespace CatCafe.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                    typeof(DbContextOptions<CatCafeDbContext>));

                services.Remove(dbContextDescriptor);

                dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                    typeof(CatCafeDbContext));

                services.Remove(dbContextDescriptor);

                services.AddSingleton<DbConnection>(container =>
                {
                    var connection = new SqliteConnection("Filename=:memory:");
                    connection.Open();
                    return connection;
                });

                services.AddDbContext<CatCafeDbContext>((container, options) =>
                {
                    var connection = container.GetRequiredService<DbConnection>();
                    options.UseSqlite(connection);

                });
                services.AddAntiforgery(t =>
                {
                    t.Cookie.Name = AntiForgeryTokenExtractor.AntiForgeryCookieName;
                    t.FormFieldName = AntiForgeryTokenExtractor.AntiForgeryFieldName;
                });
                var sp = services.BuildServiceProvider();
            });

            
        }


    }
}
