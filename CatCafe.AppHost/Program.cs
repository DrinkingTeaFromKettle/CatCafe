var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.CatCafe>("catcafe");

builder.Build().Run();
