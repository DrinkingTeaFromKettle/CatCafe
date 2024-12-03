using CatCafe.Data;
using CatCafe.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using NuGet.Protocol;
using System;
using System.Data.Common;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace CatCafe.Tests
{
    public class CatManagmentTests:IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
    {
        /*private readonly HttpClient _httpClient;*/
        private readonly CustomWebApplicationFactory _factory;

        private Cat TestCat1 = new Cat { Name = "Test1", Age = 4, Description = "Test1", Adoptable = true, Status = CatStatus.Avaliable, DateOfAcquisition = DateTime.Parse("2024-11-11"), DateOfArrival = DateTime.Parse("2024-11-11") };
        private Cat TestCat2 = new Cat { Name = "Test2", Age = 14, Description = "Test2", Adoptable = false, Status = CatStatus.TemporarilyUnavailable, DateOfAcquisition = DateTime.Parse("2024-11-11"), DateOfArrival = DateTime.Parse("2024-11-11") };

        public CatManagmentTests(CustomWebApplicationFactory applicationFactory)
        {
            _factory = applicationFactory;
        }

        public async Task InitializeAsync()
        {
            using (var sp = _factory.Services.CreateScope())
            {
                var db = sp.ServiceProvider.GetRequiredService<CatCafeDbContext>();
                db.Database.EnsureCreated();

                db.Cats.Add(TestCat1);
                db.Cats.Add(TestCat2);

                await db.SaveChangesAsync();
                var cat = db.Cats.FirstOrDefault(c => c.Name == TestCat1.Name);

                if (!db.Cats.Any())
                    throw new Exception("Data wasn't added properly");
            }
        }

        public async Task DisposeAsync()
        {
            using (var sp = _factory.Services.CreateScope())
            {
                var db = sp.ServiceProvider.GetRequiredService<CatCafeDbContext>();
                db.Database.EnsureDeleted();

            }
        }

        public async Task SeedDatabaseWithTestData(CatCafeDbContext db)
        {
            
        }

        [Fact]
        public async Task GetCats_EndpointReturnSuccessAndCorrectContentType()
        {
            var httpClient = _factory.CreateClient();
            var response = await httpClient.GetAsync("/cats");
            var responsePage = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
             response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task AddCat_EndpointResturnSuccess()
        {
            var httpClient = _factory.CreateClient();
            var getPageResponse = await httpClient.GetAsync("/cats/create");
            var antiForgeryCookie = AntiForgeryTokenExtractor.ExtractAntiForgeryCookieValue(getPageResponse);
            var antiForgeryToken = AntiForgeryTokenExtractor.ExtractAntiForgeryToken(await getPageResponse.Content.ReadAsStringAsync());
           var postRequest = new HttpRequestMessage(HttpMethod.Post, "/cats/create");
            postRequest.Headers.Add("Cookie", new CookieHeaderValue(AntiForgeryTokenExtractor.AntiForgeryCookieName, antiForgeryCookie).ToString());
            var formCat = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>(AntiForgeryTokenExtractor.AntiForgeryFieldName,antiForgeryToken),
                new KeyValuePair<string, string>("Name","TestCat"),
                new KeyValuePair<string, string>("Age","9"),
                new KeyValuePair<string, string>("Description","TestDescriptomj"),
                new KeyValuePair<string, string>("Status", "Avaliable"),
                new KeyValuePair<string, string>("Adoptable", "false"),
                new KeyValuePair<string, string>("DateOfAcquisition", DateTime.Now.ToString()),
                new KeyValuePair<string, string>("DateOfArrival", DateTime.Now.ToString())

            }
            );
            postRequest.Content = formCat;



            var response = await httpClient.SendAsync(postRequest);

            response.EnsureSuccessStatusCode();

            var responsePage =  response.Content.ReadAsStringAsync().Result;
            Assert.True(responsePage.ToString().Contains("TestCat"));
        }


        [Fact]
        public async Task EditCat_EndpointResturnSuccess()
        {
            using var sp = _factory.Services.CreateScope();
            var db = sp.ServiceProvider.GetRequiredService<CatCafeDbContext>();

            if (db.Cats.Count() > 0)
            {
                Cat? cat = db.Cats.FirstOrDefault(c => c.Name == TestCat1.Name);
                var catId = cat!.Id.ToString();
                var httpClient = _factory.CreateClient();
                var getPageResponse = await httpClient.GetAsync("/cats/edit/"+catId);
                var antiForgeryCookie = AntiForgeryTokenExtractor.ExtractAntiForgeryCookieValue(getPageResponse);
                var antiForgeryToken = AntiForgeryTokenExtractor.ExtractAntiForgeryToken(await getPageResponse.Content.ReadAsStringAsync());
                var postRequest = new HttpRequestMessage(HttpMethod.Post, "/cats/edit/" + catId);
                postRequest.Headers.Add("Cookie", new CookieHeaderValue(AntiForgeryTokenExtractor.AntiForgeryCookieName, antiForgeryCookie).ToString());
                var formCat = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>(AntiForgeryTokenExtractor.AntiForgeryFieldName,antiForgeryToken),
                new KeyValuePair<string, string>("Id",catId),
                new KeyValuePair<string, string>("Name","TestEdit"),
                new KeyValuePair<string, string>("Age","9"),
                new KeyValuePair<string, string>("Description","DescriptionTest"),
                new KeyValuePair<string, string>("Status", cat.Status.ToString()),
                new KeyValuePair<string, string>("Adoptable", cat.Adoptable.ToString()),
                new KeyValuePair<string, string>("DateOfAcquisition", cat.DateOfAcquisition.ToString()),
                new KeyValuePair<string, string>("DateOfArrival",cat.DateOfArrival.ToString())
                }
                );

                postRequest.Content = formCat;


                var response = await httpClient.SendAsync(postRequest);
                response.EnsureSuccessStatusCode();

                var responsePage =  response.Content.ReadAsStringAsync().Result;
                Assert.True(responsePage.ToString().Contains("TestEdit"));
            }
              else
                  throw new Exception("Database is empty");



          }
        [Fact]
        public async Task DeleteCat_EndpointResturnSuccess()
        {
            using var sp = _factory.Services.CreateScope();
            var db = sp.ServiceProvider.GetRequiredService<CatCafeDbContext>();

            if (db.Cats.Count() > 0)
            {
                Cat? cat = db.Cats.FirstOrDefault(c => c.Name == TestCat1.Name);

                var httpClient = _factory.CreateClient();
                var getPageResponse = await httpClient.GetAsync("/cats/delete/" + cat!.Id.ToString());
                var antiForgeryCookie = AntiForgeryTokenExtractor.ExtractAntiForgeryCookieValue(getPageResponse);
                var antiForgeryToken = AntiForgeryTokenExtractor.ExtractAntiForgeryToken(await getPageResponse.Content.ReadAsStringAsync());
                var postRequest = new HttpRequestMessage(HttpMethod.Post, "/cats/delete/" + cat!.Id.ToString());
                postRequest.Headers.Add("Cookie", new CookieHeaderValue(AntiForgeryTokenExtractor.AntiForgeryCookieName, antiForgeryCookie).ToString());
                var formData = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>(AntiForgeryTokenExtractor.AntiForgeryFieldName,antiForgeryToken)
                });
                postRequest.Content = formData;
                var response = await httpClient.SendAsync(postRequest);
                response.EnsureSuccessStatusCode();
            }
            else
                throw new Exception("Database is empty");



        }
    }
}
